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

namespace ContinuousSeo.W3cValidation.Runner
{
    using System;
    using System.Collections.Generic;
    using ContinuousSeo.Core.Diagnostics;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Validators;
    using ContinuousSeo.W3cValidation.Runner.DI;
    using ContinuousSeo.W3cValidation.Runner.UrlAggregators;
    using ContinuousSeo.W3cValidation.Runner.UrlProcessors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CssValidatorRunner : IValidatorRunner
    {
        private readonly IValidatorRunnerContext mContext;
        protected IUrlAggregator mUrlAggregator;
        protected IUrlProcessorFactory mUrlProcessorFactory;

        public CssValidatorRunner(IValidatorRunnerContext context, IUrlAggregator urlAggregator, IUrlProcessorFactory urlProcessorFactory) 
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (urlAggregator == null)
                throw new ArgumentNullException("urlAggregator");
            if (urlProcessorFactory == null)
                throw new ArgumentNullException("urlProcessorFactory");

            this.mContext = context;
            this.mUrlAggregator = urlAggregator;
            this.mUrlProcessorFactory = urlProcessorFactory;
        }

        protected virtual void Intialize()
        {
            mContext.Announcer.Header("W3C CSS Validator");

            // Keep track of how long this takes
            mContext.TotalTimeStopwatch.Start();
        }

        #region IValidatorRunner Members

        public ValidationResult Execute()
        {
            Intialize();

            var urls = mUrlAggregator.AggregateUrls();
            var processor = mUrlProcessorFactory.GetUrlProcessor(mContext.OutputFormat);
            var result = processor.Process(urls, mContext.OutputPath);

            return result;
        }

        #endregion
    }
}
