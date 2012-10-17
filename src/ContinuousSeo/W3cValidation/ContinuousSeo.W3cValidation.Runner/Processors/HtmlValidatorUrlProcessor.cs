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
    using System.Reflection;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.Core.IO;
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
        private readonly HtmlValidatorRunnerContext mRunnerContext;

        // output
        private readonly IFileNameGenerator mFileNameGenerator; // HTML only
        private readonly ResourceCopier mResourceCopier; // HTML only
        private readonly IValidatorReportWriterFactory mReportWriterFactory;
        private readonly IStreamFactory mStreamFactory;
        private readonly IXslTransformer mXslTransformer; // HTML only

        #endregion

        #region Constructor

        public HtmlValidatorUrlProcessor(
            IValidatorWrapper validator, 
            HtmlValidatorRunnerContext runnerContext, 
            IFileNameGenerator fileNameGenerator,
            ResourceCopier resourceCopier,
            IValidatorReportWriterFactory reportWriterFactory,
            IStreamFactory streamFactory,
            IXslTransformer xslTransformer)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (runnerContext == null)
                throw new ArgumentNullException("runnerContext");
            if (fileNameGenerator == null)
                throw new ArgumentNullException("fileNameGenerator");
            if (resourceCopier == null)
                throw new ArgumentNullException("resourceCopier");
            if (reportWriterFactory == null)
                throw new ArgumentNullException("reportWriterFactory");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");
            if (xslTransformer == null)
                throw new ArgumentNullException("xslTransformer");

            this.mValidator = validator;
            this.mRunnerContext = runnerContext;
            this.mFileNameGenerator = fileNameGenerator;
            this.mResourceCopier = resourceCopier;
            this.mReportWriterFactory = reportWriterFactory;
            this.mStreamFactory = streamFactory;
            this.mXslTransformer = xslTransformer;
        }

        #endregion

        #region IUrlProcessor Members

        public void ProcessUrls(IEnumerable<string> urls)
        {
            string outputFormat = (string.IsNullOrEmpty(mRunnerContext.OutputFormat)) ? string.Empty : mRunnerContext.OutputFormat.ToLowerInvariant();
            switch (outputFormat)
            {
                case "xml":
                    ProcessXmlOutput(urls);
                    break;
                default:
                    ProcessHtmlOutput(urls);
                    break;
            }  
        }

        #endregion

        #region Private Methods

        #region Html Output

        private void ProcessHtmlOutput(IEnumerable<string> urls)
        {
            using (Stream outputXmlReport = mStreamFactory.GetMemoryStream())
            {
                WriteXmlReportForHtmlOutput(outputXmlReport, urls);
            }
        }

        private void WriteXmlReportForHtmlOutput(Stream outputXmlReport, IEnumerable<string> urls)
        {
            var outputPath = GetOutputPathForHtmlOutput();

            using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
            {
                writer.WriteStartDocument();

                ValidateUrlsForHtmlOutput(writer, outputPath, urls);

                writer.WriteEndDocument();
                writer.Flush();

                // If Html output, process xml report using Xslt here
                // This must be done before the writer is closed, or the
                // stream will also be closed.
                outputXmlReport.Position = 0;
                WriteHtmlIndexFile(outputXmlReport, outputPath);
            }
        }

        private void WriteHtmlIndexFile(Stream outputXmlReport, string outputPath)
        {
            var xslFilePath = "ContinuousSeo.W3cValidation.Runner.HtmlValidatorIndex.xsl";
            using (Stream xsl = Assembly.GetExecutingAssembly().GetManifestResourceStream(xslFilePath))
            {
                mXslTransformer.Transform(outputXmlReport, xsl, Path.Combine(outputPath, "index.html"));
            }
        }

        private string GetOutputPathForHtmlOutput()
        {
            string outputPath = (mRunnerContext.OutputPath == null) ? string.Empty : mRunnerContext.OutputPath;
            // Remove any filename from the path
            if (!string.IsNullOrEmpty(outputPath))
            {
                outputPath = Path.GetDirectoryName(outputPath);
            }
            return outputPath;
        }

        private void ValidateUrlsForHtmlOutput(IValidatorReportTextWriter writer, string outputPath, IEnumerable<string> urls)
        {
            if (urls.Count() > 0)
            {
                // Copy the images and CSS files for the HTML documents
                mResourceCopier.CopyResources(outputPath);
            }

            // Process urls
            foreach (var url in urls)
            {
                ValidateUrlForHtmlOutput(writer, outputPath, url);
            }
        }

        private void ValidateUrlForHtmlOutput(IValidatorReportTextWriter writer, string outputPath, string url)
        {
            IValidatorReportItem report;
            string fileName = Path.Combine(outputPath, mFileNameGenerator.GenerateFileName(url, "html"));

            using (var outputStream = mStreamFactory.GetFileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Html);
            }

            // Write the filename to the report
            report.FileName = Path.GetFileName(fileName);
            writer.WriteUrlElement(report);

            if (mValidator.IsDefaultValidatorUrl())
            {
                Thread.Sleep(1000);
            }
        }

        #endregion

        #region Xml Output

        private void ProcessXmlOutput(IEnumerable<string> urls)
        {
            // Use entire output path (should be the complete path to xml file).
            string outputPath = GetOutputPathForXmlOutput();

            using (Stream outputXmlReport = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                WriteXmlReportForXmlOutput(outputXmlReport, urls);
            }
        }

        private string GetOutputPathForXmlOutput()
        {
            var outputPath = (mRunnerContext.OutputPath == null) ? string.Empty : mRunnerContext.OutputPath;

            // Add default filename to output path if it was not provided.
            if (String.IsNullOrEmpty(Path.GetFileName(outputPath)))
            {
                outputPath = Path.Combine(outputPath, "report.xml");
            }
            return outputPath;
        }

        private void WriteXmlReportForXmlOutput(Stream outputXmlReport, IEnumerable<string> urls)
        {
            using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
            {
                writer.WriteStartDocument();
                ValidateUrlsForXmlOutput(writer, urls);
                writer.WriteEndDocument();
            }
        }

        private void ValidateUrlsForXmlOutput(IValidatorReportTextWriter writer, IEnumerable<string> urls)
        {
            // Process urls
            foreach (var url in urls)
            {
                ValidateUrlForXmlOutput(writer, url);
            }
        }

        private void ValidateUrlForXmlOutput(IValidatorReportTextWriter writer, string url)
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

        #endregion

        #endregion

    }
}
