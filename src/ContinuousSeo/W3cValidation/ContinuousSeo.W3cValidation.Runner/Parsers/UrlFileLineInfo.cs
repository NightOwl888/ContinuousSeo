// -----------------------------------------------------------------------
// <copyright file="HtmlUrlFileLineInfo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrlFileLineInfo : IUrlFileLineInfo
    {
        public UrlFileLineInfo()
        {
            this.Mode = "single";
        }

        public string Url { get; set; }
        public string Mode { get; set; }

    }
}
