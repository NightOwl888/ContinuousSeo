// -----------------------------------------------------------------------
// <copyright file="IUrlAggregator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using ContinuousSeo.W3cValidation.Runner.Parsers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUrlAggregator
    {
        IEnumerable<string> ProcessUrl(IUrlFileLineInfo urlInfo);
    }
}
