// -----------------------------------------------------------------------
// <copyright file="IndexFileCreator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.IO;
    using ContinuousSeo.W3cValidation.Runner.Xsl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlIndexFileWriter : IHtmlIndexFileWriter
    {
        private readonly IXslTransformer mXslTransformer;
        private readonly IXslResourceProvider mXslResourceProvider;

        public HtmlIndexFileWriter(
            IXslTransformer xslTransformer,
            IXslResourceProvider xslResourceProvider)
        {
            if (xslTransformer == null)
                throw new ArgumentNullException("xslTransformer");
            if (xslResourceProvider == null)
                throw new ArgumentNullException("xslResourceProvider");

            this.mXslTransformer = xslTransformer;
            this.mXslResourceProvider = xslResourceProvider;
        }

        #region IHtmlIndexFileCreator Members

        public void CreateIndexFile(Stream outputXmlReport, string outputPath)
        {
            using (Stream xsl = Assembly.GetExecutingAssembly().GetManifestResourceStream(mXslResourceProvider.ResourceLocation))
            {
                mXslTransformer.Transform(outputXmlReport, xsl, Path.Combine(outputPath, "index.html"));
            }
        }

        #endregion
    }
}
