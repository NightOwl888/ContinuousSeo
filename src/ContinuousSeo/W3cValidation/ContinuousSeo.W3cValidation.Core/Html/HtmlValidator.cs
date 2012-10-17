﻿// -----------------------------------------------------------------------
// <copyright file="HtmlValidator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Web;
    using System.Collections.Specialized;
    using ContinuousSeo.Core.Net;

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
        private ResourceCopier resourceCopier;

        public HtmlValidator() :
            this(new HttpClient(), new HtmlValidatorResourceCopier())
        {
        }

        public HtmlValidator(IHttpClient httpClient, ResourceCopier resourceCopier)
        {
            this.httpClient = httpClient;
            this.resourceCopier = resourceCopier;
        }

        #region IsDefaultValidatorAddress

        public virtual bool IsDefaultValidatorAddress(string url)
        {
            return (string.Compare(url, defaultValidatorAddress, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        #endregion

        #region Validate Methods Without Payload (Status Only)

        public virtual HtmlValidatorResult Validate(string input)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string input, IHtmlValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string input, InputFormat inputFormat)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string input, InputFormat inputFormat, IHtmlValidatorSettings settings)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, defaultValidatorAddress);
        }

        //public virtual HtmlValidatorResult Validate(string input, string validatorAddress)
        //{
        //    return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, new HtmlValidatorSettings(), validatorAddress);
        //}

        public virtual HtmlValidatorResult Validate(string input, IHtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, InputFormat.Uri, settings, validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, new HtmlValidatorSettings(), validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string input, InputFormat inputFormat, IHtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate((Stream)null, OutputFormat.Soap12, input, inputFormat, settings, validatorAddress);
        }

        #endregion

        #region Validate Methods (Write to File)

        public virtual HtmlValidatorResult Validate(string filename, string input)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string filename, string input, IHtmlValidatorSettings settings)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(filename, outputFormat, input, inputFormat, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, IHtmlValidatorSettings settings)
        {
            return Validate(filename, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string filename, string input, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string filename, string input, IHtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate(filename, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(filename, outputFormat, input, inputFormat, new HtmlValidatorSettings(), validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(string filename, OutputFormat outputFormat, string input, InputFormat inputFormat, IHtmlValidatorSettings settings, string validatorAddress)
        {
            HtmlValidatorResult result = null;

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
                resourceCopier.CopyResources(directory);
            }

            using (FileStream output = new FileStream(filename, FileMode.Create))
            {
                result = Validate(output, outputFormat, input, inputFormat, settings, validatorAddress);
            }

            return result;
        }

        #endregion

        #region Validate Methods (Write to Stream)

        public virtual HtmlValidatorResult Validate(Stream output, string input)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(Stream output, string input, IHtmlValidatorSettings settings)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat)
        {
            return Validate(output, outputFormat, input, inputFormat, new HtmlValidatorSettings(), defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, IHtmlValidatorSettings settings)
        {
            return Validate(output, outputFormat, input, inputFormat, settings, defaultValidatorAddress);
        }

        public virtual HtmlValidatorResult Validate(Stream output, string input, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, new HtmlValidatorSettings(), validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(Stream output, string input, IHtmlValidatorSettings settings, string validatorAddress)
        {
            return Validate(output, OutputFormat.Html, input, InputFormat.Uri, settings, validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, string validatorAddress)
        {
            return Validate(output, outputFormat, input, inputFormat, new HtmlValidatorSettings(), validatorAddress);
        }

        public virtual HtmlValidatorResult Validate(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, IHtmlValidatorSettings settings, string validatorAddress)
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

            
            NameValueCollection headers;

            //int tryCount = 0;
            //do
            //{
            //    if (tryCount > 0)
            //    {
            //        if (IsDefaultValidatorAddress(validatorAddress))
            //        {
            //            System.Threading.Thread.Sleep(1000);
            //        }

            //        // Clear the stream from the last loop or we will
            //        // double the stream content.
            //        output.SetLength(0);
            //    }

                string data = GetFormData(input, inputFormat, outputFormat, settings);

                if (inputFormat == InputFormat.Fragment)
                {
                    headers = this.httpClient.Post(output, validatorAddress, data);
                }
                else
                {
                    headers = this.httpClient.Get(output, validatorAddress + "?" + data);
                }

            //    // W3C API BUG: for some reason it doesn't return headers every time
            //    // it is accessed. Connecting a second time seems to usually work, so we will try
            //    // again up to 4 times.
            //    tryCount++;
            //} while (headers["X-W3C-Validator-Status"] == null && tryCount <= 4 && outputFormat == OutputFormat.Html);

            HtmlValidatorResult result = ParseResult(headers);

            // Check if any headers were returned
            if (string.IsNullOrEmpty(result.Status))
            {
                result = FixBrokenHeaders(output, outputFormat, input, inputFormat, settings, validatorAddress);

                //Stream checkStream = null;
                //if (outputFormat == OutputFormat.Html || output.CanRead == false)
                //{
                //    if (IsDefaultValidatorAddress(validatorAddress))
                //    {
                //        System.Threading.Thread.Sleep(1000);
                //    }

                //    // Headers failed, so we will get the report again in Soap 1.2 format in 
                //    // an in memory stream
                //    checkStream = new MemoryStream();
                //    string checkData = GetFormData(input, inputFormat, OutputFormat.Soap12, settings);

                //    // This time, ignore headers.
                //    if (inputFormat == InputFormat.Fragment)
                //    {
                //        this.httpClient.Post(checkStream, validatorAddress, data);
                //    }
                //    else
                //    {
                //        this.httpClient.Get(checkStream, validatorAddress + "?" + data);
                //    }
                //}
                //else
                //{
                //    output.Position = 0;
                //    output.CopyTo(checkStream);
                //}

                //var parser = new HtmlValidatorSoap12ResponseParser();
                //var response = parser.ParseResponse(checkStream);

                //var errors = response.Errors.Count();
                //var warnings = response.Warnings.Count();
                //var status = response.Validity ? "Valid" : "Invalid";
                //var recursion = 1;

                //result = new HtmlValidatorResult(status, errors, warnings, recursion);
            }

            return result;
        }

        #endregion

        #region IO Handling

        private string GetFormData(string input, InputFormat inputFormat, OutputFormat outputFormat, IHtmlValidatorSettings settings)
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

            if (settings.GroupErrors)
                data += "&group=1";

            if (settings.UseHtmlTidy)
                data += "&st=1";

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

        private HtmlValidatorResult FixBrokenHeaders(Stream output, OutputFormat outputFormat, string input, InputFormat inputFormat, IHtmlValidatorSettings settings, string validatorAddress)
        {
            Stream checkStream = new MemoryStream();
            if (outputFormat == OutputFormat.Html || output.CanRead == false)
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
                    this.httpClient.Post(checkStream, validatorAddress, checkData);
                }
                else
                {
                    this.httpClient.Get(checkStream, validatorAddress + "?" + checkData);
                }
            }
            else
            {
                output.Position = 0;
                output.CopyTo(checkStream);
            }

            checkStream.Position = 0;
            var parser = new HtmlValidatorSoap12ResponseParser();
            var response = parser.ParseResponse(checkStream);

            var errors = response.Errors.Count();
            var warnings = response.Warnings.Count();
            var status = response.Validity ? "Valid" : "Invalid";
            var recursion = 1;

            var result = new HtmlValidatorResult(status, errors, warnings, recursion);
            return result;
        }


        #endregion

    }
}
