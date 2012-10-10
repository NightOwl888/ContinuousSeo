// -----------------------------------------------------------------------
// <copyright file="HtmlUrlProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Linq;
    using System.Collections.Generic;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Output;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorUrlProcessor : IUrlProcessor
    {
        #region Private Members

        private readonly HtmlValidator Validator;
        private readonly HtmlValidatorRunnerContext RunnerContext;
        private readonly IUrlFileParser UrlFileParser;
        private readonly IUrlAggregator UrlAggregator;
        private readonly IOutputPathProvider OutputPathProvider;

        #endregion

        #region Constructor

        public HtmlValidatorUrlProcessor(
            HtmlValidator validator, 
            HtmlValidatorRunnerContext runnerContext, 
            IUrlFileParser urlFileParser, 
            IUrlAggregator urlAggregator,
            IOutputPathProvider outputPathProvider)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (runnerContext == null)
                throw new ArgumentNullException("runnerContext");
            if (urlFileParser == null)
                throw new ArgumentNullException("urlFileParser");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (outputPathProvider == null)
                throw new ArgumentNullException("outputPathProvider");

            this.Validator = validator;
            this.RunnerContext = runnerContext;
            this.UrlFileParser = urlFileParser;
            this.UrlAggregator = urlAggregator;
            this.OutputPathProvider = outputPathProvider;
        }

        #endregion

        #region IUrlProcessor Members

        //public IEnumerable<IValidatorReportItem> ProcessUrls()
        //{
            

        //    string targetFile = this.RunnerContext.TargetUrlFiles;
        //    string[] args = (string[])this.RunnerContext.UrlReplacementArgs;

        //    // TODO: Check to ensure file exists

        //    var lines = UrlFileParser.ParseFile(targetFile, args);


        //    var urls = new List<string>();
        //    var result = new List<ValidatorReportItem>();
            
        //    // Add urls passed in directly from context
        //    foreach (var url in this.RunnerContext.TargetUrls)
        //    {
        //        urls.Add(string.Format(url, (object[])args));
        //    }

        //    // Process lines in file
        //    foreach (var line in lines)
        //    {
        //        urls.AddRange(UrlAggregator.ProcessUrl(line));
        //    }

        //    // Process urls in result
        //    foreach (var url in urls)
        //    {
        //        // MOVETO: ValidateUrl()
        //        // TODO: wrap in try/catch so any error can be reported in the output
        //        var report = new ValidatorReportItem();

        //        string fileNameExtension = "html";
        //        OutputFormat outputFormat = OutputFormat.Html;

        //        switch (RunnerContext.OutputFormat.ToLowerInvariant())
        //        {
        //            case null:
        //            case "":
        //            case "html":
        //                outputFormat = OutputFormat.Html;
        //                fileNameExtension = "html";
        //                break;
        //        }

        //        // TODO: Move the file generation to external class so it can be set to a non-random value.
        //        string outputFileName = Guid.NewGuid().ToString().ToLowerInvariant() + "." + fileNameExtension;
        //        string outputPath = Path.Combine(RunnerContext.OutputPath, outputFileName);

        //        string validatorUrl = RunnerContext.ValidatorUrl;

                
        //        // TODO: Validate Url format before calling them.

        //        var validatorResult = this.Validator.Validate(outputPath, outputFormat, url, InputFormat.Uri, RunnerContext, validatorUrl);

        //        // TODO: Add 1 second delay if validatorUrl is default.


        //        // TODO: Extract domain name from Url and add to report.
        //        // TODO: Add elapsed time to the report.
        //        report.FileName = outputFileName;
        //        report.Url = url;
        //        report.Errors = validatorResult.Errors;
        //        report.Warnings = validatorResult.Warnings;
        //        report.IsValid = validatorResult.IsValid;

                

        //        result.Add(report);
        //    }

        //    return result;
        //}

        //public IEnumerable<IValidatorReportItem> ProcessUrls()
        //{
        //    var result = new List<IValidatorReportItem>();
        //    var urls = new List<string>();
        //    var lines = new List<IUrlFileLineInfo>();
        //    string targetFile = RunnerContext.TargetUrlFiles;
        //    string[] args = RunnerContext.UrlReplacementArgs.ToArray();

        //    // Add urls passed in directly from context
        //    foreach (var url in this.RunnerContext.TargetUrls)
        //    {
        //        urls.Add(string.Format(url, (object[])args));
        //    }

        //    // Add sitemaps files passed in directly from context
        //    foreach (var file in this.RunnerContext.TargetSitemapsFiles)
        //    {
        //        var fileReplaced = string.Format(file, (object[])args);
        //        lines.Add(new UrlFileLineInfo(fileReplaced, "sitemaps"));
        //    }

        //    if (!string.IsNullOrEmpty(targetFile))
        //    {
        //        lines.AddRange(UrlFileParser.ParseFile(targetFile, args));
        //    }

        //    // Process lines in file/passed in
        //    foreach (var line in lines)
        //    {
        //        urls.AddRange(UrlAggregator.ProcessLine(line));
        //    }

        //    // Process aggregated urls
        //    foreach (var url in urls)
        //    {
        //        result.Add(ProcessUrl(url));

        //        string validatorUrl = RunnerContext.ValidatorUrl;

        //        // Add 1 second delay if validatorUrl is default (or empty, which will lead to default), 
        //        // per the W3C Validator Service documentation.
        //        if (string.IsNullOrEmpty(validatorUrl) || Validator.IsDefaultValidatorAddress(validatorUrl))
        //        {
        //            Thread.Sleep(1000);
        //        }
        //    }

        //    return result;
        //}

        public IEnumerable<IValidatorReportItem> ProcessUrls()
        {
            var urls = new List<string>();
            var lines = new List<IUrlFileLineInfo>();
            string[] args = RunnerContext.UrlReplacementArgs.ToArray();

            AddTargetUrls(RunnerContext.TargetUrls, urls, args);
            AddLinesFromTargetSitemapsFiles(RunnerContext.TargetSitemapsFiles, lines, args);
            AddLinesFromTargetUrlFiles(RunnerContext.TargetUrlFiles, lines, args);
            AddUrlsFromProcessedLines(lines, urls);

            // Process aggregated urls
            return this.ValidateUrls(urls);
        }

        #endregion

        #region ProcessUrl

        //public IValidatorReportItem ProcessUrl(string url)
        //{
        //    var result = new ValidatorReportItem();
        //    result.Url = url;

        //    if (!ValidateAndReportUrlFormat(result, url))
        //    {
        //        // Url is invalid, so we report and exit early
        //        return result;
        //    }

        //    // wrap in try/catch so any error can be reported in the output
        //    try
        //    {
        //        // NOTE: If output format is xml we will probably want to make the output directory
        //        // a temp directory where the files can be generated and then combined into 1 file in a later step.
        //        var outputPath = OutputPathProvider.GetOutputPath(url);
        //        var outputFormat = GetFileOutputFormat();
        //        var validatorUrl = RunnerContext.ValidatorUrl;

        //        var validatorResult = this.Validator.Validate(outputPath, outputFormat, url, InputFormat.Uri, RunnerContext, validatorUrl);

        //        // TODO: Add elapsed time to the report.
        //        result.FileName = Path.GetFileName(outputPath);
        //        result.Errors = validatorResult.Errors;
        //        result.Warnings = validatorResult.Warnings;
        //        result.IsValid = validatorResult.IsValid;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsValid = false;
        //        result.ExceptionMessage = ex.Message;
        //    }

        //    return result;
        //}

        public IValidatorReportItem ProcessUrl(string url)
        {
            if (!IsValidUrl(url))
            {
                return ReportFailure(url, string.Empty, "The url is not in a valid format.");
            }

            // wrap in try/catch so any error can be reported in the output
            try
            {
                // NOTE: If output format is xml we will probably want to make the output directory
                // a temp directory where the files can be generated and then combined into 1 file in a later step.
                var outputPath = OutputPathProvider.GetOutputPath(url);
                var outputFormat = GetFileOutputFormat();
                var validatorUrl = RunnerContext.ValidatorUrl;

                var validatorResult = this.Validator.Validate(outputPath, outputFormat, url, InputFormat.Uri, RunnerContext, validatorUrl);

                return ReportSuccess(
                    url, 
                    GetDomainName(url), 
                    Path.GetFileName(outputPath), 
                    validatorResult.Errors, 
                    validatorResult.Warnings, 
                    validatorResult.IsValid);
            }
            catch (Exception ex)
            {
                return ReportFailure(url, GetDomainName(url), ex.Message);
            }
        }

        #endregion

        #region Private Methods

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
                urls.AddRange(UrlAggregator.ProcessLine(line));
            }
        }

        private IEnumerable<IValidatorReportItem> ValidateUrls(IEnumerable<string> urls)
        {
            var result = new List<IValidatorReportItem>();

            // Process aggregated urls
            foreach (var url in urls)
            {
                result.Add(ProcessUrl(url));

                if (IsDefaultValidatorUrl())
                {
                    Thread.Sleep(1000);
                }
            }

            return result;
        }

        private bool IsDefaultValidatorUrl()
        {
            string validatorUrl = RunnerContext.ValidatorUrl;
            return (string.IsNullOrEmpty(validatorUrl) || Validator.IsDefaultValidatorAddress(validatorUrl));
        }



        private IValidatorReportItem ReportSuccess(string url, string domainName, string fileName, int errors, int warnings, bool isValid)
        {
            var result = new ValidatorReportItem();
            result.Url = url;
            result.DomainName = domainName;
            // TODO: Add elapsed time to the report.
            result.FileName = fileName;
            result.Errors = errors;
            result.Warnings = warnings;
            result.IsValid = isValid;
            return result;
        }

        private IValidatorReportItem ReportFailure(string url, string domainName, string errorMessage)
        {
            var result = new ValidatorReportItem();
            result.Url = url;
            result.DomainName = domainName;
            result.ExceptionMessage = errorMessage;
            result.IsValid = false;
            return result;
        }

        private bool IsValidUrl(string url)
        {
            Uri uri;
            return (Uri.TryCreate(url, UriKind.Absolute, out uri));
        }

        private string GetDomainName(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return uri.IsDefaultPort ? uri.DnsSafeHost : uri.DnsSafeHost + ":" + uri.Port;
            }
            return string.Empty;
        }

        //private bool ValidateAndReportUrlFormat(IValidatorReportItem report, string url)
        //{
        //    // Validate Url format
        //    Uri uri;
        //    if (Uri.TryCreate(url, UriKind.Absolute, out uri))
        //    {
        //        // Extract domain name and port from Url and add to report.
        //        report.DomainName = uri.IsDefaultPort ? uri.DnsSafeHost : uri.DnsSafeHost + ":" + uri.Port;
        //        return true;
        //    }
        //    else
        //    {
        //        // Url is invalid, so we report and return false
        //        report.IsValid = false;
        //        report.ExceptionMessage = "The url is not in a valid format.";
        //        return false;
        //    }
        //}

        private OutputFormat GetFileOutputFormat()
        {
            OutputFormat result;
            string format = (string.IsNullOrEmpty(RunnerContext.OutputFormat)) ? string.Empty : RunnerContext.OutputFormat.ToLowerInvariant();
            switch (format)
            {
                case "xml":
                    result = OutputFormat.Soap12;
                    break;
                default:
                    result = OutputFormat.Html;
                    break;
            }
            return result;
        }

        #endregion

    }
}
