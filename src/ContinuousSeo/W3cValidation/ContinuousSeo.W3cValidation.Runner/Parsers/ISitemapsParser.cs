// -----------------------------------------------------------------------
// <copyright file="IUrlModeProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISitemapsParser
    {
        IEnumerable<string> ParseUrlsFromFile(Stream file);
        IEnumerable<string> ParseUrlsFromFile(string pathOrUrl);
    }
}
