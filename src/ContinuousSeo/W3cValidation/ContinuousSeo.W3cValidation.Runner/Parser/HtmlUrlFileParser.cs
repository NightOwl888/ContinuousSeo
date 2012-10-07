// -----------------------------------------------------------------------
// <copyright file="HtmlUrlFileParser.cs" company="">
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
    public class HtmlUrlFileParser : IUrlFileParser
    {
        #region IUrlFileParser Members

        public IUrlFileLineInfo ParseLine(string line, string domain)
        {
            if (string.IsNullOrEmpty(line))
                return null;

            // separate the fields in the line
            string[] fields = line.Split(Convert.ToChar(9));

            var lineInfo = new HtmlUrlFileLineInfo();
            string protocol = "http";

            // Get protocol if it exists
            if (fields.Count() > 1 && !string.IsNullOrEmpty(fields[1]))
            {
                protocol = fields[1];
            }
            
            string url = fields[0];

            // Fix up relative urls
            if (!url.StartsWith("http"))
            {
                if (!url.StartsWith("/"))
                    url = "/" + url;
                url = protocol + "://" + domain + url;
            }

            lineInfo.Url = url;

            // Get Mode
            if (fields.Count() > 2 && !string.IsNullOrEmpty(fields[2]))
            {
                lineInfo.Mode = fields[2];
            }

            return lineInfo;
        }

        #endregion
    }
}
