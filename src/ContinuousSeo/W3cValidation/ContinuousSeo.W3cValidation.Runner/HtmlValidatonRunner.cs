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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatonRunner : IValidationRunner
    {
        private readonly HtmlValidatorRunnerContext mContext;
        protected IUrlProcessor mProcessor;

        public HtmlValidatonRunner(HtmlValidatorRunnerContext context, IUrlProcessor processor)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (processor == null)
                throw new ArgumentNullException("processor");

            mContext = context;
            mProcessor = processor;
        }

        protected virtual void Intialize()
        {

        }

        #region IValidationRunner Members

        public void Execute()
        {
            Intialize();

            mProcessor.ProcessUrls();
        }

        #endregion
    }
}
