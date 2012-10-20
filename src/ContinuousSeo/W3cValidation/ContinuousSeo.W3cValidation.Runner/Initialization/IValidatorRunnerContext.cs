// -----------------------------------------------------------------------
// <copyright file="IHtmlValidatorRunnerContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Initialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorRunnerContext : IRunnerContext
    {
        IEnumerable<string> TargetSitemapsFiles { get; set; }
        IEnumerable<string> TargetUrls { get; set; }
        IEnumerable<string> TargetProjectFiles { get; set; }
        IEnumerable<string> UrlReplacementArgs { get; set; }
        string OutputPath { get; set; } // Directory to write output files
        string OutputFormat { get; set; } // Currently only accepts Html
        string ValidatorUrl { get; set; }
        bool DirectInputMode { get; set; }
    }
}
