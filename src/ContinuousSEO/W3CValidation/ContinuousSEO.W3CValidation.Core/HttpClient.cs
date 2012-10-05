// -----------------------------------------------------------------------
// <copyright file="HttpClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Web;
    using System.Collections.Specialized;
    using System.Net;

    /// <summary>
    /// Used to send either a Get or Post request, filling the request body as appropriate.
    /// </summary>
    public class HttpClient : IHttpClient
    {

        public NameValueCollection Get(Stream output, string url)
        {
            return Post(output, url, String.Empty);
        }


        public NameValueCollection Post(Stream output, string url, string data)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            NameValueCollection result = new NameValueCollection();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (!string.IsNullOrEmpty(data))
            {
                SetRequestStream(request, data);
            }

            // Get the response from the server
            WebResponse response = request.GetResponse();

            // Get the headers
            result.Add(response.Headers);

            Stream responseStream = response.GetResponseStream();
            try
            {
                responseStream.CopyTo(output);
            }
            finally
            {
                responseStream.Close();
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
            Stream requestStream = request.GetRequestStream();
            try
            {
                requestStream.Write(dataBytes, 0, dataBytes.Length);
            }
            finally
            {
                requestStream.Close();
            }
        }


    }
}
