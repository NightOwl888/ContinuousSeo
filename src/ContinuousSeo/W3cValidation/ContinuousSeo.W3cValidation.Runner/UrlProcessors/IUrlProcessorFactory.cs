// -----------------------------------------------------------------------
// <copyright file="IUrlProcessorFactory.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.UrlProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUrlProcessorFactory
    {
        UrlProcessor GetUrlProcessor(string outputFormat);
    }
}
