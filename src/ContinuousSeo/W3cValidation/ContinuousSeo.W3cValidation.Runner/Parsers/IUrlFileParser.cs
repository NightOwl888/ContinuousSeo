// -----------------------------------------------------------------------
// <copyright file="IHtmlUrlFileParser.cs" company="">
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
    public interface IUrlFileParser
    {
        IUrlFileLineInfo ParseLine(string line, string[] urlReplacementArgs);
        IEnumerable<IUrlFileLineInfo> ParseFile(Stream file, string[] urlReplacementArgs);
    }
}
