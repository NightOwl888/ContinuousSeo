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
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUrlAggregator
    {
        IEnumerable<string> AggregateUrls(HtmlValidatorRunnerContext context);
        //IEnumerable<string> ProcessLine(IUrlFileLineInfo urlInfo);
    }
}
