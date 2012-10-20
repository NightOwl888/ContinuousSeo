// -----------------------------------------------------------------------
// <copyright file="ICssValidatorRunnerContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Initialization
{
    using System;
    using System.Collections.Generic;
    using ContinuousSeo.W3cValidation.Core.Css;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICssValidatorRunnerContext : ICssValidatorSettings, IValidatorRunnerContext
    {
        //IEnumerable<string> TargetUrls { get; set; }
        //IEnumerable<string> TargetProjectFiles { get; set; }
        //IEnumerable<string> UrlReplacementArgs { get; set; }
        //string OutputPath { get; set; } // Directory to write output files
        //string OutputFormat { get; set; }
        //string ValidatorUrl { get; set; }
        //bool DirectInputMode { get; set; }
    }
}
