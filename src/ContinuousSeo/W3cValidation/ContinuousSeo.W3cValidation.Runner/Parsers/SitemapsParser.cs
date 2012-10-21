#region Copyright
// -----------------------------------------------------------------------
//
// Copyright (c) 2012, Shad Storhaug <shad@shadstorhaug.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// -----------------------------------------------------------------------
#endregion

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
            var result = new List<string>();
            if (pathOrUrl.Contains("/"))
            {
                try
                {
                    using (Stream file = mHttpClient.GetResponseStream(pathOrUrl))
                    {
                        result.AddRange(ParseUrlsFromFile(file));
                    }
                }
                finally
                {
                    mHttpClient.Close();
                }
            }
            else
            {
                using (Stream file = mStreamFactory.GetFileStream(pathOrUrl, FileMode.Open, FileAccess.Read))
                {
                    result.AddRange(ParseUrlsFromFile(file));
                }
            }
            return result;
        }


        #endregion

    }
}
