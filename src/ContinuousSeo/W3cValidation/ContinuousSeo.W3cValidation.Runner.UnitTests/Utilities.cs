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

namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Utilities
    {

        public static void WriteLinesToStream(Stream stream, string[] lines)
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
            // Flush the writer
            writer.Flush();

            // Leave StreamWriter open or the underlying stream will be closed

            // Reset stream to beginning
            stream.Position = 0;
        }

    }
}
