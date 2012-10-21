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
    /// Advanced settings to be passed in to the CSS W3C validator API. These
    /// settings are documented at http://jigsaw.w3.org/css-validator/manual.html#requestformat.
    /// </summary>
    public class CssValidatorSettings : ICssValidatorSettings
    {
        //public CssValidatorSettings()
        //{
        //    CssProfile = "css2";
        //    Language = "en";
        //    WarningLevel = "2";
        //}

        public string UserMedium { get; set; }
        public string CssProfile { get; set; }
        public string Language { get; set; }
        public string WarningLevel { get; set; }
    }
}
