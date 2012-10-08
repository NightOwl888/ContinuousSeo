// -----------------------------------------------------------------------
// <copyright file="IHttpClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.Core.Net
{
    using System;
    using System.Collections.Specialized;
    using System.IO;

    /// <summary>
    /// Interface for HttpClient.
    /// </summary>
    public interface IHttpClient
    {
        NameValueCollection Get(Stream output, string url);
        NameValueCollection Post(Stream output, string url, string data);
        Stream GetResponseStream(string url);
    }
}
