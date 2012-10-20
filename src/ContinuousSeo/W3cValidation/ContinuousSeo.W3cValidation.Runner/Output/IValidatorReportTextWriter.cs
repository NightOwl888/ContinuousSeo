// -----------------------------------------------------------------------
// <copyright file="IValidationReportXmlTextWriter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using ContinuousSeo.W3cValidation.Runner.UrlProcessors;
    using ContinuousSeo.W3cValidation.Runner.Validators;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorReportTextWriter : IDisposable
    {
        void WriteStartDocument();
        void WriteEndDocument();
        void WriteUrlElement(IValidatorReportItem urlReport);
        void WriteUrlElement(IValidatorReportItem urlReport, Stream response);
        void WriteElapsedTime(IValidatorReportTimes times);
        void Flush();
    }
}
