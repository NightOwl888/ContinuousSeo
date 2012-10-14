// -----------------------------------------------------------------------
// <copyright file="HtmlUrlProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Output;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorUrlProcessor : IUrlProcessor
    {
        #region Private Members

        // validation
        private readonly IValidatorWrapper mValidator;
        private readonly HtmlValidatorRunnerContext RunnerContext;

        // parsing inputs
        private readonly IUrlAggregator UrlAggregator;

        // output
        private readonly IFileNameGenerator mFileNameGenerator; // HTML only
        private readonly ResourceCopier ResourceCopier; // HTML only
        private readonly IValidatorReportWriterFactory ReportWriterFactory;
        private readonly IStreamFactory mStreamFactory;

        #endregion

        #region Constructor

        public HtmlValidatorUrlProcessor(
            IValidatorWrapper validator, 
            HtmlValidatorRunnerContext runnerContext, 
            IUrlAggregator urlAggregator,
            IFileNameGenerator fileNameGenerator,
            ResourceCopier resourceCopier,
            IValidatorReportWriterFactory reportWriterFactory,
            IStreamFactory streamFactory)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (runnerContext == null)
                throw new ArgumentNullException("runnerContext");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (fileNameGenerator == null)
                throw new ArgumentNullException("fileNameGenerator");
            if (resourceCopier == null)
                throw new ArgumentNullException("resourceCopier");
            if (reportWriterFactory == null)
                throw new ArgumentNullException("reportWriterFactory");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");

            this.mValidator = validator;
            this.RunnerContext = runnerContext;
            this.UrlAggregator = urlAggregator;
            this.mFileNameGenerator = fileNameGenerator;
            this.ResourceCopier = resourceCopier;
            this.ReportWriterFactory = reportWriterFactory;
            this.mStreamFactory = streamFactory;
        }

        #endregion

        #region IUrlProcessor Members

        public void ProcessUrls()
        {
            var urls = UrlAggregator.AggregateUrls(RunnerContext);

            string outputFormat = (string.IsNullOrEmpty(RunnerContext.OutputFormat)) ? string.Empty : RunnerContext.OutputFormat.ToLowerInvariant();
            switch (outputFormat)
            {
                case "xml":
                    ValidateUrlsWithXmlOutput(urls);
                    break;
                default:
                    ValidateUrlsWithHtmlOutput(urls);
                    break;
            }  
        }

        #endregion

        #region Private Methods

        private void ValidateUrlsWithHtmlOutput(IEnumerable<string> urls)
        {
            IValidatorReportItem report;
            Stream outputStream;
            string outputPath = (RunnerContext.OutputPath == null) ? string.Empty : RunnerContext.OutputPath;
            string outputDirectory = string.Empty;
            bool areResourcesWritten = false;

            if (!string.IsNullOrEmpty(outputPath))
            {
                outputDirectory = Path.GetDirectoryName(outputPath);
            }

            using (Stream outputXmlReport = mStreamFactory.GetMemoryStream())
            {
                using (var writer = ReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
                {
                    writer.WriteStartDocument();

                    // Process aggregated urls
                    foreach (var url in urls)
                    {
                        string fileName = Path.Combine(outputDirectory, mFileNameGenerator.GenerateFileName(url, "html"));
                        using (outputStream = mStreamFactory.GetFileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Html);
                        }

                        report.FileName = Path.GetFileName(fileName);
                        writer.WriteUrlElement(report);

                        if (!areResourcesWritten)
                        {
                            ResourceCopier.CopyResources(Path.GetDirectoryName(outputPath));
                            areResourcesWritten = true;
                        }

                        if (mValidator.IsDefaultValidatorUrl())
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    writer.WriteEndDocument();

                }

                // TODO: If Html output, process xml report using Xslt here
                outputXmlReport.Position = 0;
                
                using (var output = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    outputXmlReport.CopyTo(output);
                }
            }
        }

        private void ValidateUrlsWithXmlOutput(IEnumerable<string> urls)
        {
            // Use entire output path (should be the complete path to xml file).
            string outputPath = (RunnerContext.OutputPath == null) ? string.Empty : RunnerContext.OutputPath;
            using (Stream outputXmlReport = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                using (var writer = ReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
                {
                    writer.WriteStartDocument();

                    // Process aggregated urls
                    foreach (var url in urls)
                    {
                        using (var outputStream = mStreamFactory.GetMemoryStream())
                        {
                            var report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Soap12);
                            outputStream.Position = 0;
                            writer.WriteUrlElement(report, outputStream);
                        }

                        if (mValidator.IsDefaultValidatorUrl())
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    writer.WriteEndDocument();
                }
            }
        }





        #endregion

    }
}
