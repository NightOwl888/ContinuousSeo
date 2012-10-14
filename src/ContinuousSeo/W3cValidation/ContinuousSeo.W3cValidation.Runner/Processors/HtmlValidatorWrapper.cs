// -----------------------------------------------------------------------
// <copyright file="HtmlValidator.cs" company="">
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
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Initialization;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorWrapper : IValidatorWrapper
    {
        private readonly HtmlValidator mValidator;
        private readonly HtmlValidatorRunnerContext mContext;

        public HtmlValidatorWrapper(HtmlValidator validator, HtmlValidatorRunnerContext context)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (context == null)
                throw new ArgumentNullException("context");

            mValidator = validator;
            mContext = context;
        }

        #region IValidator Members

        public IValidatorReportItem ValidateUrl(string url, Stream output, OutputFormat outputFormat)
        {
            if (!IsValidUrl(url))
            {
                return ReportFailure(url, string.Empty, "The url is not in a valid format.");
            }

            // wrap in try/catch so any error can be reported in the output
            try
            {
                var validatorUrl = mContext.ValidatorUrl;

                var validatorResult = mValidator.Validate(output, outputFormat, url, InputFormat.Uri, mContext, validatorUrl);

                return ReportSuccess(
                    url,
                    GetDomainName(url),
                    validatorResult.Errors,
                    validatorResult.Warnings,
                    validatorResult.IsValid);
            }
            catch (Exception ex)
            {
                return ReportFailure(url, GetDomainName(url), ex.Message);
            }
        }

        public bool IsDefaultValidatorUrl()
        {
            string validatorUrl = mContext.ValidatorUrl;
            return (string.IsNullOrEmpty(validatorUrl) || mValidator.IsDefaultValidatorAddress(validatorUrl));
        }

        #endregion


        #region Private Members

        private IValidatorReportItem ReportSuccess(string url, string domainName, int errors, int warnings, bool isValid)
        {
            var result = new ValidatorReportItem();
            result.Url = url;
            result.DomainName = domainName;
            // TODO: Add elapsed time to the report.
            //result.FileName = fileName;
            result.Errors = errors;
            result.Warnings = warnings;
            result.IsValid = isValid;
            return result;
        }

        private IValidatorReportItem ReportFailure(string url, string domainName, string errorMessage)
        {
            var result = new ValidatorReportItem();
            result.Url = url;
            result.DomainName = domainName;
            result.ErrorMessage = errorMessage;
            result.IsValid = false;
            return result;
        }

        private bool IsValidUrl(string url)
        {
            Uri uri;
            return (Uri.TryCreate(url, UriKind.Absolute, out uri));
        }

        private string GetDomainName(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                return uri.IsDefaultPort ? uri.DnsSafeHost : uri.DnsSafeHost + ":" + uri.Port;
            }
            return string.Empty;
        }

        #endregion


        
    }
}
