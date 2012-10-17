// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorRunnerContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Initialization
{
    using System;
    using System.Collections.Generic;
    using ContinuousSeo.W3cValidation.Core.Html;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorRunnerContext : IValidatorRunnerContext, IHtmlValidatorSettings
    {

        public virtual IEnumerable<string> TargetSitemapsFiles { get; set; }


        #region IValidatorRunnerContext Members

        public virtual IEnumerable<string> TargetUrls { get; set; }
        public virtual IEnumerable<string> TargetUrlFiles { get; set; }
        public virtual IEnumerable<string> UrlReplacementArgs { get; set; }
        public virtual string OutputPath { get; set; } // Directory to write output files
        public virtual string OutputFormat { get; set; } // Currently only accepts Html
        public virtual string ValidatorUrl { get; set; }

        #endregion

        #region IHtmlValidatorSettings Members

        public virtual string CharSet { get; set; }
        public virtual string DocType { get; set; }
        public virtual bool Verbose { get; set; }
        public virtual bool Debug { get; set; }
        public virtual bool ShowSource { get; set; }
        public virtual bool Outline { get; set; }
        public virtual bool GroupErrors { get; set; }
        public virtual bool UseHtmlTidy { get; set; }

        #endregion
    }
}
