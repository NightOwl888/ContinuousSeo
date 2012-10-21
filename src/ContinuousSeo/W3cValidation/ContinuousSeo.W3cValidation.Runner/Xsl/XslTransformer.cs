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

namespace ContinuousSeo.W3cValidation.Runner.Xsl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Xml;
    using System.Xml.Xsl;
    using System.Xml.XPath;
    using ContinuousSeo.Core.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XslTransformer : IXslTransformer
    {
        private readonly IStreamFactory mStreamFactory;

        public XslTransformer(IStreamFactory streamFactory)
        {
            if (streamFactory == null)
                throw new ArgumentNullException("streamFactory");

            this.mStreamFactory = streamFactory;
        }

        #region IXslTransformer Members

        public void Transform(Stream xml, Stream xsl, string outputPath)
        {
            var transform = new XslCompiledTransform();
            var xmlDocument = new XPathDocument(xml);

            using (var xslReader = XmlReader.Create(xsl, new XmlReaderSettings() { ProhibitDtd = false }))
            {
                using (var output = mStreamFactory.GetFileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    transform.Load(xslReader);
                    transform.Transform(xmlDocument, null, output);
                }
            }
        }

        #endregion
    }
}
