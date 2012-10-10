// -----------------------------------------------------------------------
// <copyright file="IOutputPathProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IOutputPathProvider
    {
        string GetOutputPath(string url);
        string GetOutputPathWithoutExtension();
        string GetOutputFileName(string url);
        string GetOutputFileNameExtension();
    }
}
