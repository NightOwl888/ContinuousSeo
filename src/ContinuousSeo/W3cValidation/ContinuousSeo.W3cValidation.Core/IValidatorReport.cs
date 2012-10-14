// -----------------------------------------------------------------------
// <copyright file="IValidatorReport.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorReport
    {
        string Url { get; }
        string CheckedBy { get; }
        string Doctype { get; }
        string Charset { get; }
        bool Validity { get; }
        IEnumerable<IValidatorMessage> Errors { get; }
        IEnumerable<IValidatorMessage> Warnings { get; }
    }
}
