// -----------------------------------------------------------------------
// <copyright file="FileNameGenerator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using ContinuousSeo.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FileNameGenerator : IFileNameGenerator
    {
        private readonly GuidProvider GuidProvider;

        public FileNameGenerator(GuidProvider guidProvider)
        {
            if (guidProvider == null)
                throw new ArgumentNullException("guidProvider");

            this.GuidProvider = guidProvider;
        }

        #region IFileNameGenerator Members

        public string GenerateFileName(string url, string extension)
        {
            // For now, we are just returning a random Guid for the filename
            // The url is being passed so it can somehow be used to make a more sensible choice in the future.
            return GuidProvider.NewGuid.ToString().ToLowerInvariant() + "." + extension;
        }

        #endregion
    }
}
