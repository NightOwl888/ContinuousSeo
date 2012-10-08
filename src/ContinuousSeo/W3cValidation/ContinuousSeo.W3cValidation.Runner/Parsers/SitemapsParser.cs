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
        private readonly IHttpClient HttpClient;
        private readonly IFileReader FileReader;

        public SitemapsParser(IHttpClient httpClient, IFileReader fileReader)
        {
            if (httpClient == null)
                throw new ArgumentNullException("httpClient");
            if (fileReader == null)
                throw new ArgumentNullException("fileReader");

            this.HttpClient = httpClient;
            this.FileReader = fileReader;
        }


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
                using (Stream file = HttpClient.GetResponseStream(pathOrUrl))
                {
                    return ParseUrlsFromFile(file);
                }
            }
            else
            {
                using (Stream file = FileReader.GetFileStream(pathOrUrl))
                {
                    return ParseUrlsFromFile(file);
                }
            }
        }


        #endregion

    }
}
