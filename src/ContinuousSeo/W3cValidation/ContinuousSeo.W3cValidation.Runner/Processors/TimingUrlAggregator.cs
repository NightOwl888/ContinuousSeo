// -----------------------------------------------------------------------
// <copyright file="TimingUrlAggregator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    using ContinuousSeo.Core.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class TimingUrlAggregator : UrlAggregator
    {
        protected readonly Stopwatch mStopwatch = StopwatchProvider.Current.NewStopwatch();

        public override IEnumerable<string> AggregateUrls()
        {
            mStopwatch.Reset();
            mStopwatch.Start();

            var result = base.AggregateUrls();

            mStopwatch.Stop();

            return result;
        }
    }
}
