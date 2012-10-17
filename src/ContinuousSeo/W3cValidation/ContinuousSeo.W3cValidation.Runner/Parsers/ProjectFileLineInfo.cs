// -----------------------------------------------------------------------
// <copyright file="ProjectFileLineInfo.cs" company="">
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
    public class ProjectFileLineInfo : IProjectFileLineInfo
    {
        public ProjectFileLineInfo()
        {
            this.Mode = "single";
        }

        public ProjectFileLineInfo(string url, string mode)
        {
            this.Url = url;
            this.Mode = mode;
        }

        public string Url { get; set; }
        public string Mode { get; set; }

    }
}
