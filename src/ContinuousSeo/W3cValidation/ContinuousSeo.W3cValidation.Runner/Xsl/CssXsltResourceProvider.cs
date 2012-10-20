// -----------------------------------------------------------------------
// <copyright file="CssXslResourceProvider.cs" company="">
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
    public class CssXslResourceProvider : IXslResourceProvider
    {

        #region IXsltResourceProvider Members

        public string ResourceLocation
        {
            get { return "ContinuousSeo.W3cValidation.Runner.Xsl.CssValidatorIndex.xsl"; }
        }

        #endregion
    }
}
