// -----------------------------------------------------------------------
// <copyright file="IHtmlValidationRunnerContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Initialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Core.Html;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHtmlValidatorRunnerContext : IHtmlValidatorSettings
    {
        IEnumerable<string> TargetSitemapsFiles { get; set; }
        IEnumerable<string> TargetUrls { get; set; }
        IEnumerable<string> TargetProjectFiles { get; set; }
        IEnumerable<string> UrlReplacementArgs { get; set; }
        string OutputPath { get; set; } // Directory to write output files
        string OutputFormat { get; set; }
        string ValidatorUrl { get; set; }
        bool DirectInputMode { get; set; }

        IAnnouncer Announcer { get; }
        Stopwatch TotalTimeStopwatch { get; }

    }
}
