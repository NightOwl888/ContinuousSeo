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
    using System.Reflection;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.Core.IO;
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
        private readonly HtmlValidatorRunnerContext mRunnerContext;

        // parsing inputs
        private readonly IUrlAggregator mUrlAggregator;

        // output
        private readonly IFileNameGenerator mFileNameGenerator; // HTML only
        private readonly ResourceCopier mResourceCopier; // HTML only
        private readonly IValidatorReportWriterFactory mReportWriterFactory;
        private readonly IStreamFactory mStreamFactory;
        private readonly IXslTransformer mXslTransformer;

        #endregion

        #region Constructor

        public HtmlValidatorUrlProcessor(
            IValidatorWrapper validator, 
            HtmlValidatorRunnerContext runnerContext, 
            IUrlAggregator urlAggregator,
            IFileNameGenerator fileNameGenerator,
            ResourceCopier resourceCopier,
            IValidatorReportWriterFactory reportWriterFactory,
            IStreamFactory streamFactory,
            IXslTransformer xslTransformer)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (runnerContext == null)
                throw new ArgumentNullException("runnerContext");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (fileNameGenerator == null)
                throw new ArgumentNullException("fileNameGenerator");
            if (resourceCopier == null)
                throw new ArgumentNullException("resourceCopier");
            if (reportWriterFactory == null)
                throw new ArgumentNullException("reportWriterFactory");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");
            if (xslTransformer == null)
                throw new ArgumentNullException("xslTransformer");

            this.mValidator = validator;
            this.mRunnerContext = runnerContext;
            this.mUrlAggregator = urlAggregator;
            this.mFileNameGenerator = fileNameGenerator;
            this.mResourceCopier = resourceCopier;
            this.mReportWriterFactory = reportWriterFactory;
            this.mStreamFactory = streamFactory;
            this.mXslTransformer = xslTransformer;
        }

        #endregion

        #region IUrlProcessor Members

        public void ProcessUrls()
        {
            var urls = mUrlAggregator.AggregateUrls();

            string outputFormat = (string.IsNullOrEmpty(mRunnerContext.OutputFormat)) ? string.Empty : mRunnerContext.OutputFormat.ToLowerInvariant();
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

        #region Private Methods

        #region Html Output

        private void ValidateUrlsWithHtmlOutput(IEnumerable<string> urls)
        {
            IValidatorReportItem report;
            Stream outputStream;
            string outputPath = (mRunnerContext.OutputPath == null) ? string.Empty : mRunnerContext.OutputPath;
            bool areResourcesWritten = false;

            // Remove any filename from the path
            if (!string.IsNullOrEmpty(outputPath))
            {
                outputPath = Path.GetDirectoryName(outputPath);
            }

            using (Stream outputXmlReport = mStreamFactory.GetMemoryStream())
            {
                using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
                {
                    writer.WriteStartDocument();

                    // Process aggregated urls
                    foreach (var url in urls)
                    {
                        string fileName = Path.Combine(outputPath, mFileNameGenerator.GenerateFileName(url, "html"));
                        using (outputStream = mStreamFactory.GetFileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Html);
                        }

                        report.FileName = Path.GetFileName(fileName);
                        writer.WriteUrlElement(report);

                        if (!areResourcesWritten)
                        {
                            mResourceCopier.CopyResources(outputPath);
                            areResourcesWritten = true;
                        }

                        if (mValidator.IsDefaultValidatorUrl())
                        {
                            Thread.Sleep(1000);
                        }
                    }

                    writer.WriteEndDocument();
                    writer.Flush();

                    // If Html output, process xml report using Xslt here
                    outputXmlReport.Position = 0;

                    var xslFilePath = "ContinuousSeo.W3cValidation.Runner.HtmlValidatorIndex.xsl";
                    using (Stream xsl = Assembly.GetExecutingAssembly().GetManifestResourceStream(xslFilePath))
                    {
                        mXslTransformer.Transform(outputXmlReport, xsl, Path.Combine(outputPath, "index.html"));
                    }

                }
            }
        }

        //private void ValidateUrlsWithHtmlOutput(IEnumerable<string> urls)
        //{
        //    IValidatorReportItem report;
        //    Stream outputStream;
        //    string outputPath = (mRunnerContext.OutputPath == null) ? string.Empty : mRunnerContext.OutputPath;
        //    bool areResourcesWritten = false;

        //    // Remove any filename from the path
        //    if (!string.IsNullOrEmpty(outputPath))
        //    {
        //        outputPath = Path.GetDirectoryName(outputPath);
        //    }

        //    using (Stream outputXmlReport = mStreamFactory.GetMemoryStream())
        //    {
        //        var xslFilePath = "ContinuousSeo.W3cValidation.Runner.HtmlValidatorIndex.xsl";
        //        using (Stream xsl = Assembly.GetExecutingAssembly().GetManifestResourceStream(xslFilePath))
        //        {
        //            mXslTransformer.Transform(outputXmlReport, xsl, Path.Combine(outputPath, "index.html"));
        //        }

        //        using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
        //        {
        //            writer.WriteStartDocument();

        //            // Process aggregated urls
        //            foreach (var url in urls)
        //            {
        //                string fileName = Path.Combine(outputPath, mFileNameGenerator.GenerateFileName(url, "html"));
        //                using (outputStream = mStreamFactory.GetFileStream(fileName, FileMode.Create, FileAccess.Write))
        //                {
        //                    report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Html);
        //                }

        //                report.FileName = Path.GetFileName(fileName);
        //                writer.WriteUrlElement(report);

        //                if (!areResourcesWritten)
        //                {
        //                    mResourceCopier.CopyResources(outputPath);
        //                    areResourcesWritten = true;
        //                }

        //                if (mValidator.IsDefaultValidatorUrl())
        //                {
        //                    Thread.Sleep(1000);
        //                }
        //            }

        //            writer.WriteEndDocument();
        //            writer.Flush();

        //            // TODO: If Html output, process xml report using Xslt here
        //            //outputXmlReport.Position = 0;
        //            //using (var output = mStreamFactory.GetFileStream(Path.Combine(outputPath, "index.xml"), FileMode.Create, FileAccess.Write))
        //            //{
        //            //    outputXmlReport.CopyTo(output);
        //            //}

        //            //var xsl = new MemoryStream();
        //            //var xslFilePath = "ContinuousSeo.W3cValidation.Runner.HtmlValidatorIndex.xsl";
        //            //using (Stream xsl = Assembly.GetExecutingAssembly().GetManifestResourceStream(xslFilePath))
        //            //{
        //            //    mXslTransformer.Transform(outputXmlReport, xsl, Path.Combine(outputPath, "index.html"));
        //            //}

        //        }
        //    }
        //}

        #endregion

        #region Xml Output

        private void ValidateUrlsWithXmlOutput(IEnumerable<string> urls)
        {
            // Use entire output path (should be the complete path to xml file).
            string outputPath = (mRunnerContext.OutputPath == null) ? string.Empty : mRunnerContext.OutputPath;
            using (Stream outputXmlReport = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                //using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
                //{
                //    writer.WriteStartDocument();

                //    // Process aggregated urls
                //    foreach (var url in urls)
                //    {
                //        using (var outputStream = mStreamFactory.GetMemoryStream())
                //        {
                //            var report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Soap12);
                //            outputStream.Position = 0;
                //            writer.WriteUrlElement(report, outputStream);
                //        }

                //        if (mValidator.IsDefaultValidatorUrl())
                //        {
                //            Thread.Sleep(1000);
                //        }
                //    }

                //    writer.WriteEndDocument();
                //}

                WriteXmlReportWithXmlOutput(outputXmlReport, urls);
            }
        }

        private void WriteXmlReportWithXmlOutput(Stream outputXmlReport, IEnumerable<string> urls)
        {
            using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
            {
                writer.WriteStartDocument();

                //// Process aggregated urls
                //foreach (var url in urls)
                //{
                //    using (var outputStream = mStreamFactory.GetMemoryStream())
                //    {
                //        var report = mValidator.ValidateUrl(url, outputStream, OutputFormat.Soap12);
                //        outputStream.Position = 0;
                //        writer.WriteUrlElement(report, outputStream);
                //    }

                //    if (mValidator.IsDefaultValidatorUrl())
                //    {
                //        Thread.Sleep(1000);
                //    }
                //}

                ValidateUrlsWithXmlOutputInner(writer, urls);

                writer.WriteEndDocument();
            }
        }

        private void ValidateUrlsWithXmlOutputInner(IValidatorReportTextWriter writer, IEnumerable<string> urls)
        {
            // Process aggregated urls
            foreach (var url in urls)
            {
                ValidateUrlWithXmlOutputInner(writer, url);
            }
        }

        private void ValidateUrlWithXmlOutputInner(IValidatorReportTextWriter writer, string url)
        {
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

        #endregion

      


        #endregion

    }
}
