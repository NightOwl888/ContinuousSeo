// -----------------------------------------------------------------------
// <copyright file="AnnouncingHtmlValidatorUrlProcessorForXmlOutput.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.UrlProcessors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Processors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnnouncingXmlOutputUrlProcessor : TimingXmlOutputUrlProcessor
    {

        // TODO: Inject announcer?

        public AnnouncingXmlOutputUrlProcessor(
            IValidatorWrapper validator,
            IRunnerContext context,
            IValidatorReportWriterFactory reportWriterFactory,
            IStreamFactory streamFactory)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (context == null)
                throw new ArgumentNullException("context");
            if (reportWriterFactory == null)
                throw new ArgumentNullException("reportWriterFactory");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");

            base.mValidator = validator;
            this.mContext = context;
            this.mReportWriterFactory = reportWriterFactory;
            this.mStreamFactory = streamFactory;
        }

        public override ValidationResult Process(IEnumerable<string> urls, string outputPath)
        {
            mContext.Announcer.Heading("Beginning processing with XML output");

            var result = base.Process(urls, outputPath);

            mContext.Announcer.Heading("Completed processing with XML output");
            mContext.Announcer.Say("Total Elapsed Time:");
            mContext.Announcer.ElapsedTime(mContext.TotalTimeStopwatch.Elapsed);
            mContext.Announcer.Heading(string.Format("Validation completed with {0} error(s) and {1} warning(s).", result.TotalErrors, result.TotalWarnings));

            return result;
        }

        protected override IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream)
        {
            // Report process start
            string processName = string.Format("validation for '{0}'", url);
            this.OutputStartProcess(processName);

            var report = base.ValidateUrl(url, writer, outputStream);

            // Report process completed
            this.OutputEndProcess(processName);

            return report;
        }

        private void OutputStartProcess(string processName)
        {
            mContext.Announcer.Say(string.Format("Starting {0}...", processName));
        }

        private void OutputEndProcess(string processName)
        {
            mContext.Announcer.Say(string.Format("Completed {0}.", processName));
            mContext.Announcer.ElapsedTime(mStopwatch.Elapsed);
        }
    }
}
