// -----------------------------------------------------------------------
// <copyright file="ValidationReport.cs" company="">
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
    public class ValidatorReportItem : IValidatorReportItem
    {
        #region IValidationReport Members

        public string DomainName { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public bool IsValid { get; set; }
        public int Errors { get; set; }
        public int Warnings { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
    }
}
