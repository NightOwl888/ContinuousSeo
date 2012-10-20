// -----------------------------------------------------------------------
// <copyright file="UrlProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
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
