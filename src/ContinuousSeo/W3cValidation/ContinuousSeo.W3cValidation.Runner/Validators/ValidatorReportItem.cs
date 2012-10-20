// -----------------------------------------------------------------------
// <copyright file="ValidationReport.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Validators
{
    using System;
    using ContinuousSeo.W3cValidation.Runner.UrlProcessors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidatorReportItem : IValidatorReportItem, IValidatorReportTimes
    {
        #region IValidatorReportItem Members

        public string DomainName { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public bool IsValid { get; set; }
        public int Errors { get; set; }
        public int Warnings { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime LocalStartTime { get; set; }
        public DateTime LocalEndTime { get; set; }
        public DateTime UtcStartTime { get; set; }
        public DateTime UtcEndTime { get; set; }
        public string ElapsedTime { get; set; }

        #endregion
    }
}
