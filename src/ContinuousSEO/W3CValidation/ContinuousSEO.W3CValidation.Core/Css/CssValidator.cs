// -----------------------------------------------------------------------
// <copyright file="CssValidator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core.Css
{
    using System;
    using System.IO;
    using System.Web;
    using System.Collections.Specialized;

    /// <summary>
    /// Class that contains methods that wrap the W3C CSS Validation API at 
    /// http://jigsaw.w3.org/css-validator. These methods allow for quick 
    /// header-only inspection of status or to write the output in either 
    /// SOAP or HTML format to a file or stream.
    /// </summary>
    public class CssValidator
    {

        const string defaultValidatorAddress = "http://jigsaw.w3.org/css-validator/validator";
        private IHttpClient httpClient;

        public CssValidator() :
            this(new HttpClient())
        {
        }

        public CssValidator(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region Validate Methods Without Payload (Status Only)

        public CssValidatorResult Validate(string input)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string input, CssValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat, CssValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, defaultValidatorAddress);
        }

        //public CssValidatorResult Validate(string input, string validatorAddress)
        //{
        //    return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new CssValidatorSettings(), validatorAddress);
        //}

        public CssValidatorResult Validate(string input, CssValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, validatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat, CssValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, validatorAddress);
        }

        #endregion

        #region Validate Methods (Write to File)

        public CssValidatorResult Validate(string filename, string input)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, string input, CssValidatorSettings settings)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(filename, outputFormat, input, inputFormat, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, CssValidatorSettings settings)
        {
            return Validate(filename, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, string input, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(string filename, string input, CssValidatorSettings settings, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(filename, outputFormat, input, inputFormat, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, CssValidatorSettings settings, string validatorAddress)
        {
            CssValidatorResult result = null;

            // Create the directory automatically if it doesn't exist.
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileStream output = new FileStream(filename, FileMode.Create);
            try
            {
                result = Validate(output, outputFormat, input, inputFormat, settings, validatorAddress);
            }
            finally
            {
                output.Flush();
                output.Dispose();
            }

            return result;
        }

        #endregion

        #region Validate Methods (Write to Stream)

        public CssValidatorResult Validate(Stream output, string input)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, string input, CssValidatorSettings settings)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(output, outputFormat, input, inputFormat, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, CssValidatorSettings settings)
        {
            return Validate(output, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, string input, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(Stream output, string input, CssValidatorSettings settings, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(output, outputFormat, input, inputFormat, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, CssValidatorSettings settings, string validatorAddress)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (string.IsNullOrEmpty(validatorAddress))
                validatorAddress = defaultValidatorAddress;

            string data = GetFormData(input, inputFormat, outputFormat, settings);
            NameValueCollection headers;

            headers = this.httpClient.Get(output, validatorAddress + "?" + data);

            CssValidatorResult result = ParseResult(headers);

            return result;
        }

        #endregion

        #region IO Handling

        private string GetFormData(string input, InputFormat inputFormat, OutputFormat outputFormat, CssValidatorSettings settings)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            string data = "uri=" + HttpUtility.UrlEncode(input);

            if (inputFormat == InputFormat.Fragment)
            {
                data = "text=" + HttpUtility.UrlEncode(input);
            }

            if (!string.IsNullOrEmpty(settings.UserMedium))
                data += "&usermedium=" + HttpUtility.UrlEncode(settings.UserMedium);

            if (!string.IsNullOrEmpty(settings.CssProfile))
                data += "&profile=" + HttpUtility.UrlEncode(settings.CssProfile);

            if (!string.IsNullOrEmpty(settings.Language))
                data += "&lang=" + HttpUtility.UrlEncode(settings.Language);

            if (!string.IsNullOrEmpty(settings.WarningLevel))
                data += "&warning=" + HttpUtility.UrlEncode(settings.WarningLevel);

            if (outputFormat == OutputFormat.Soap12)
                data += "&output=soap12";

            return data;
        }

        private CssValidatorResult ParseResult(NameValueCollection headers)
        {
            string status = string.Empty;
            int errors = 0;

            status = headers["X-W3C-Validator-Status"];
            int.TryParse(headers["X-W3C-Validator-Errors"], out errors);

            CssValidatorResult result = new CssValidatorResult(status, errors);

            return result;
        }

        #endregion

    }
}
