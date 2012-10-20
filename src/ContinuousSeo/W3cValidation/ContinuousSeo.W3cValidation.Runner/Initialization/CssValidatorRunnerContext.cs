// -----------------------------------------------------------------------
// <copyright file="CssValidatorRunnerContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Initialization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Core.Css;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CssValidatorRunnerContext : ICssValidatorRunnerContext
    {
        public CssValidatorRunnerContext(IAnnouncer announcer)
        {
            if (announcer == null)
                throw new ArgumentNullException("announcer");

            this.Announcer = announcer;
            this.TotalTimeStopwatch = StopwatchProvider.Current.NewStopwatch();
        }

        #region ICssValidatorRunnerContext Members

        public virtual IEnumerable<string> TargetSitemapsFiles { get; set; }
        public virtual IEnumerable<string> TargetUrls { get; set; }
        public virtual IEnumerable<string> TargetProjectFiles { get; set; }
        public virtual IEnumerable<string> UrlReplacementArgs { get; set; }
        public virtual string OutputPath { get; set; } // Directory to write output files
        public virtual string OutputFormat { get; set; }
        public virtual string ValidatorUrl { get; set; }
        public virtual bool DirectInputMode { get; set; }

        #endregion

        #region ICssValidatorSettings Members

        public string UserMedium { get; set; }
        public string CssProfile { get; set; }
        public string Language { get; set; }
        public string WarningLevel { get; set; }

        #endregion

        #region IRunnerContext Members

        public IAnnouncer Announcer { get; private set; }
        public Stopwatch TotalTimeStopwatch { get; private set; }

        #endregion
    }
}
