// -----------------------------------------------------------------------
// <copyright file="OutputPathProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.IO;
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutputPathProvider : IOutputPathProvider
    {
        private readonly IValidatorRunnerContext RunnerContext;
        private readonly IFileNameGenerator FileNameGenerator;

        public OutputPathProvider(IValidatorRunnerContext runnerContext, IFileNameGenerator fileNameGenerator)
        {
            if (runnerContext == null)
                throw new ArgumentNullException("runnerContext");
            if (fileNameGenerator == null)
                throw new ArgumentNullException("fileNameGenerator");

            this.RunnerContext = runnerContext;
            this.FileNameGenerator = fileNameGenerator;
        }

        #region IOutputPathProvider Members

        public string GetOutputPath(string url)
        {
            return Path.Combine(GetOutputPath(), GetOutputFileName(url));
        }

        public string GetOutputPath()
        {
            return (RunnerContext.OutputPath == null) ? string.Empty : RunnerContext.OutputPath;
        }

        public string GetOutputFileName(string url)
        {
            var fileNameExtension = GetOutputFileNameExtension();
            return FileNameGenerator.GenerateFileName(url, fileNameExtension);
        }

        public string GetOutputFileNameExtension()
        {
            string result;
            switch (RunnerContext.OutputFormat.ToLowerInvariant())
            {
                case "xml":
                    result = "xml";
                    break;
                default:
                    result = "html";
                    break;
            }
            return result;
        }

        #endregion
    }
}
