// -----------------------------------------------------------------------
// <copyright file="HtmlUrlFileLineInfo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlUrlFileLineInfo : IUrlFileLineInfo
    {
        public HtmlUrlFileLineInfo()
        {
            this.Mode = "single";
        }

        #region IUrlFileLineInfo Members

        public string Url { get; set; }
        public string Mode { get; set; }

        #endregion
    }
}
