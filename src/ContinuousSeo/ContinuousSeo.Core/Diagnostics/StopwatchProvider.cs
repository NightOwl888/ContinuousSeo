// -----------------------------------------------------------------------
// <copyright file="StopwatchProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.Core.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class StopwatchProvider
    {
        private static StopwatchProvider current;

        static StopwatchProvider()
        {
            StopwatchProvider.current = new DefaultStopwatchProvider();
        }
        public static StopwatchProvider Current
        {
            get { return StopwatchProvider.current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                StopwatchProvider.current = value;
            }
        }
        public abstract System.Diagnostics.Stopwatch NewStopwatch();

        public static void ResetToDefault()
        {
            StopwatchProvider.current = new DefaultStopwatchProvider();
        }
    }
}
