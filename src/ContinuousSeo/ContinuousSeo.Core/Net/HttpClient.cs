// -----------------------------------------------------------------------
// <copyright file="HttpClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.Core.Net
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Collections.Specialized;
    using System.Net;
    using ContinuousSeo.Core.IO;

    /// <summary>
    /// Used to send either a Get or Post request, filling the request body as appropriate
    /// and returning the payload if the output stream is not null.
    /// </summary>
    public class HttpClient : IHttpClient, IDisposable
    {
        private WebResponse mWebResponse = null;


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
            using (WebResponse response = request.GetResponse())
            {
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
            }

            return result;
        }

        public Stream GetResponseStream(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            this.mWebResponse = request.GetResponse();
            return mWebResponse.GetResponseStream();
        }

        public void Close()
        {
            this.Dispose();
        }

        public string GetResponseText(string url)
        {
            string result = string.Empty;
            using (var client = new WebClient())
            {
                // Gets the encoding from the HTTP header information automatically
                // and falls back to byte order mark if header not supplied.
                result = client.DownloadString(url);
            }
            return result;

            //string result = string.Empty;
            //using (var document = new MemoryStream())
            //{
            //    Get(document, url);
            //    document.Position = 0;
            //    using (var reader = new StreamReader(document))
            //    {
            //        result = reader.ReadToEnd();
            //    }
            //}
            //return result;
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



        #region IDisposable Members

        public void Dispose()
        {
            if (mWebResponse != null)
            {
                mWebResponse.Close();
            }
        }

        #endregion
    }
}
