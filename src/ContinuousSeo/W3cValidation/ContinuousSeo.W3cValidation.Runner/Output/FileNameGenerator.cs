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

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.IO;
    using System.Text;
    using ContinuousSeo.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileNameGenerator : IFileNameGenerator
    {
        #region IFileNameGenerator Members

        public string GenerateFileName(string url, string extension)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentNullException("extension");

            const int fileNameLengthLimit = 250;
            int limit = ((fileNameLengthLimit - extension.Length) - 6); // 1 char for the . and 5 for a random number

            int max = url.Length > limit ? limit : url.Length;
            char[] chars = url.ToCharArray(0, max);
            var builder = new StringBuilder();
            foreach (char c in chars)
            {
                if (IsValidFileNameCharacter(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append('_');
                }
            }

            if (builder.Length == limit)
            {
                // we have reached the maximum, so we need to add some random numbers to the end to
                // help ensure uniqueness.
                var r = new Random();
                for (int i = 0; i < 5; i++)
                {
                    builder.Append(r.Next(0, 9));
                }
            }

            // Now append the extension.
            builder.Append('.');
            builder.Append(extension);

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        private bool IsValidFileNameCharacter(char input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (c.Equals(input))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

    }
}
