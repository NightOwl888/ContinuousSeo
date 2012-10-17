// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorReport.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents detailed report information from the W3C HTML Validator.
    /// </summary>
    public class HtmlValidatorReport : IValidatorReport
    {
        public HtmlValidatorReport(string url, string checkedBy, string doctype, string charset, bool validity, IEnumerable<IValidatorMessage> errors, IEnumerable<IValidatorMessage> warnings)
        {
            this.Url = url;
            this.CheckedBy = checkedBy;
            this.Doctype = doctype;
            this.Charset = charset;
            this.Validity = validity;
            this.Errors = errors;
            this.Warnings = warnings;
        }

        public string Url { get; private set; }
        public string CheckedBy { get; private set; }
        public string Doctype { get; private set; }
        public string Charset { get; private set; }
        public bool Validity { get; private set; }
        public IEnumerable<IValidatorMessage> Errors { get; private set; }
        public IEnumerable<IValidatorMessage> Warnings { get; private set; }
    }
}
