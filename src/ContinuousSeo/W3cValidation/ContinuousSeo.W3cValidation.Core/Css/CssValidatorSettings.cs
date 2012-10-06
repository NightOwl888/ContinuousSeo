// -----------------------------------------------------------------------
// <copyright file="CssValidatorSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Css
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Advanced settings to be passed in to the CSS W3C validator API. These
    /// settings are documented at http://jigsaw.w3.org/css-validator/manual.html#requestformat.
    /// </summary>
    public class CssValidatorSettings
    {
        //public CssValidatorSettings()
        //{
        //    CssProfile = "css2";
        //    Language = "en";
        //    WarningLevel = "2";
        //}

        public string UserMedium { get; set; }
        public string CssProfile { get; set; }
        public string Language { get; set; }
        public string WarningLevel { get; set; }
    }
}
