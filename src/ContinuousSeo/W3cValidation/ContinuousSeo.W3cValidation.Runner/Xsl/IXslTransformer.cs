// -----------------------------------------------------------------------
// <copyright file="IXslTransformer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Xsl
{
    using System;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IXslTransformer
    {
        void Transform(Stream xml, Stream xsl, string outputPath);
    }
}
