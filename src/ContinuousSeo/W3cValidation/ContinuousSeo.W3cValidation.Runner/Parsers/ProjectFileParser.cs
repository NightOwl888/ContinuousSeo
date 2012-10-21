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

namespace ContinuousSeo.W3cValidation.Runner.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using ContinuousSeo.Core.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProjectFileParser : IProjectFileParser
    {
        private readonly IStreamFactory mStreamFactory;

        public ProjectFileParser(IStreamFactory streamFactory)
        {
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");

            this.mStreamFactory = streamFactory;
        }

        #region IProjectFileParser Members

        /// <summary>
        /// Parses a line from a Url input file.
        /// </summary>
        /// <param name="line">A line from a Url input file.</param>
        /// <param name="urlReplacementArgs">Arguments to be replaced in the Url field using standard .NET string 
        /// formatting syntax. For example, the string {0} will be replaced by the first argument in this array, 
        /// the string {1} by the second, and so forth.</param>
        /// <returns>A <see cref="IProjectFileLineInfo"/> object.</returns>
        public IProjectFileLineInfo ParseLine(string line, string[] urlReplacementArgs)
        {
            if (string.IsNullOrEmpty(line))
                return null;

            // separate the fields in the line by tab character
            string[] fields = line.Split(Convert.ToChar(9));

            var result = new ProjectFileLineInfo();

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

        public IEnumerable<IProjectFileLineInfo> ParseFile(Stream file, string[] urlReplacementArgs)
        {
            List<IProjectFileLineInfo> result = new List<IProjectFileLineInfo>();
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

        public IEnumerable<IProjectFileLineInfo> ParseFile(string path, string[] urlReplacementArgs)
        {
            using (var file = mStreamFactory.GetFileStream(path, FileMode.Open, FileAccess.Read))
            {
                return ParseFile(file, urlReplacementArgs);
            }
        }


        #endregion

    }
}
