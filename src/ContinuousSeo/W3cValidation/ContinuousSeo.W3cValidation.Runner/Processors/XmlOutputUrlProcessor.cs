// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorProcessorForXmlOutput.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Output;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class XmlOutputUrlProcessor : UrlProcessor
    {
        protected IStreamFactory mStreamFactory;


        public override ValidationResult Process(IEnumerable<string> urls, string outputPath)
        {
            // Use entire output path (should be the complete path to xml file).
            outputPath = FixOutputPath(outputPath);

            using (Stream outputXmlReport = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                return WriteXmlReport(urls, outputXmlReport, outputPath);
            }
        }

        protected override string FixOutputPath(string outputPath)
        {
            if (outputPath == null) return string.Empty;

            // Add default filename to output path if it was not provided.
            if (String.IsNullOrEmpty(Path.GetFileName(outputPath)))
            {
                outputPath = Path.Combine(outputPath, "report.xml");
            }
            return outputPath;
        }

        protected override ValidationResult WriteXmlReport(IEnumerable<string> urls, Stream outputXmlReport, string outputPath)
        {
            var result = new ValidationResult();

            using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
            {
                writer.WriteStartDocument();
                result = ValidateUrls(urls, writer, outputPath);
                writer.WriteEndDocument();
            }
            return result;
        }

        protected override ValidationResult ValidateUrls(IEnumerable<string> urls, IValidatorReportTextWriter writer, string outputPath)
        {
            var result = new ValidationResult();

            // Process urls
            foreach (var url in urls)
            {
                using (var outputStream = mStreamFactory.GetMemoryStream())
                {
                    var report = ValidateUrl(url, writer, outputStream);
                    AddResultTotals(result, report);

                    outputStream.Position = 0;
                    writer.WriteUrlElement(report, outputStream);
                }
            }
            return result;
        }

        protected override IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream)
        {
            PauseIteration();
            return mValidator.ValidateUrl(url, outputStream, OutputFormat.Soap12);
        }

    }
}
