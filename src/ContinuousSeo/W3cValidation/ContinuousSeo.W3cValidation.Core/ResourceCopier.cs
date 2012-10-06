// -----------------------------------------------------------------------
// <copyright file="ResourceCopier.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;

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
