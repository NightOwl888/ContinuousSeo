// -----------------------------------------------------------------------
// <copyright file="IValidatorReportTimes.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorReportTimes
    {
        DateTime LocalStartTime { get; set; }
        DateTime LocalEndTime { get; set; }
        DateTime UtcStartTime { get; set; }
        DateTime UtcEndTime { get; set; }
        string ElapsedTime { get; set; }
    }
}
