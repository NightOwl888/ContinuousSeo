﻿// -----------------------------------------------------------------------
// <copyright file="HttpClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Web;
    using System.Collections.Specialized;
    using System.Net;

    /// <summary>
    /// Used to send either a Get or Post request, filling the request body as appropriate
    /// and returning the payload if the output stream is not null.
    /// </summary>
    public class HttpClient : IHttpClient
    {

        public NameValueCollection Get(Stream output, string url)
        {
            return Post(output, url, String.Empty);
        }


        public NameValueCollection Post(Stream output, string url, string data)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            bool headersOnly = (output == null);

            NameValueCollection result = new NameValueCollection();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (!string.IsNullOrEmpty(data))
            {
                SetRequestStream(request, data);
            }

            // Get the response from the server
            WebResponse response = request.GetResponse();

            if (response != null && response.Headers != null)
            {
                // Get the headers
                result.Add(response.Headers);

                if (!headersOnly)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        responseStream.CopyTo(output);
                    }
                }
            }
           
            return result;
        }


        private void SetRequestStream(HttpWebRequest request, string data)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            request.ContentLength = dataBytes.Length;

            // Load the request data
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(dataBytes, 0, dataBytes.Length);
            }
        }


    }
}
