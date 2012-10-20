// -----------------------------------------------------------------------
// <copyright file="IRunnerContext.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Initialization
{
    using System;
    using System.Diagnostics;
    using ContinuousSeo.Core.Announcers;
   

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRunnerContext
    {
        IAnnouncer Announcer { get; }
        Stopwatch TotalTimeStopwatch { get; }
    }
}
