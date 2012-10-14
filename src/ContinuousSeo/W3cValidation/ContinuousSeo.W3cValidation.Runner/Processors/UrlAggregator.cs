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
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrlAggregator : IUrlAggregator
    {
        private readonly ISitemapsParser SitemapsParser;
        private readonly IUrlFileParser UrlFileParser;

        #region Constructor

        public UrlAggregator(IUrlFileParser urlFileParser, ISitemapsParser sitemapsParser)
        {
            if (urlFileParser == null)
                throw new ArgumentNullException("urlFileParser");
            if (sitemapsParser == null)
                throw new ArgumentNullException("sitemapsParser");

            this.UrlFileParser = urlFileParser;
            this.SitemapsParser = sitemapsParser;
        }

        #endregion

        #region IUrlAggregator Members

        public IEnumerable<string> AggregateUrls(HtmlValidatorRunnerContext context)
        {
            var urls = new List<string>();
            var lines = new List<IUrlFileLineInfo>();
            string[] args = context.UrlReplacementArgs.ToArray();

            AddTargetUrls(context.TargetUrls, urls, args);
            AddLinesFromTargetSitemapsFiles(context.TargetSitemapsFiles, lines, args);
            AddLinesFromTargetUrlFiles(context.TargetUrlFiles, lines, args);
            AddUrlsFromProcessedLines(lines, urls);

            return urls;
        }

        #endregion

        #region Private Members

        private IEnumerable<string> ProcessLine(IUrlFileLineInfo urlInfo)
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

        private void AddTargetUrls(IEnumerable<string> targetUrls, List<string> urls, string[] args)
        {
            // Add urls passed in directly from context
            foreach (var url in targetUrls)
            {
                urls.Add(string.Format(url, (object[])args));
            }
        }

        private void AddLinesFromTargetSitemapsFiles(IEnumerable<string> targetSitemapsFiles, List<IUrlFileLineInfo> lines, string[] args)
        {
            // Add sitemaps files passed in directly from context
            foreach (var file in targetSitemapsFiles)
            {
                var fileReplaced = string.Format(file, (object[])args);
                lines.Add(new UrlFileLineInfo(fileReplaced, "sitemaps"));
            }
        }

        private void AddLinesFromTargetUrlFiles(IEnumerable<string> targetUrlFiles, List<IUrlFileLineInfo> lines, string[] args)
        {
            foreach (var file in targetUrlFiles)
            {
                lines.AddRange(UrlFileParser.ParseFile(file, args));
            }
        }

        private void AddUrlsFromProcessedLines(IEnumerable<IUrlFileLineInfo> lines, List<string> urls)
        {
            // Process lines in file/passed in
            foreach (var line in lines)
            {
                urls.AddRange(ProcessLine(line));
            }
        }

        #endregion

    }
}
