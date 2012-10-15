// -----------------------------------------------------------------------
// <copyright file="UrlModeProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.Core.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SitemapsParser : ISitemapsParser
    {
        #region Private Members

        private readonly IHttpClient mHttpClient;
        private readonly IStreamFactory mStreamFactory;

        #endregion

        #region Constructor

        public SitemapsParser(IHttpClient httpClient, IStreamFactory streamFactory)
        {
            if (httpClient == null)
                throw new ArgumentNullException("httpClient");
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");

            this.mHttpClient = httpClient;
            this.mStreamFactory = streamFactory;
        }

        #endregion
        
        #region ISitemapsParser Members

        public IEnumerable<string> ParseUrlsFromFile(Stream file)
        {
            var result = new List<String>();
            bool inLoc = false;

            using (XmlTextReader reader = new XmlTextReader(file))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "loc")
                            {
                                inLoc = true;
                            }
                            break;

                        case XmlNodeType.EndElement:
                            inLoc = false;
                            break;

                        default:
                            if (inLoc && reader.HasValue)
                            {
                                result.Add(reader.Value);
                            }
                            break;
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> ParseUrlsFromFile(string pathOrUrl)
        {
            if (pathOrUrl.Contains("/"))
            {
                using (Stream file = mHttpClient.GetResponseStream(pathOrUrl))
                {
                    return ParseUrlsFromFile(file);
                }
            }
            else
            {
                using (Stream file = mStreamFactory.GetFileStream(pathOrUrl, FileMode.Open, FileAccess.Read))
                {
                    return ParseUrlsFromFile(file);
                }
            }
        }


        #endregion

    }
}
