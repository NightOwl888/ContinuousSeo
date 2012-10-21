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
    using System.Threading;
    using ContinuousSeo.Core;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Validators;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class UrlProcessor
    {
        protected IRunnerContext mContext;
        protected IValidatorWrapper mValidator;
        protected IValidatorReportWriterFactory mReportWriterFactory;


        #region Overridable Members

        public abstract ValidationResult Process(IEnumerable<string> urls, string outputPath);

        protected virtual string FixOutputPath(string outputPath)
        {
            return outputPath;
        }

        protected virtual ValidationResult WriteXmlReport(IEnumerable<string> urls, Stream outputXmlReport, string outputPath)
        {
            throw new NotImplementedException();
        }

        protected virtual ValidationResult ValidateUrls(IEnumerable<string> urls, IValidatorReportTextWriter writer, string outputPath)
        {
            throw new NotImplementedException();
        }

        protected abstract IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream);

        #endregion



        protected virtual void PauseIteration()
        {
            if (mValidator.IsDefaultValidatorUrl())
            {
                Thread.Sleep(1000);
            }
        }


        #region Result Helpers

        protected virtual void AddResultTotals(ValidationResult totals, IValidatorReportItem report)
        {
            totals.TotalErrors += report.Errors;
            totals.TotalWarnings += report.Warnings;
        }

        protected virtual IValidatorReportTimes ReportElapsedTime(TimeSpan elapsed)
        {
            var result = new ValidatorReportTimes();
            this.ReportElapsedTime(result, elapsed);
            return result;
        }

        protected virtual void ReportElapsedTime(IValidatorReportTimes report, TimeSpan elapsed)
        {
            DateTime utcNow = TimeProvider.Current.UtcNow;
            DateTime now = System.TimeZone.CurrentTimeZone.ToLocalTime(utcNow);
            report.LocalStartTime = now.Subtract(elapsed);
            report.LocalEndTime = now;
            report.UtcStartTime = utcNow.Subtract(elapsed);
            report.UtcEndTime = utcNow;
            report.ElapsedTime = string.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }

        protected virtual void ReportTotalElapsedTime(IValidatorReportTextWriter writer, TimeSpan elapsed)
        {
            var times = ReportElapsedTime(elapsed);
            writer.WriteElapsedTime(times);
        }

        #endregion

    }
}
