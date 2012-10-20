// -----------------------------------------------------------------------
// <copyright file="UrlProcessorFactory.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.UrlProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrlProcessorFactory : IUrlProcessorFactory
    {
        private readonly UrlProcessor mXmlProcessor;
        private readonly UrlProcessor mHtmlProcessor;

        public UrlProcessorFactory(
            UrlProcessor xmlProcessor,
            UrlProcessor htmlProcessor)
        {
            if (xmlProcessor == null)
                throw new ArgumentNullException("xmlProcessor");
            if (htmlProcessor == null)
                throw new ArgumentNullException("htmlProcessor");

            this.mXmlProcessor = xmlProcessor;
            this.mHtmlProcessor = htmlProcessor;
        }

        #region IUrlProcessorFactory Members

        public UrlProcessor GetUrlProcessor(string outputFormat)
        {
            outputFormat = (string.IsNullOrEmpty(outputFormat)) ? string.Empty : outputFormat.ToLowerInvariant();
            switch (outputFormat)
            {
                case "xml":
                    return this.mXmlProcessor;
                default:
                    return this.mHtmlProcessor;
            }
        }

        #endregion
    }
}
