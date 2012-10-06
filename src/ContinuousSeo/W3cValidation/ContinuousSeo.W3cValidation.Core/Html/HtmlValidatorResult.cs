// -----------------------------------------------------------------------
// <copyright file="ValidationResult.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core.Html
{

    /// <summary>
    /// The resultant header information from the W3C HTML Validation API.
    /// </summary>
    public class HtmlValidatorResult
    {
        public HtmlValidatorResult(string status, int errors, int warnings, int recursion)
        {
            this.Status = status;
            this.Errors = errors;
            this.Warnings = warnings;
            this.Recursion = recursion;
        }

        public string Status { get; private set; }
        public bool IsValid { get { return Status == "Valid"; } }
        public int Errors { get; private set; }
        public int Warnings { get; private set; }
        public int Recursion { get; private set; }
    }
}
