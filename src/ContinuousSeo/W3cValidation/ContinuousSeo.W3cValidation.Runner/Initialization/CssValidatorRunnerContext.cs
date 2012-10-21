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
