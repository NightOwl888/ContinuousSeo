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

namespace ContinuousSeo.W3cValidation.Core.Css
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents detailed report information from the W3C CSS Validator.
    /// </summary>
    public class CssValidatorReport : IValidatorReport
    {
        public CssValidatorReport(string url, string checkedBy, string doctype, string charset, bool validity, IEnumerable<IValidatorMessage> errors, IEnumerable<IValidatorMessage> warnings)
        {
            this.Url = url;
            this.CheckedBy = checkedBy;
            this.Doctype = doctype;
            this.Charset = charset;
            this.Validity = validity;
            this.Errors = errors;
            this.Warnings = warnings;
        }

        public string Url { get; private set; }
        public string CheckedBy { get; private set; }
        public string Doctype { get; private set; }
        public string Charset { get; private set; }
        public bool Validity { get; private set; }
        public IEnumerable<IValidatorMessage> Errors { get; private set; }
        public IEnumerable<IValidatorMessage> Warnings { get; private set; }
    }
}
