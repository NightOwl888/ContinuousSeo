// -----------------------------------------------------------------------
// <copyright file="CssValidatorSettings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSEO.W3CValidation.Core.Css
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CssValidatorSettings
    {
        public CssValidatorSettings()
        {
            CssProfile = "css2";
            Language = "en";
            WarningLevel = "2";
        }

        public string UserMedium { get; set; }
        public string CssProfile { get; set; }
        public string Language { get; set; }
        public string WarningLevel { get; set; }
    }
}
