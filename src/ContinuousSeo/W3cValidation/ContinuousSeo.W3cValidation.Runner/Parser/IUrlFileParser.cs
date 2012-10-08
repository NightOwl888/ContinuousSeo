// -----------------------------------------------------------------------
// <copyright file="IHtmlUrlFileParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Parser
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUrlFileParser
    {
        UrlFileLineInfo ParseLine(string line, string[] urlReplacementArgs);
    }
}
