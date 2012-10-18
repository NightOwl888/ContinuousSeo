// -----------------------------------------------------------------------
// <copyright file="DefaultStopwatchProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.Core.Diagnostics
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DefaultStopwatchProvider : StopwatchProvider
    {
        public override System.Diagnostics.Stopwatch NewStopwatch()
        {
            return new System.Diagnostics.Stopwatch();
        }
    }
}
