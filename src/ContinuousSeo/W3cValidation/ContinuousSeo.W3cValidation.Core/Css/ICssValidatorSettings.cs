// -----------------------------------------------------------------------
// <copyright file="ICssValidatorSettings.cs" company="">
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
    /// TODO: Update summary.
    /// </summary>
    public interface ICssValidatorSettings
    {
        string UserMedium { get; set; }
        string CssProfile { get; set; }
        string Language { get; set; }
        string WarningLevel { get; set; }
    }
}
