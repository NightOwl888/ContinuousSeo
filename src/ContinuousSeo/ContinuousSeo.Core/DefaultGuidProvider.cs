// -----------------------------------------------------------------------
// <copyright file="DefaultGuidProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DefaultGuidProvider : GuidProvider
    {
        public override Guid NewGuid
        {
            get { return Guid.NewGuid(); }
        }
    }
}
