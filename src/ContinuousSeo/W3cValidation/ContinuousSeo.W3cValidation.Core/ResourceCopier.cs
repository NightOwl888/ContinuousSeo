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

namespace ContinuousSeo.W3cValidation.Core
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;
    using ContinuousSeo.Core.IO;

    /// <summary>
    /// Abstract base class with common methods used to copy resources from within this
    /// assembly to a specified destinationPath.
    /// </summary>
    public abstract class ResourceCopier
    {
        public void CopyResources(string destinationPath)
        {
            var resources = GetResourceMap();

            foreach (string key in resources.Keys)
            {
                var source = key;
                var destination = Path.Combine(destinationPath, resources[key]);
                CopyResource(source, destination);
            }
        }

        protected abstract Dictionary<string, string> GetResourceMap();

        protected void CopyResource(string sourcePath, string destinationPath)
        {
            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentNullException("sourcePath");
            }

            if (string.IsNullOrEmpty(destinationPath))
            {
                throw new ArgumentNullException("destinationPath");
            }

            // Create the destination directory if it doesn't exist
            string directory = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Copy the file from the resource to destination if it doesn't exist
            if (!File.Exists(destinationPath))
            {
                using (Stream sourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sourcePath))
                {
                    if (sourceStream == null)
                    {
                        throw new ArgumentException("sourcePath '" + sourcePath + "' doesn't exist as an embedded resource. " + 
                            "Use GetManifestResourceNames() to discover the available paths to use.");
                    }

                    using (FileStream destinationStream = new FileStream(destinationPath, FileMode.Create))
                    {
                        sourceStream.CopyTo(destinationStream);
                    }
                }
            }
        }
    }
}
