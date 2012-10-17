// -----------------------------------------------------------------------
// <copyright file="IProjectFileParser.cs" company="">
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
    public interface IProjectFileParser
    {
        IProjectFileLineInfo ParseLine(string line, string[] urlReplacementArgs);
        IEnumerable<IProjectFileLineInfo> ParseFile(Stream file, string[] urlReplacementArgs);
        IEnumerable<IProjectFileLineInfo> ParseFile(string path, string[] urlReplacementArgs);
    }
}
