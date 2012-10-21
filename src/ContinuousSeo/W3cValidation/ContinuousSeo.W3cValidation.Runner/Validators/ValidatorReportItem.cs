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

namespace ContinuousSeo.W3cValidation.Runner.Validators
{
    using System;
    using ContinuousSeo.W3cValidation.Runner.UrlProcessors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidatorReportItem : IValidatorReportItem, IValidatorReportTimes
    {
        #region IValidatorReportItem Members

        public string DomainName { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public bool IsValid { get; set; }
        public int Errors { get; set; }
        public int Warnings { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime LocalStartTime { get; set; }
        public DateTime LocalEndTime { get; set; }
        public DateTime UtcStartTime { get; set; }
        public DateTime UtcEndTime { get; set; }
        public string ElapsedTime { get; set; }

        #endregion
    }
}
