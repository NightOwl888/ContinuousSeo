// -----------------------------------------------------------------------
// <copyright file="UrlAggregator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ContinuousSeo.W3cValidation.Runner.Parsers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrlAggregator : IUrlAggregator
    {
        private readonly ISitemapsParser SitemapsParser;

        public UrlAggregator(ISitemapsParser sitemapsParser)
        {
            if (sitemapsParser == null)
                throw new ArgumentNullException("sitemapsParser");

            this.SitemapsParser = sitemapsParser;
        }

        #region IUrlAggregator Members

        public IEnumerable<string> ProcessLine(IUrlFileLineInfo urlInfo)
        {
            if (urlInfo == null)
                throw new ArgumentNullException("urlInfo");

            var result = new List<string>();

            switch (urlInfo.Mode)
            {
                case null:
                case "":
                case "single":
                    result.Add(urlInfo.Url);
                    break;

                case "sitemaps":
                    result.AddRange(SitemapsParser.ParseUrlsFromFile(urlInfo.Url));
                    break;

            }

            return result;
        }

        #endregion
    }
}
