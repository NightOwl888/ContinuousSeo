// -----------------------------------------------------------------------
// <copyright file="LoggingUrlProcessorHtmlOutput.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnnouncingHtmlOutputUrlProcessor : TimingHtmlOutputUrlProcessor
    {
        // TODO: Inject announcer?

        public AnnouncingHtmlOutputUrlProcessor(
            IValidatorWrapper validator,
            IRunnerContext context,
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

        public override ValidationResult Process(IEnumerable<string> urls, string outputPath)
        {
            mContext.Announcer.Heading("Beginning processing with HTML output");

            var result = base.Process(urls, outputPath);

            mContext.Announcer.Heading("Completed processing with HTML output");
            mContext.Announcer.Say("Total Elapsed Time:");
            mContext.Announcer.ElapsedTime(mContext.TotalTimeStopwatch.Elapsed);
            mContext.Announcer.Heading(string.Format("Validation completed with {0} error(s) and {1} warning(s).", result.TotalErrors, result.TotalWarnings));

            return result;
        }

        protected override IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream)
        {
            // start stopwatch
            string processName = string.Format("validation for '{0}'", url);
            this.OutputStartProcess(processName);

            var report = base.ValidateUrl(url, writer, outputStream);

            // report times to the report
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
