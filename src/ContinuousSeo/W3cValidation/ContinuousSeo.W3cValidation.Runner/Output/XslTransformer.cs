// -----------------------------------------------------------------------
// <copyright file="XslTransformer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
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

            using (var xslReader = XmlReader.Create(xsl, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
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
