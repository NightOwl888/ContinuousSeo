// -----------------------------------------------------------------------
// <copyright file="mUrlAggregator.cs" company="">
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
        private readonly HtmlValidatorRunnerContext mContext;
        private readonly ISitemapsParser mSitemapsParser;
        private readonly IProjectFileParser mProjectFileParser;

        #region Constructor

        public UrlAggregator(HtmlValidatorRunnerContext context, IProjectFileParser projectFileParser, ISitemapsParser sitemapsParser)
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

        #endregion

        #region IUrlAggregator Members

        public IEnumerable<string> AggregateUrls()
        {
            var urls = new List<string>();
            var lines = new List<IProjectFileLineInfo>();
            string[] args = (mContext.UrlReplacementArgs == null) ? new string[0] : mContext.UrlReplacementArgs.ToArray();

            AddTargetUrls(mContext.TargetUrls, urls, args);
            AddLinesFromTargetSitemapsFiles(mContext.TargetSitemapsFiles, lines, args);
            AddLinesFromTargetProjectFiles(mContext.TargetProjectFiles, lines, args);
            AddUrlsFromProcessedLines(lines, urls);

            return urls;
        }

        #endregion

        #region Private Members

        private IEnumerable<string> ProcessLine(IProjectFileLineInfo urlInfo)
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
                    result.AddRange(mSitemapsParser.ParseUrlsFromFile(urlInfo.Url));
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

        private void AddLinesFromTargetSitemapsFiles(IEnumerable<string> targetSitemapsFiles, List<IProjectFileLineInfo> lines, string[] args)
        {
            if (targetSitemapsFiles == null) return;

            // Add sitemaps files passed in directly from context
            foreach (var file in targetSitemapsFiles)
            {
                var fileReplaced = string.Format(file, (object[])args);
                lines.Add(new ProjectFileLineInfo(fileReplaced, "sitemaps"));
            }
        }

        private void AddLinesFromTargetProjectFiles(IEnumerable<string> targetProjectFiles, List<IProjectFileLineInfo> lines, string[] args)
        {
            if (targetProjectFiles == null) return;

            foreach (var file in targetProjectFiles)
            {
                lines.AddRange(mProjectFileParser.ParseFile(file, args));
            }
        }

        private void AddUrlsFromProcessedLines(IEnumerable<IProjectFileLineInfo> lines, List<string> urls)
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
