// -----------------------------------------------------------------------
// <copyright file="IIndexFileCreator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHtmlIndexFileWriter
    {
        void CreateIndexFile(Stream outputXmlReport, string outputPath);
    }
}
