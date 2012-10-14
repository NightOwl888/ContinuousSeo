// -----------------------------------------------------------------------
// <copyright file="IValidator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using ContinuousSeo.W3cValidation.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorWrapper
    {
        IValidatorReportItem ValidateUrl(string url, Stream output, OutputFormat outputFormat);
        bool IsDefaultValidatorUrl();
    }
}
