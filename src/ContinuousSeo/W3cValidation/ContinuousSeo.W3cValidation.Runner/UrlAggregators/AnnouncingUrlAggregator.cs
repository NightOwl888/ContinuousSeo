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

namespace ContinuousSeo.W3cValidation.Runner.UrlAggregators
{
    using System;
    using System.Collections.Generic;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Parsers;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnnouncingUrlAggregator : TimingUrlAggregator
    {
        public AnnouncingUrlAggregator(IValidatorRunnerContext context, IProjectFileParser projectFileParser, ISitemapsParser sitemapsParser)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (projectFileParser == null)
                throw new ArgumentNullException("projectFileParser");
            if (sitemapsParser == null)
                throw new ArgumentNullException("sitemapsParser");

            this.mContext = context;
            this.mProjectFileParser = projectFileParser;
            this.mSitemapsParser = sitemapsParser;
        }

        public override IEnumerable<string> AggregateUrls()
        {
            var processName = "aggregating target URLs";
            OutputStartProcess(processName);

            var result = base.AggregateUrls();

            OutputEndProcess(processName);

            return result;
        }

        private void OutputStartProcess(string processName)
        {
            mContext.Announcer.Say(string.Format("Starting {0}...", processName));
        }

        private void OutputEndProcess(string processName)
        {
            mContext.Announcer.Say(string.Format("Completed {0}.", processName));
            mContext.Announcer.ElapsedTime(mStopwatch.Elapsed);
        }
    }
}
