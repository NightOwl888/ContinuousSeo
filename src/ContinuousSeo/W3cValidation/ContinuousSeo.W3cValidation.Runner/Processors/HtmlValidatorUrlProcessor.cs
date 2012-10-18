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
    using System.Diagnostics;
    using ContinuousSeo.Core;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.Core.Diagnostics;
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

        private readonly IValidatorWrapper mValidator;
        private readonly IHtmlValidatorRunnerContext mContext;
        private readonly IFileNameGenerator mFileNameGenerator; // HTML only
        private readonly ResourceCopier mResourceCopier; // HTML only
        private readonly IValidatorReportWriterFactory mReportWriterFactory;
        private readonly IStreamFactory mStreamFactory;
        private readonly IXslTransformer mXslTransformer; // HTML only
        private readonly Stopwatch mStopwatch = StopwatchProvider.Current.NewStopwatch();

        #endregion

        #region Constructor

        public HtmlValidatorUrlProcessor(
            IValidatorWrapper validator, 
            IHtmlValidatorRunnerContext context, 
            IFileNameGenerator fileNameGenerator,
            ResourceCopier resourceCopier,
            IValidatorReportWriterFactory reportWriterFactory,
            IStreamFactory streamFactory,
            IXslTransformer xslTransformer)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (context == null)
                throw new ArgumentNullException("context");
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
            this.mContext = context;
            this.mFileNameGenerator = fileNameGenerator;
            this.mResourceCopier = resourceCopier;
            this.mReportWriterFactory = reportWriterFactory;
            this.mStreamFactory = streamFactory;
            this.mXslTransformer = xslTransformer;
        }

        #endregion

        #region IUrlProcessor Members

        public ValidationResult ProcessUrls(IEnumerable<string> urls)
        {
            ValidationResult result = new ValidationResult();
            string outputFormat = (string.IsNullOrEmpty(mContext.OutputFormat)) ? string.Empty : mContext.OutputFormat.ToLowerInvariant();
            switch (outputFormat)
            {
                case "xml":
                    mContext.Announcer.Heading("Beginning processing with XML output");
                    result= ProcessXmlOutput(urls);
                    mContext.Announcer.Heading("Completed processing with XML output");
                    break;
                default:
                    mContext.Announcer.Heading("Beginning processing with HTML output");
                    result= ProcessHtmlOutput(urls);
                    mContext.Announcer.Heading("Completed processing with HTML output");
                    break;
            }
            mContext.Announcer.Say("Total Elapsed Time:");
            mContext.Announcer.ElapsedTime(mContext.TotalTimeStopwatch.Elapsed);

            return result;
        }

        #endregion

        #region Private Methods

        #region Html Output

        private ValidationResult ProcessHtmlOutput(IEnumerable<string> urls)
        {
            using (Stream outputXmlReport = mStreamFactory.GetMemoryStream())
            {
                return WriteXmlReportForHtmlOutput(outputXmlReport, urls);
            }
        }

        private ValidationResult WriteXmlReportForHtmlOutput(Stream outputXmlReport, IEnumerable<string> urls)
        {
            var result = new ValidationResult();
            var outputPath = GetOutputPathForHtmlOutput();

            using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
            {
                writer.WriteStartDocument();

                result = ValidateUrlsForHtmlOutput(writer, outputPath, urls);

                // report total elapsed time
                ReportTotalElapsedTime(writer);

                writer.WriteEndDocument();
                writer.Flush();

                // If Html output, process xml report using Xslt here
                // This must be done before the writer is closed, or the
                // stream will also be closed.
                outputXmlReport.Position = 0;
                WriteHtmlIndexFile(outputXmlReport, outputPath);
            }
            return result;
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
            string outputPath = (mContext.OutputPath == null) ? string.Empty : mContext.OutputPath;
            // Remove any filename from the path
            if (!string.IsNullOrEmpty(outputPath))
            {
                outputPath = Path.GetDirectoryName(outputPath);
            }
            return outputPath;
        }

        private ValidationResult ValidateUrlsForHtmlOutput(IValidatorReportTextWriter writer, string outputPath, IEnumerable<string> urls)
        {
            var result = new ValidationResult();
            bool first = true;

            if (urls.Count() > 0)
            {
                // Copy the images and CSS files for the HTML documents
                mResourceCopier.CopyResources(outputPath);
            }

            // Process urls
            foreach (var url in urls)
            {
                first = PauseIteration(first);
                AddResultTotals(result, ValidateUrlForHtmlOutput(writer, outputPath, url));
            }
            return result;
        }

        private IValidatorReportItem ValidateUrlForHtmlOutput(IValidatorReportTextWriter writer, string outputPath, string url)
        {
            IValidatorReportItem result = new ValidatorReportItem();

            // start stopwatch
            string processName = string.Format("validation for '{0}'", url);
            this.OutputStartProcess(processName);

            string fileName = Path.Combine(outputPath, mFileNameGenerator.GenerateFileName(url, "html"));
            using (var outputStream = mStreamFactory.GetFileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                result = mValidator.ValidateUrl(url, outputStream, OutputFormat.Html);
            }

            // Write the filename to the report
            result.FileName = Path.GetFileName(fileName);

            // report times to the report
            this.OutputEndProcess(processName);
            this.ReportElapsedTime(result, mStopwatch.Elapsed);

            writer.WriteUrlElement(result);

            return result;
        }

        #endregion

        #region Xml Output

        private ValidationResult ProcessXmlOutput(IEnumerable<string> urls)
        {
            // Use entire output path (should be the complete path to xml file).
            string outputPath = GetOutputPathForXmlOutput();

            using (Stream outputXmlReport = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
               return WriteXmlReportForXmlOutput(outputXmlReport, urls);
            }
        }

        private string GetOutputPathForXmlOutput()
        {
            var outputPath = (mContext.OutputPath == null) ? string.Empty : mContext.OutputPath;

            // Add default filename to output path if it was not provided.
            if (String.IsNullOrEmpty(Path.GetFileName(outputPath)))
            {
                outputPath = Path.Combine(outputPath, "report.xml");
            }
            return outputPath;
        }

        private ValidationResult WriteXmlReportForXmlOutput(Stream outputXmlReport, IEnumerable<string> urls)
        {
            var result = new ValidationResult();
            using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
            {
                writer.WriteStartDocument();
                result = ValidateUrlsForXmlOutput(writer, urls);

                // report total elapsed time
                ReportTotalElapsedTime(writer);

                writer.WriteEndDocument();
            }
            return result;
        }

        private ValidationResult ValidateUrlsForXmlOutput(IValidatorReportTextWriter writer, IEnumerable<string> urls)
        {
            var result = new ValidationResult();
            bool first = true;

            // Process urls
            foreach (var url in urls)
            {
                first = PauseIteration(first);
                AddResultTotals(result, ValidateUrlForXmlOutput(writer, url));
            }
            return result;
        }

        private IValidatorReportItem ValidateUrlForXmlOutput(IValidatorReportTextWriter writer, string url)
        {
            IValidatorReportItem result = new ValidatorReportItem();

            using (var outputStream = mStreamFactory.GetMemoryStream())
            {
                // Start stopwatch
                string processName = string.Format("validation for '{0}'", url);
                this.OutputStartProcess(processName);


                result = mValidator.ValidateUrl(url, outputStream, OutputFormat.Soap12);

                // Report times to the report
                this.OutputEndProcess(processName);
                this.ReportElapsedTime(result, mStopwatch.Elapsed);

                outputStream.Position = 0;
                writer.WriteUrlElement(result, outputStream);
            }

            return result;
        }

        #endregion

        private void AddResultTotals(ValidationResult totals, IValidatorReportItem report)
        {
            totals.TotalErrors += report.Errors;
            totals.TotalWarnings += report.Warnings;
        }

        private bool PauseIteration(bool first)
        {
            if (!first && mValidator.IsDefaultValidatorUrl())
            {
                Thread.Sleep(1000);
            }
            return false;
        }

        private void OutputStartProcess(string processName)
        {
            mContext.Announcer.Say(string.Format("Starting {0}...", processName));
            mStopwatch.Reset();
            mStopwatch.Start();
        }

        private void OutputEndProcess(string processName)
        {
            mStopwatch.Stop();
            mContext.Announcer.Say(string.Format("Completed {0}.", processName));
            mContext.Announcer.ElapsedTime(mStopwatch.Elapsed);
        }

        private IValidatorReportTimes ReportElapsedTime(TimeSpan elapsed)
        {
            var result = new ValidatorReportTimes();
            this.ReportElapsedTime(result, elapsed);
            return result;
        }

        private void ReportElapsedTime(IValidatorReportTimes report, TimeSpan elapsed)
        {
            DateTime utcNow = TimeProvider.Current.UtcNow;
            DateTime now = System.TimeZone.CurrentTimeZone.ToLocalTime(utcNow);
            report.LocalStartTime = now.Subtract(elapsed);
            report.LocalEndTime = now;
            report.UtcStartTime = utcNow.Subtract(elapsed);
            report.UtcEndTime = utcNow;
            report.ElapsedTime = string.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }

        private void ReportTotalElapsedTime(IValidatorReportTextWriter writer)
        {
            mContext.TotalTimeStopwatch.Stop();
            var elapsed = ReportElapsedTime(mContext.TotalTimeStopwatch.Elapsed);
            writer.WriteElapsedTime(elapsed);
        }

        #endregion

    }
}
