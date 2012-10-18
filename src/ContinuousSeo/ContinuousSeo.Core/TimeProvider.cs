// -----------------------------------------------------------------------
// <copyright file="TimeProvider.cs" company="">
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
    public abstract class TimeProvider
    {
        private static TimeProvider current;

        static TimeProvider()
        {
            TimeProvider.current = new DefaultTimeProvider();
        }
        public static TimeProvider Current
        {
            get { return TimeProvider.current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                TimeProvider.current = value;
            }
        }
        public abstract DateTime UtcNow { get; }

        public static void ResetToDefault()
        {
            TimeProvider.current = new DefaultTimeProvider();
        }
    }
}
