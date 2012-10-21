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
    using System.IO;
    using System.Collections.Generic;

    /// <summary>
    /// Contains methods to copy the embedded resources from within this assembly to a path relative to 
    /// a specified destination directory that are required by the HTML output method of the CSSValidator.
    /// </summary>
    public class CssValidatorResourceCopier : ResourceCopier
    {

        protected override Dictionary<string, string> GetResourceMap()
        {
            var map = new Dictionary<string, string>();

            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.error.png", @"images\info_icons\error.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.info.png", @"images\info_icons\info.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.ok.png", @"images\info_icons\ok.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.warning.png", @"images\info_icons\warning.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.w3c.png", @"images\w3c.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.style.base.css", @"style\base.css");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.style.results.css", @"style\results.css");

            return map;
        }
    }
}
