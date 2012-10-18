// -----------------------------------------------------------------------
// <copyright file="CssValidator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Css
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Collections.Specialized;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.Core.IO;

    /// <summary>
    /// Class that contains methods that wrap the W3C CSS Validation API at 
    /// http://jigsaw.w3.org/css-validator. These methods allow for quick 
    /// header-only inspection of status or to write the output in either 
    /// SOAP or HTML format to a file or stream.
    /// </summary>
    public class CssValidator
    {
        const string defaultValidatorAddress = "http://jigsaw.w3.org/css-validator/validator";
        private readonly IHttpClient mHttpClient;
        private readonly IStreamFactory mStreamFactory;
        private readonly ResourceCopier mResourceCopier;
        private readonly IValidatorSoap12ResponseParser mSoapResponseParser;

        public CssValidator() :
            this(new HttpClient(), new StreamFactory(), new CssValidatorResourceCopier(), new CssValidatorSoap12ResponseParser())
        {
        }

        public CssValidator(IHttpClient httpClient, IStreamFactory streamFactory, ResourceCopier resourceCopier, IValidatorSoap12ResponseParser soapResponseParser)
        {
            if (httpClient == null)
                throw new ArgumentNullException("httpClient");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");
            if (resourceCopier == null)
                throw new ArgumentNullException("resourceCopier");
            if (soapResponseParser == null)
                throw new ArgumentNullException("soapResponseParser");

            this.mHttpClient = httpClient;
            this.mStreamFactory = streamFactory;
            this.mResourceCopier = resourceCopier;
            this.mSoapResponseParser = soapResponseParser;
        }

        #region IsDefaultValidatorAddress

        public virtual bool IsDefaultValidatorAddress(string url)
        {
            return (string.Compare(url, defaultValidatorAddress, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        #endregion

        #region Validate Methods Without Payload (Status Only)

        public CssValidatorResult Validate(string input)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string input, ICssValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat, ICssValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, defaultValidatorAddress);
        }

        //public CssValidatorResult Validate(string input, string validatorAddress)
        //{
        //    return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new CssValidatorSettings(), validatorAddress);
        //}

        public CssValidatorResult Validate(string input, ICssValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, validatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(string input, InputFormat inputFormat, ICssValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, validatorAddress);
        }

        #endregion

        #region Validate Methods (Write to File)

        public CssValidatorResult Validate(string filename, string input)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, string input, ICssValidatorSettings settings)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(filename, outputFormat, input, inputFormat, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, ICssValidatorSettings settings)
        {
            return Validate(filename, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(string filename, string input, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(string filename, string input, ICssValidatorSettings settings, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(filename, outputFormat, input, inputFormat, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, ICssValidatorSettings settings, string validatorAddress)
        {
            CssValidatorResult result = null;

            // Create the directory automatically if it doesn't exist.
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (outputFormat == OutputFormat.Html)
            {
                // Copy the resources to the output directory if they don't already exist.
                // This is to ensure all dependencies of the HTML document are there.
                mResourceCopier.CopyResources(directory);
            }

            using (FileStream output = new FileStream(filename, FileMode.Create))
            {
                result = Validate(output, outputFormat, input, inputFormat, settings, validatorAddress);
            }

            return result;
        }

        #endregion

        #region Validate Methods (Write to Stream)

        public CssValidatorResult Validate(Stream output, string input)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, string input, ICssValidatorSettings settings)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(output, outputFormat, input, inputFormat, new CssValidatorSettings(), defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, ICssValidatorSettings settings)
        {
            return Validate(output, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public CssValidatorResult Validate(Stream output, string input, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(Stream output, string input, ICssValidatorSettings settings, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(output, outputFormat, input, inputFormat, new CssValidatorSettings(), validatorAddress);
        }

        public CssValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, ICssValidatorSettings settings, string validatorAddress)
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

            headers = this.mHttpClient.Get(output, validatorAddress + "?" + data);

            CssValidatorResult result = ParseResult(headers);

            if (string.IsNullOrEmpty(result.Status))
            {
                result = FixBrokenHeaders(output, outputFormat, input, inputFormat, settings, validatorAddress);
            }

            return result;
        }

        #endregion

        #region IO Handling

        private string GetFormData(string input, InputFormat inputFormat, OutputFormat outputFormat, ICssValidatorSettings settings)
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

        private CssValidatorResult FixBrokenHeaders(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, ICssValidatorSettings settings, string validatorAddress)
        {
            Stream checkStream = mStreamFactory.GetMemoryStream();
            if (outputFormat != OutputFormat.Soap12 || output.CanRead == false)
            {
                if (IsDefaultValidatorAddress(validatorAddress))
                {
                    System.Threading.Thread.Sleep(1000);
                }

                // Headers failed, so we will get the report again in Soap 1.2 format in 
                // an in memory stream
                string checkData = GetFormData(input, inputFormat, OutputFormat.Soap12, settings);

                // This time, ignore headers.
                if (inputFormat == InputFormat.Fragment)
                {
                    this.mHttpClient.Post(checkStream, validatorAddress, checkData);
                }
                else
                {
                    this.mHttpClient.Get(checkStream, validatorAddress + "?" + checkData);
                }
            }
            else
            {
                output.Position = 0;
                output.CopyTo(checkStream);
            }

            checkStream.Position = 0;
            var response = mSoapResponseParser.ParseResponse(checkStream);

            var errors = response.Errors.Count();
            var status = response.Validity ? "Valid" : "Invalid";

            var result = new CssValidatorResult(status, errors);
            return result;
        }

        #endregion

    }
}
