// -----------------------------------------------------------------------
// <copyright file="IUrlProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUrlProcessor
    {
        IEnumerable<IValidatorReportItem> ProcessUrls();
    }
}
