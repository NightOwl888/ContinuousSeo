﻿// -----------------------------------------------------------------------
// <copyright file="CssValidatorResult.cs" company="">
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
    public class CssValidatorResult
    {
        public CssValidatorResult(string status, int errors)
        {
            this.Status = status;
            this.Errors = errors;
        }

        public string Status { get; private set; }
        public bool IsValid { get { return Status == "Valid"; } }
        public int Errors { get; private set; }
    }
}
