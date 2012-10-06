// -----------------------------------------------------------------------
// <copyright file="HtmlValidator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core.Html
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Collections.Specialized;

    /// <summary>
    /// Class that contains methods that wrap the W3C HTML Validation API at 
    /// http://validator.w3.org/. These methods allow for quick header-only 
    /// inspection of status or to write the output in either SOAP or HTML 
    /// format to a file or stream.
    /// </summary>
    public class HtmlValidator
    {
        const string defaultValidatorAddress = "http://validator.w3.org/check";
        private IHttpClient httpClient;

        public HtmlValidator() :
            this(new HttpClient())
        {
        }

        public HtmlValidator(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        #region Validate Methods Without Payload (Status Only)

        public HtmlValidatorResult Validate(string input)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(string input, HtmlValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(string input, InputFormat inputFormat)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(string input, InputFormat inputFormat, HtmlValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, defaultValidatorAddress);
        }

        //public HtmlValidatorResult Validate(string input, string validatorAddress)
        //{
        //    return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new HtmlValidatorSettings(), validatorAddress);
        //}

        public HtmlValidatorResult Validate(string input, HtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, validatorAddress);
        }

        public HtmlValidatorResult Validate(string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new HtmlValidatorSettings(), validatorAddress);
        }

        public HtmlValidatorResult Validate(string input, InputFormat inputFormat, HtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, validatorAddress);
        }

        #endregion

        #region Validate Methods (Write to File)

        public HtmlValidatorResult Validate(string filename, string input)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(string filename, string input, HtmlValidatorSettings settings)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(filename, outputFormat, input, inputFormat, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, HtmlValidatorSettings settings)
        {
            return Validate(filename, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(string filename, string input, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), validatorAddress);
        }

        public HtmlValidatorResult Validate(string filename, string input, HtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(filename, outputFormat, input, inputFormat, new HtmlValidatorSettings(), validatorAddress);
        }

        public HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, HtmlValidatorSettings settings, string validatorAddress)
        {
            HtmlValidatorResult result = null;

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

        public HtmlValidatorResult Validate(Stream output, string input)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(Stream output, string input, HtmlValidatorSettings settings)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(output, outputFormat, input, inputFormat, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, HtmlValidatorSettings settings)
        {
            return Validate(output, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public HtmlValidatorResult Validate(Stream output, string input, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), validatorAddress);
        }

        public HtmlValidatorResult Validate(Stream output, string input, HtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(output, outputFormat, input, inputFormat, new HtmlValidatorSettings(), validatorAddress);
        }

        public HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, HtmlValidatorSettings settings, string validatorAddress)
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

            if (inputFormat == InputFormat.Fragment)
            {
                headers = this.httpClient.Post(output, validatorAddress, data);
            }
            else
            {
                headers = this.httpClient.Get(output, validatorAddress + "?" + data);
            }

            HtmlValidatorResult result = ParseResult(headers);

            return result;
        }

        #endregion

        #region IO Handling

        private string GetFormData(string input, InputFormat inputFormat, OutputFormat outputFormat, HtmlValidatorSettings settings)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            string data = "uri=" + HttpUtility.UrlEncode(input);

            if (inputFormat == InputFormat.Fragment)
            {
                // fix fragment so it is possible to exclude the <html> and other envelope tags and only test specific tags.
                input = FixHtmlFragment(input);
                data = "fragment=" + HttpUtility.UrlEncode(input);
            }

            if (!string.IsNullOrEmpty(settings.CharSet))
                data += "&charset=" + HttpUtility.UrlEncode(settings.CharSet);

            if (!string.IsNullOrEmpty(settings.DocType))
                data += "&doctype=" + HttpUtility.UrlEncode(settings.DocType);

            if (outputFormat == OutputFormat.Soap12)
                data += "&output=soap12";

            if (settings.Verbose)
                data += "&verbose=1";

            if (settings.Debug)
                data += "&debug=1";

            if (settings.ShowSource)
                data += "&ss=1";

            if (settings.Outline)
                data += "&outline=1";

            return data;
        }

        private HtmlValidatorResult ParseResult(NameValueCollection headers)
        {
            string status = string.Empty;
            int errors = 0;
            int warnings = 0;
            int recursion = 0;
            
            status = headers["X-W3C-Validator-Status"];
            int.TryParse(headers["X-W3C-Validator-Errors"], out errors);
            int.TryParse(headers["X-W3C-Validator-Warnings"], out warnings);
            int.TryParse(headers["X-W3C-Validator-Recursion"], out recursion);

            HtmlValidatorResult result = new HtmlValidatorResult(status, errors, warnings, recursion);
            return result;
        }

        private string FixHtmlFragment(string input)
        {
            if (input.IndexOf("<body>", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                input = "<body>" + input + "</body>";
            }

            if (input.IndexOf("<html>", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                input = "<html><head><title></title></head>" + input + "</html>";
            }

            if (!input.StartsWith("<!DOCTYPE", StringComparison.InvariantCultureIgnoreCase))
            {
                // Default to HTML5 if doctype not supplied
                input = "<!DOCTYPE html>" + input;
            }

            return input;
        }

        #endregion

    }
}
