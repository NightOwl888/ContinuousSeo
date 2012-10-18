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
        //protected IStopwatch mStopwatch;

        public HtmlValidatorRunner(IHtmlValidatorRunnerContext context, IUrlAggregator urlAggregator, IUrlProcessor processor) //, IStopwatch stopwatch
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (processor == null)
                throw new ArgumentNullException("processor");
            //if (stopwatch == null)
            //    throw new ArgumentNullException("stopwatch");

            this.mContext = context;
            this.mUrlAggregator = urlAggregator;
            this.mProcessor = processor;
            //this.mStopwatch = stopwatch;
        }

        protected virtual void Intialize()
        {
            //var container = new Container();

            //// Setup configuration of DI
            //container.Configure(r => r.AddRegistry<HtmlValidatorRegistry>());
        }

        #region IValidatorRunner Members

        public ValidationResult Execute()
        {
            Intialize();

            // Keep track of how long this takes
            mContext.TotalTimeStopwatch.Start();

            var urls = mUrlAggregator.AggregateUrls();
            var result = mProcessor.ProcessUrls(urls);

            return result;
        }

        #endregion
    }
}
