// -----------------------------------------------------------------------
// <copyright file="GuidProvider.cs" company="">
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
    public abstract class GuidProvider
    {
        private static GuidProvider current;

        static GuidProvider()
        {
            GuidProvider.current = new DefaultGuidProvider();
        }

        public static GuidProvider Current
        {
            get { return GuidProvider.current; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                GuidProvider.current = value;
            }
        }

        public abstract Guid NewGuid { get; }

        public static void ResetToDefault()
        {
            GuidProvider.current = new DefaultGuidProvider();
        }
    }
}
