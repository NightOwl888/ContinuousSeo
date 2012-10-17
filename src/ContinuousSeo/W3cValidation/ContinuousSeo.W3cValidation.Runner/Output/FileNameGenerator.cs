// -----------------------------------------------------------------------
// <copyright file="FileNameGenerator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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

            // For now, we are just returning a random Guid for the filename
            // The url is being passed so it can somehow be used to make a more sensible choice in the future.
            //return GuidProvider.NewGuid.ToString().ToLowerInvariant() + "." + extension;
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
