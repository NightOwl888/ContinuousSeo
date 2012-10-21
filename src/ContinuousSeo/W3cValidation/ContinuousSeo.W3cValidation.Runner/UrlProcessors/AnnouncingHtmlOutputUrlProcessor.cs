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
    using System.IO;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Xsl;
    using ContinuousSeo.W3cValidation.Runner.Validators;
    using ContinuousSeo.W3cValidation.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnnouncingHtmlOutputUrlProcessor : TimingHtmlOutputUrlProcessor
    {

        public AnnouncingHtmlOutputUrlProcessor(
            IValidatorWrapper validator,
            IRunnerContext context,
            IFileNameGenerator fileNameGenerator,
            ResourceCopier resourceCopier,
            IValidatorReportWriterFactory reportWriterFactory,
            IStreamFactory streamFactory,
            IHtmlIndexFileWriter htmlIndexFileWriter)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (context == null)
                throw new ArgumentNullException("context");
            if (fileNameGenerator == null)
                throw new ArgumentNullException("fileNameGenerator");
            if (resourceCopier == null)
                throw new ArgumentNullException("resourceCopier");
            if (reportWriterFactory == null)
                throw new ArgumentNullException("reportWriterFactory");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");
            if (htmlIndexFileWriter == null)
                throw new ArgumentNullException("htmlIndexFileWriter");

            this.mValidator = validator;
            this.mContext = context;
            this.mFileNameGenerator = fileNameGenerator;
            this.mResourceCopier = resourceCopier;
            this.mReportWriterFactory = reportWriterFactory;
            this.mStreamFactory = streamFactory;
            this.mHtmlIndexFileWriter = htmlIndexFileWriter;
        }

        public override ValidationResult Process(IEnumerable<string> urls, string outputPath)
        {
            mContext.Announcer.Heading("Beginning processing with HTML output");

            var result = base.Process(urls, outputPath);

            mContext.Announcer.Heading("Completed processing with HTML output");
            mContext.Announcer.Say("Total Elapsed Time:");
            mContext.Announcer.ElapsedTime(mContext.TotalTimeStopwatch.Elapsed);
            mContext.Announcer.Heading(string.Format("Validation completed with {0} error(s) and {1} warning(s).", result.TotalErrors, result.TotalWarnings));

            return result;
        }

        protected override IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream)
        {
            // Report process start
            string processName = string.Format("validation for '{0}'", url);
            this.OutputStartProcess(processName);

            var report = base.ValidateUrl(url, writer, outputStream);

            // Report process completed
            this.OutputEndProcess(processName);

            return report;
        }

        private void OutputStartProcess(string processName)
        {
            mContext.Announcer.Say(string.Format("Starting {0}...", processName));
        }

        private void OutputEndProcess(string processName)
        {
            mContext.Announcer.Say(string.Format("Completed {0}.", processName));
            mContext.Announcer.ElapsedTime(mStopwatch.Elapsed);
        }
    }
}
