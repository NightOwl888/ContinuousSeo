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
