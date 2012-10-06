// -----------------------------------------------------------------------
// <copyright file="CssValidatorResourceCopier.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core.Css
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

            map.Add(@"ContinuousSEO.W3CValidation.Core.Resources.style.base.css", @"style\base.css");
            map.Add(@"ContinuousSEO.W3CValidation.Core.Resources.style.results.css", @"style\results.css");

            return map;
        }
    }
}
