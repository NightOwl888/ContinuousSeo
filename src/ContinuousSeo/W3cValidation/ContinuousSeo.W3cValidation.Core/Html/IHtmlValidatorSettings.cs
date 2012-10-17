// -----------------------------------------------------------------------
// <copyright file="IHtmlValidtorSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHtmlValidatorSettings
    {
        string CharSet { get; set; }
        string DocType { get; set; }
        bool Verbose { get; set; }
        bool Debug { get; set; }
        bool ShowSource { get; set; }
        bool Outline { get; set; }
        bool GroupErrors { get; set; }
        bool UseHtmlTidy { get; set; }
    }
}
