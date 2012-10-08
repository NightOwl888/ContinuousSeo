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
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UrlFileParser : IUrlFileParser
    {
        #region IUrlFileParser Members

        /// <summary>
        /// Parses a line from a Url input file.
        /// </summary>
        /// <param name="line">A line from a Url input file.</param>
        /// <param name="urlReplacementArgs">Arguments to be replaced in the Url field using standard .NET string 
        /// formatting syntax. For example, the string {0} will be replaced by the first argument in this array, 
        /// the string {1} by the second, and so forth.</param>
        /// <returns>A <see cref="IUrlFileLineInfo"/> object.</returns>
        public IUrlFileLineInfo ParseLine(string line, string[] urlReplacementArgs)
        {
            if (string.IsNullOrEmpty(line))
                return null;

            // separate the fields in the line by tab character
            string[] fields = line.Split(Convert.ToChar(9));

            var result = new UrlFileLineInfo();

            // Get Url
            if (fields.Count() > 0 && !string.IsNullOrEmpty(fields[0]))
            {
                result.Url = string.Format(fields[0], (object[])urlReplacementArgs);
            }
            else
            {
                return null;
            }

            // Get Mode
            if (fields.Count() > 1 && !string.IsNullOrEmpty(fields[1]))
            {
                result.Mode = fields[1].ToLowerInvariant();
            }

            return result;
        }

        public IEnumerable<IUrlFileLineInfo> ParseFile(Stream file, string[] urlReplacementArgs)
        {
            List<IUrlFileLineInfo> result = new List<IUrlFileLineInfo>();
            using (StreamReader reader = new StreamReader(file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result.Add(ParseLine(line, urlReplacementArgs));
                }
            }
            return result;
        }

        public IEnumerable<IUrlFileLineInfo> ParseFile(string path, string[] urlReplacementArgs)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return ParseFile(file, urlReplacementArgs);
            }
        }


        #endregion


    }
}
