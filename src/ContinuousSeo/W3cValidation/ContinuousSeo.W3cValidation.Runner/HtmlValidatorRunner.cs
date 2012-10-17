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
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.DI;
    using StructureMap;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRunner : IValidatorRunner
    {
        private readonly HtmlValidatorRunnerContext mContext;
        protected IUrlAggregator mUrlAggregator;
        protected IUrlProcessor mProcessor;

        public HtmlValidatorRunner(HtmlValidatorRunnerContext context, IUrlAggregator urlAggregator, IUrlProcessor processor)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (processor == null)
                throw new ArgumentNullException("processor");

            mContext = context;
            mUrlAggregator = urlAggregator;
            mProcessor = processor;
        }

        protected virtual void Intialize()
        {
            //var container = new Container();

            //// Setup configuration of DI
            //container.Configure(r => r.AddRegistry<HtmlValidatorRegistry>());
        }

        #region IValidatorRunner Members

        public void Execute()
        {
            Intialize();

            var urls = mUrlAggregator.AggregateUrls();
            mProcessor.ProcessUrls(urls);
        }

        #endregion
    }
}
