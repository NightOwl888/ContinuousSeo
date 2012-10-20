// -----------------------------------------------------------------------
// <copyright file="HtmlXslResourceProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Xsl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlXslResourceProvider : IXslResourceProvider
    {
        #region IXslResourceProvider Members

        public string ResourceLocation
        {
            get { return "ContinuousSeo.W3cValidation.Runner.Xsl.HtmlValidatorIndex.xsl"; }
        }

        #endregion
    }
}
