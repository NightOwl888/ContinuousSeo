// -----------------------------------------------------------------------
// <copyright file="IHtmlValidatorIndexFileCreator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using ContinuousSeo.W3cValidation.Runner.Processors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHtmlValidatorIndexFileCreator
    {
        void CreateIndexFile(Stream output, IEnumerable<IValidatorReportItem> reports);
    }
}
