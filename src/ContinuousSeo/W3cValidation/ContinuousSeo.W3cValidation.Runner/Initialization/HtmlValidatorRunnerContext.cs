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
    using ContinuousSeo.W3cValidation.Core.Html;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRunnerContext : IHtmlValidatorRunnerContext
    {
        public HtmlValidatorRunnerContext(IAnnouncer announcer)
        {
            if (announcer == null)
                throw new ArgumentNullException("announcer");

            this.Announcer = announcer;
            this.TotalTimeStopwatch = StopwatchProvider.Current.NewStopwatch();
        }

        public virtual IEnumerable<string> TargetSitemapsFiles { get; set; }
        public virtual IEnumerable<string> TargetUrls { get; set; }
        public virtual IEnumerable<string> TargetProjectFiles { get; set; }
        public virtual IEnumerable<string> UrlReplacementArgs { get; set; }
        public virtual string OutputPath { get; set; } // Directory to write output files
        public virtual string OutputFormat { get; set; }
        public virtual string ValidatorUrl { get; set; }
        public virtual bool DirectInputMode { get; set; }


        #region IHtmlValidatorSettings Members

        public virtual string CharSet { get; set; }
        public virtual string DocType { get; set; }
        public virtual bool Verbose { get; set; }
        public virtual bool Debug { get; set; }
        public virtual bool ShowSource { get; set; }
        public virtual bool Outline { get; set; }
        public virtual bool GroupErrors { get; set; }
        public virtual bool UseHtmlTidy { get; set; }

        #endregion

        public IAnnouncer Announcer { get; private set; }
        public Stopwatch TotalTimeStopwatch { get; private set; }

    }
}
