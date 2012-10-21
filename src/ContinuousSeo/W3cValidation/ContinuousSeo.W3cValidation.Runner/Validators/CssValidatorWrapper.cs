#region Copyright
// -----------------------------------------------------------------------
//
// Copyright (c) 2012, Shad Storhaug <shad@shadstorhaug.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// -----------------------------------------------------------------------
#endregion

namespace ContinuousSeo.W3cValidation.Runner.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Css;
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CssValidatorWrapper : IValidatorWrapper
    {
        private readonly CssValidator mValidator;
        private readonly ICssValidatorRunnerContext mContext;
        private readonly IHttpClient mHttpClient;

        public CssValidatorWrapper(CssValidator validator, ICssValidatorRunnerContext context, IHttpClient httpClient)
        {
            if (validator == null)
                throw new ArgumentNullException("validator");
            if (context == null)
                throw new ArgumentNullException("context");
            if (httpClient == null)
                throw new ArgumentNullException("httpClient");

            mValidator = validator;
            mContext = context;
            mHttpClient = httpClient;
        }

        #region IValidator Members

        public IValidatorReportItem ValidateUrl(string url, Stream output, OutputFormat outputFormat)
        {
            if (!IsValidUrl(url))
            {
                return ReportFailure(url, string.Empty, "The url is not in a valid format.");
            }

            var validatorUrl = mContext.ValidatorUrl;
            var input = url;
            var inputFormat = InputFormat.Uri;
            
            // wrap in try/catch so any error can be reported in the output
            try
            {
                if (mContext.DirectInputMode)
                {
                    // Retrieve the text from the webserver and input it directly
                    // to the W3C API
                    inputFormat = InputFormat.Fragment;
                    input = mHttpClient.GetResponseText(url);
                }

                var validatorResult = mValidator.Validate(output, outputFormat, input, inputFormat, mContext, validatorUrl);

                return ReportSuccess(
                    url,
                    GetDomainName(url),
                    validatorResult.Errors,
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

        private IValidatorReportItem ReportSuccess(string url, string domainName, int errors, bool isValid)
        {
            var result = new ValidatorReportItem();
            result.Url = url;
            result.DomainName = domainName;
            // TODO: Add elapsed time to the report.
            //result.FileName = fileName;
            result.Errors = errors;
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
            mContext.Announcer.Error(errorMessage);
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
