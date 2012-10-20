// -----------------------------------------------------------------------
// <copyright file="ValidatorReportTimes.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.UrlProcessors
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidatorReportTimes : IValidatorReportTimes
    {
        public DateTime LocalStartTime { get; set; }
        public DateTime LocalEndTime { get; set; }
        public DateTime UtcStartTime { get; set; }
        public DateTime UtcEndTime { get; set; }
        public string ElapsedTime { get; set; }
    }
}
