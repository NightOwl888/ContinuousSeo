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

namespace ContinuousSeo.W3cValidation.Core.Html
{

    /// <summary>
    /// The resultant header information from the W3C HTML Validation API.
    /// </summary>
    public class HtmlValidatorResult
    {
        public HtmlValidatorResult(string status, int errors, int warnings, int recursion)
        {
            this.Status = status;
            this.Errors = errors;
            this.Warnings = warnings;
            this.Recursion = recursion;
        }

        public string Status { get; private set; }
        public bool IsValid { get { return Status == "Valid"; } }
        public int Errors { get; private set; }
        public int Warnings { get; private set; }
        public int Recursion { get; private set; }
    }
}
