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
    using System.Diagnostics;
    using System.IO;
    using ContinuousSeo.Core;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Validators;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class TimingHtmlOutputUrlProcessor : HtmlOutputUrlProcessor
    {
        protected readonly Stopwatch mStopwatch = StopwatchProvider.Current.NewStopwatch();

        protected override ValidationResult ValidateUrls(IEnumerable<string> urls, IValidatorReportTextWriter writer, string outputPath)
        {
            var result = base.ValidateUrls(urls, writer, outputPath);

            // report total elapsed time
            mContext.TotalTimeStopwatch.Stop();
            ReportTotalElapsedTime(writer, mContext.TotalTimeStopwatch.Elapsed);

            return result;
        }

        protected override IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream)
        {
            // Start stopwatch
            mStopwatch.Reset();
            mStopwatch.Start();

            var result = base.ValidateUrl(url, writer, outputStream);

            // NOTE: this must occur here for the wrapper class to log the correct times
            mStopwatch.Stop();
            // report times to the report
            this.ReportElapsedTime(result, mStopwatch.Elapsed);

            return result;
        }
    }
}
