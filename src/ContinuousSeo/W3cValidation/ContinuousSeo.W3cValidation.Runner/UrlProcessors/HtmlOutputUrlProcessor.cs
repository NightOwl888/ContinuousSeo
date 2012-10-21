#region Copyright
// -----------------------------------------------------------------------
//
// Copyright (c) 2012, Shad Storhaug <shad@shadstorhaug.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// -----------------------------------------------------------------------
#endregion

namespace ContinuousSeo.W3cValidation.Runner.UrlProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Xsl;
    using ContinuousSeo.W3cValidation.Runner.Validators;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class HtmlOutputUrlProcessor : UrlProcessor
    {
        protected IFileNameGenerator mFileNameGenerator; // HTML only
        protected ResourceCopier mResourceCopier; // HTML only
        protected IStreamFactory mStreamFactory;
        protected IHtmlIndexFileWriter mHtmlIndexFileWriter;


        public override ValidationResult Process(IEnumerable<string> urls, string outputPath)
        {
            outputPath = FixOutputPath(outputPath);

            using (Stream outputXmlReport = mStreamFactory.GetMemoryStream())
            {
                return WriteXmlReport(urls, outputXmlReport, outputPath);
            }
        }

        protected override string FixOutputPath(string outputPath)
        {
            if (outputPath == null) return string.Empty;

            // Remove any filename from the path
            if (!string.IsNullOrEmpty(outputPath))
            {
                outputPath = Path.GetDirectoryName(outputPath);
            }
            return outputPath;
        }

        protected override ValidationResult WriteXmlReport(IEnumerable<string> urls, Stream outputXmlReport, string outputPath)
        {
            var result = new ValidationResult();

            using (var writer = mReportWriterFactory.GetTextWriter(outputXmlReport, Encoding.UTF8))
            {
                writer.WriteStartDocument();

                result = ValidateUrls(urls, writer, outputPath);

                writer.WriteEndDocument();
                writer.Flush();

                // If Html output, process xml report using Xslt here
                // This must be done before the writer is closed, or the
                // stream will also be closed.
                outputXmlReport.Position = 0;
                mHtmlIndexFileWriter.CreateIndexFile(outputXmlReport, outputPath);
            }
            return result;
        }

        protected override ValidationResult ValidateUrls(IEnumerable<string> urls, IValidatorReportTextWriter writer, string outputPath)
        {
            var result = new ValidationResult();

            if (urls.Count() > 0)
            {
                // Copy the images and CSS files for the HTML documents
                mResourceCopier.CopyResources(outputPath);
            }

            // Process urls
            foreach (var url in urls)
            {
                string fileName = Path.Combine(outputPath, mFileNameGenerator.GenerateFileName(url, "html"));
                using (var outputStream = mStreamFactory.GetFileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    var report = ValidateUrl(url ,writer, outputStream); 
                    AddResultTotals(result, report);

                    // Write the filename to the report
                    report.FileName = Path.GetFileName(fileName);

                    writer.WriteUrlElement(report);
                }
            }
            return result;
        }

        protected override IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream)
        {
            PauseIteration();
            return mValidator.ValidateUrl(url, outputStream, OutputFormat.Html);
        }

    }
}
