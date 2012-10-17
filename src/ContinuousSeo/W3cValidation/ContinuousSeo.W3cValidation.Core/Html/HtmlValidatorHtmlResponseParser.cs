// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorHtmlResponseParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorHtmlResponseParser
    {
        public IValidatorReport ParseResponse(Stream response)
        {
            string pageText = string.Empty;
            int errors = -1;
            int warnings = -1;

            response.Position = 0;
            using (var reader = new StreamReader(response))
            {
                pageText = reader.ReadToEnd();
            }

            string pattern = @">\s*(?'errors'\d+) [Ee]rrors(?:, (?'warnings'\d+) [Ww]arning\(s\))*\s*<";

            //var re = new Regex(pattern, RegexOptions.Compiled);

            var match = Regex.Match(pageText,pattern, RegexOptions.Compiled);

            if (match.Success)
            {
                if (match.Groups["errors"] != null) 
                {
                    int.TryParse(match.Groups["errors"].Value, out errors);
                }
                if (match.Groups["warnings"] != null)
                {
                    int.TryParse(match.Groups["warnings"].Value, out warnings);
                }
            }

            return null;
        }
    }
}
