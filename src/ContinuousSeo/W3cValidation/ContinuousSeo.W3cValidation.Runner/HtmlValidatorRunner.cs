// -----------------------------------------------------------------------
// <copyright file="HtmlValidatonRunner.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.DI;
    using StructureMap;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRunner : IValidatorRunner
    {
        private readonly IHtmlValidatorRunnerContext mContext;
        protected IUrlAggregator mUrlAggregator;
        protected IUrlProcessor mProcessor;

        public HtmlValidatorRunner(IHtmlValidatorRunnerContext context, IUrlAggregator urlAggregator, IUrlProcessor processor) 
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (processor == null)
                throw new ArgumentNullException("processor");

            this.mContext = context;
            this.mUrlAggregator = urlAggregator;
            this.mProcessor = processor;
        }

        protected virtual void Intialize()
        {
        }

        #region IValidatorRunner Members

        public ValidationResult Execute()
        {
            Intialize();

            mContext.Announcer.Header("W3C HTML Validator");

            // Keep track of how long this takes
            mContext.TotalTimeStopwatch.Start();

            var urls = mUrlAggregator.AggregateUrls();
            var result = mProcessor.ProcessUrls(urls);

            mContext.Announcer.Header(string.Format("Validation completed with {0} error(s) and {1} warning(s).", result.TotalErrors, result.TotalWarnings));

            return result;
        }

        #endregion
    }
}
