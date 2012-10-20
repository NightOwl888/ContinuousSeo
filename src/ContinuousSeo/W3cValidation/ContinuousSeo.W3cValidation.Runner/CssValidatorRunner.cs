// -----------------------------------------------------------------------
// <copyright file="CssValidatorRunner.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner
{
    using System;
    using System.Collections.Generic;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.DI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CssValidatorRunner : IValidatorRunner
    {
        private readonly IValidatorRunnerContext mContext;
        protected IUrlAggregator mUrlAggregator;
        protected IUrlProcessorFactory mUrlProcessorFactory;

        public CssValidatorRunner(IValidatorRunnerContext context, IUrlAggregator urlAggregator, IUrlProcessorFactory urlProcessorFactory) 
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (urlProcessorFactory == null)
                throw new ArgumentNullException("urlProcessorFactory");

            this.mContext = context;
            this.mUrlAggregator = urlAggregator;
            this.mUrlProcessorFactory = urlProcessorFactory;
        }

        protected virtual void Intialize()
        {
            mContext.Announcer.Header("W3C CSS Validator");

            // Keep track of how long this takes
            mContext.TotalTimeStopwatch.Start();
        }

        #region IValidatorRunner Members

        public ValidationResult Execute()
        {
            Intialize();

            var urls = mUrlAggregator.AggregateUrls();
            var processor = mUrlProcessorFactory.GetUrlProcessor(mContext.OutputFormat);
            var result = processor.Process(urls, mContext.OutputPath);

            return result;
        }

        #endregion
    }
}
