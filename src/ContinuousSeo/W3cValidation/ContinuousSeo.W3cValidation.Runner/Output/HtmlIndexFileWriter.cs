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
