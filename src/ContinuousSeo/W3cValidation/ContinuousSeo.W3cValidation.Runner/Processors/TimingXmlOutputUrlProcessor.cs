// -----------------------------------------------------------------------
// <copyright file="TimingXmlOutputUrlProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using ContinuousSeo.Core;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class TimingXmlOutputUrlProcessor : XmlOutputUrlProcessor
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
            mStopwatch.Reset();
            mStopwatch.Start();

            var result = base.ValidateUrl(url, writer, outputStream);

            // NOTE: this must occur here for the wrapper class to log the correct times
            mStopwatch.Stop();
            // report times to the report
            this.ReportElapsedTime(result, mStopwatch.Elapsed);

            return result;
        }



        protected IValidatorReportTimes GetReportedElapsedTime(TimeSpan elapsed)
        {
            var result = new ValidatorReportTimes();
            this.ReportElapsedTime(result, elapsed);
            return result;
        }

        protected void ReportElapsedTime(IValidatorReportTimes report, TimeSpan elapsed)
        {
            DateTime utcNow = TimeProvider.Current.UtcNow;
            DateTime now = System.TimeZone.CurrentTimeZone.ToLocalTime(utcNow);
            report.LocalStartTime = now.Subtract(elapsed);
            report.LocalEndTime = now;
            report.UtcStartTime = utcNow.Subtract(elapsed);
            report.UtcEndTime = utcNow;
            report.ElapsedTime = string.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }

        protected void ReportTotalElapsedTime(IValidatorReportTextWriter writer, TimeSpan elapsed)
        {
            var times = GetReportedElapsedTime(elapsed);
            writer.WriteElapsedTime(times);
        }

    }
}



