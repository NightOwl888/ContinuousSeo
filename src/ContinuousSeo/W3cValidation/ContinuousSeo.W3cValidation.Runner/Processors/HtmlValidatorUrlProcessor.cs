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
    using System.Text;
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

        // validation
        private readonly IValidatorWrapper mValidator;
        private readonly HtmlValidatorRunnerContext RunnerContext;

        // parsing inputs
        private readonly IUrlAggregator UrlAggregator;

        // output
        //private readonly IOutputPathProvider OutputPathProvider;
        private readonly IFileNameGenerator mFileNameGenerator; // HTML only
        private readonly ResourceCopier ResourceCopier; // HTML only
        private readonly IValidatorReportWriterFactory ReportWriterFactory;
        private readonly IStreamFactory mStreamFactory;

        #endregion

        #region Constructor

        public HtmlValidatorUrlProcessor(
            IValidatorWrapper validator, 
            HtmlValidatorRunnerContext runnerContext, 
            IUrlAggregator urlAggregator,
            //IOutputPathProvider outputPathProvider,
            IFileNameGenerator fileNameGenerator,
            ResourceCopier resourceCopier,
            IValidatorReportWriterFactory reportWriterFactory,
            IStreamFactory streamFactory)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (runnerContext == null)
                throw new ArgumentNullException("runnerContext");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            //if (outputPathProvider == null)
            //    throw new ArgumentNullException("outputPathProvider");
            if (fileNameGenerator == null)
                throw new ArgumentNullException("fileNameGenerator");
            if (resourceCopier == null)
                throw new ArgumentNullException("resourceCopier");
            if (reportWriterFactory == null)
                throw new ArgumentNullException("reportWriterFactory");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");

            this.mValidator = validator;
            this.RunnerContext = runnerContext;
            this.UrlAggregator = urlAggregator;
            //this.OutputPathProvider = outputPathProvider;
            this.mFileNameGenerator = fileNameGenerator;
            this.ResourceCopier = resourceCopier;
            this.ReportWriterFactory = reportWriterFactory;
            this.mStreamFactory = streamFactory;
        }

        #endregion

        #region IUrlProcessor Members

        public void ProcessUrls()
        {
            //var urls = new List<string>();
            //var lines = new List<IUrlFileLineInfo>();
            //string[] args = RunnerContext.UrlReplacementArgs.ToArray();

            ////AddTargetUrls(RunnerContext.TargetUrls, urls, args);
            ////AddLinesFromTargetSitemapsFiles(RunnerContext.TargetSitemapsFiles, lines, args);
            ////AddLinesFromTargetUrlFiles(RunnerContext.TargetUrlFiles, lines, args);
            ////AddUrlsFromProcessedLines(lines, urls);

            //// Process aggregated urls
            //ValidateUrls(urls);

            var urls = UrlAggregator.AggregateUrls(RunnerContext);

            //ValidateUrls(urls);

            string outputFormat = (string.IsNullOrEmpty(RunnerContext.OutputFormat)) ? string.Empty : RunnerContext.OutputFormat.ToLowerInvariant();
            switch (outputFormat)
            {
                case "xml":
                    ValidateUrlsWithXmlOutput(urls);
                    break;
                default:
                    ValidateUrlsWithHtmlOutput(urls);
                    break;
            }  
        }

        #endregion

        #region ProcessUrl

        //public IValidatorReportItem ProcessUrl(string url, Stream output, OutputFormat outputFormat)
        //{
        //    if (!IsValidUrl(url))
        //    {
        //        return ReportFailure(url, string.Empty, "The url is not in a valid format.");
        //    }

        //    // wrap in try/catch so any error can be reported in the output
        //    try
        //    {
        //        var validatorUrl = RunnerContext.ValidatorUrl;

                
        //        // NOTE: If output format is xml we will probably want to make the output directory
        //        // a temp directory where the files can be generated and then combined into 1 file in a later step.

        //        //var outputPath = OutputPathProvider.GetOutputPath(url);
        //        //var validatorResult = this.Validator.Validate(outputPath, outputFormat, url, InputFormat.Uri, RunnerContext, validatorUrl);

        //        var validatorResult = this.Validator.Validate(output, outputFormat, url, InputFormat.Uri, RunnerContext, validatorUrl);

        //        return ReportSuccess(
        //            url, 
        //            GetDomainName(url), 
        //            validatorResult.Errors, 
        //            validatorResult.Warnings, 
        //            validatorResult.IsValid);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ReportFailure(url, GetDomainName(url), ex.Message);
        //    }
        //}

        #endregion

        #region Private Methods

        

        //private IEnumerable<IValidatorReportItem> ValidateUrls(IEnumerable<string> urls, Stream outputXmlReport)
        //{
        //    var result = new List<IValidatorReportItem>();
        //    var outputFormat = GetFileOutputFormat();

        //    // Process aggregated urls
        //    foreach (var url in urls)
        //    {


        //        result.Add(ProcessUrl(url, outputXmlReport, outputFormat));

        //        if (IsDefaultValidatorUrl())
        //        {
        //            Thread.Sleep(1000);
        //        }
        //    }

        //    return result;
        //}

        //private void ValidateUrls(IEnumerable<string> urls)
        //{
        //    var outputFormat = GetFileOutputFormat();
        //    Stream outputStream;
        //    string outputPath = string.Empty;
        //    IValidatorReportItem report;
        //    bool areResourcesWritten = false;

        //    Stream outputXmlReport; // TODO: setup stream

        //    switch (outputFormat)
        //    {
        //        case OutputFormat.Soap12:
        //             outputPath = OutputPathProvider.GetOutputPath();
        //             outputXmlReport = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        //            break;
        //        default:
        //            outputXmlReport = new MemoryStream();
        //            break;

        //    }

        //    // TODO: Bring outputXmlReport stream into this method and use it to generate
        //    // the xml report or pass the report to another method that will turn it to html

        //    using (var writer = ReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
        //    {
        //        writer.WriteStartDocument();

        //        // Process aggregated urls
        //        foreach (var url in urls)
        //        {
        //            switch (outputFormat)
        //            {
        //                case OutputFormat.Soap12: // Xml


        //                    using (outputStream = new MemoryStream())
        //                    {
        //                        report = ProcessUrl(url, outputStream, outputFormat);

        //                        outputStream.Position = 0;

        //                        writer.WriteUrlElement(report, outputStream);
        //                    }
        //                    break;

        //                default: // HTML
        //                    outputPath = OutputPathProvider.GetOutputPath(url);
        //                    using (outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //                    {
        //                        report = ProcessUrl(url, outputStream, outputFormat);
        //                    }

        //                    report.FileName = Path.GetFileName(outputPath);
        //                    writer.WriteUrlElement(report);

        //                    if (!areResourcesWritten)
        //                    {
        //                        ResourceCopier.CopyResources(Path.GetDirectoryName(outputPath));
        //                        areResourcesWritten = true;
        //                    }
        //                    break;

        //            }


        //            //ProcessUrl(url, outputStream, outputFormat);

        //            if (IsDefaultValidatorUrl())
        //            {
        //                Thread.Sleep(1000);
        //            }
        //        }

        //        writer.WriteEndDocument();

                
        //    }

        //    // TODO: If Html output, process xml report using Xslt here

        //    //return result;
        //}


        //private void ValidateUrls(IEnumerable<string> urls)
        //{
        //    string outputFormat = (string.IsNullOrEmpty(RunnerContext.OutputFormat)) ? string.Empty : RunnerContext.OutputFormat.ToLowerInvariant();
        //    switch (outputFormat)
        //    {
        //        case "xml":
        //            ValidateUrlsWithXmlOutput(urls);
        //            break;
        //        default:
        //            ValidateUrlsWithHtmlOutput(urls);
        //            break;
        //    }    
        //}

        private void ValidateUrlsWithHtmlOutput(IEnumerable<string> urls)
        {
            IValidatorReportItem report;
            Stream outputStream;
            string outputPath = (RunnerContext.OutputPath == null) ? string.Empty : RunnerContext.OutputPath;
            string outputDirectory = string.Empty;
            bool areResourcesWritten = false;

            if (!string.IsNullOrEmpty(outputPath))
            {
                outputDirectory = Path.GetDirectoryName(outputPath);
            }

            using (Stream outputXmlReport = mStreamFactory.GetMemoryStream())
            {
                using (var writer = ReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
                {
                    writer.WriteStartDocument();

                    // Process aggregated urls
                    foreach (var url in urls)
                    {
                        //outputPath = OutputPathProvider.GetOutputPath(url);

                        string fileName = Path.Combine(outputDirectory, mFileNameGenerator.GenerateFileName(url, "html"));
                        //using (outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        using (outputStream = mStreamFactory.GetFileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Html);
                        }

                        report.FileName = Path.GetFileName(fileName);
                        writer.WriteUrlElement(report);

                        if (!areResourcesWritten)
                        {
                            ResourceCopier.CopyResources(Path.GetDirectoryName(outputPath));
                            areResourcesWritten = true;
                        }

                        if (mValidator.IsDefaultValidatorUrl())
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    writer.WriteEndDocument();

                }

                // If Html output, process xml report using Xslt here
                outputXmlReport.Position = 0;
                
                //using (var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                using (var output = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    outputXmlReport.CopyTo(output);
                }
            }
        }

        private void ValidateUrlsWithXmlOutput(IEnumerable<string> urls)
        {
            // Use entire output path (should be the complete path to xml file).
            string outputPath = (RunnerContext.OutputPath == null) ? string.Empty : RunnerContext.OutputPath;
            //using (Stream outputXmlReport = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            using (Stream outputXmlReport = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                using (var writer = ReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
                {
                    writer.WriteStartDocument();

                    // Process aggregated urls
                    foreach (var url in urls)
                    {
                        //using (var outputStream = new MemoryStream())
                        using (var outputStream = mStreamFactory.GetMemoryStream())
                        {
                            var report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Soap12);
                            outputStream.Position = 0;
                            writer.WriteUrlElement(report, outputStream);
                        }

                        if (mValidator.IsDefaultValidatorUrl())
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    writer.WriteEndDocument();
                }
            }
        }







        //private string GetOutputFileNameExtension()
        //{
        //    string result;
        //    string outputFormat = (String.IsNullOrEmpty(RunnerContext.OutputFormat)) ? String.Empty : RunnerContext.OutputFormat.ToLowerInvariant();
        //    switch (outputFormat)
        //    {
        //        case "xml":
        //            result = "xml";
        //            break;
        //        default:
        //            result = "html";
        //            break;
        //    }
        //    return result;
        //}







        


        //private bool IsDefaultValidatorUrl()
        //{
        //    string validatorUrl = RunnerContext.ValidatorUrl;
        //    return (string.IsNullOrEmpty(validatorUrl) || Validator.IsDefaultValidatorAddress(validatorUrl));
        //}

        //private IValidatorReportItem ReportSuccess(string url, string domainName, int errors, int warnings, bool isValid)
        //{
        //    var result = new ValidatorReportItem();
        //    result.Url = url;
        //    result.DomainName = domainName;
        //    // TODO: Add elapsed time to the report.
        //    //result.FileName = fileName;
        //    result.Errors = errors;
        //    result.Warnings = warnings;
        //    result.IsValid = isValid;
        //    return result;
        //}

        //private IValidatorReportItem ReportFailure(string url, string domainName, string errorMessage)
        //{
        //    var result = new ValidatorReportItem();
        //    result.Url = url;
        //    result.DomainName = domainName;
        //    result.ErrorMessage = errorMessage;
        //    result.IsValid = false;
        //    return result;
        //}

        //private bool IsValidUrl(string url)
        //{
        //    Uri uri;
        //    return (Uri.TryCreate(url, UriKind.Absolute, out uri));
        //}

        //private string GetDomainName(string url)
        //{
        //    Uri uri;
        //    if (Uri.TryCreate(url, UriKind.Absolute, out uri))
        //    {
        //        return uri.IsDefaultPort ? uri.DnsSafeHost : uri.DnsSafeHost + ":" + uri.Port;
        //    }
        //    return string.Empty;
        //}

        //private OutputFormat GetFileOutputFormat()
        //{
        //    OutputFormat result;
        //    string format = (string.IsNullOrEmpty(RunnerContext.OutputFormat)) ? string.Empty : RunnerContext.OutputFormat.ToLowerInvariant();
        //    switch (format)
        //    {
        //        case "xml":
        //            result = OutputFormat.Soap12;
        //            break;
        //        default:
        //            result = OutputFormat.Html;
        //            break;
        //    }
        //    return result;
        //}

        #endregion

    }
}
