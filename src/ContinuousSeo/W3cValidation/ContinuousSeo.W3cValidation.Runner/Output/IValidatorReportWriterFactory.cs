// -----------------------------------------------------------------------
// <copyright file="IValidatorReportWriterFactory.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorReportWriterFactory
    {
        IValidatorReportTextWriter GetTextWriter(Stream stream, Encoding encoding);
        IValidatorReportTextWriter GetTextWriter(string filename, Encoding encoding);
    }
}
