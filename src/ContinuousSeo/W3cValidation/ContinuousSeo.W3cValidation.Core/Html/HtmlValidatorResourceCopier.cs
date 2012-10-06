// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorResourceCopier.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains methods to copy the embedded resources from within this assembly to a path relative to 
    /// a specified destination directory that are required by the HTML output method of the HtmlValidator.
    /// </summary>
    public class HtmlValidatorResourceCopier : ResourceCopier
    {
        protected override Dictionary<string, string> GetResourceMap()
        {
            var map = new Dictionary<string, string>();

            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.error.png", @"images\info_icons\error.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.info.png", @"images\info_icons\info.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.ok.png", @"images\info_icons\ok.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.info_icons.warning.png", @"images\info_icons\warning.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.opensource-55x48.png", @"images\opensource-55x48.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.images.w3c.png", @"images\w3c.png");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.style.base", @"style\base");
            map.Add(@"ContinuousSeo.W3cValidation.Core.Resources.style.results", @"style\results");

            return map;
        }
    }
}
