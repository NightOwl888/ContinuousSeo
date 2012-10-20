// -----------------------------------------------------------------------
// <copyright file="mUrlAggregator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.UrlAggregators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class UrlAggregator : IUrlAggregator
    {
        protected IValidatorRunnerContext mContext;
        protected ISitemapsParser mSitemapsParser;
        protected IProjectFileParser mProjectFileParser;

        #region IUrlAggregator Members

        public virtual IEnumerable<string> AggregateUrls()
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

        protected IEnumerable<string> ProcessLine(IProjectFileLineInfo urlInfo)
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

        protected void AddTargetUrls(IEnumerable<string> targetUrls, List<string> urls, string[] args)
        {
            if (targetUrls == null) return;

            // Add urls passed in directly from context
            foreach (var url in targetUrls)
            {
                urls.Add(string.Format(url, (object[])args));
            }
        }

        protected void AddLinesFromTargetSitemapsFiles(IEnumerable<string> targetSitemapsFiles, List<IProjectFileLineInfo> lines, string[] args)
        {
            if (targetSitemapsFiles == null) return;

            // Add sitemaps files passed in directly from context
            foreach (var file in targetSitemapsFiles)
            {
                var fileReplaced = string.Format(file, (object[])args);
                lines.Add(new ProjectFileLineInfo(fileReplaced, "sitemaps"));
            }
        }

        protected void AddLinesFromTargetProjectFiles(IEnumerable<string> targetProjectFiles, List<IProjectFileLineInfo> lines, string[] args)
        {
            if (targetProjectFiles == null) return;

            foreach (var file in targetProjectFiles)
            {
                lines.AddRange(mProjectFileParser.ParseFile(file, args));
            }
        }

        protected void AddUrlsFromProcessedLines(IEnumerable<IProjectFileLineInfo> lines, List<string> urls)
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
