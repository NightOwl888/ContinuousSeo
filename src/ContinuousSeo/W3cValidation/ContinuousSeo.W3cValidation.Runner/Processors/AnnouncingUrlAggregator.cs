// -----------------------------------------------------------------------
// <copyright file="AnnouncingUrlAggregator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Parsers;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnnouncingUrlAggregator : TimingUrlAggregator
    {
        public AnnouncingUrlAggregator(IValidatorRunnerContext context, IProjectFileParser projectFileParser, ISitemapsParser sitemapsParser)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (projectFileParser == null)
                throw new ArgumentNullException("projectFileParser");
            if (sitemapsParser == null)
                throw new ArgumentNullException("sitemapsParser");

            this.mContext = context;
            this.mProjectFileParser = projectFileParser;
            this.mSitemapsParser = sitemapsParser;
        }

        public override IEnumerable<string> AggregateUrls()
        {
            var processName = "aggregating target URLs";
            OutputStartProcess(processName);

            var result = base.AggregateUrls();

            OutputEndProcess(processName);

            return result;
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
