//// -----------------------------------------------------------------------
//// <copyright file="Stopwatch.cs" company="">
//// TODO: Update copyright text.
//// </copyright>
//// -----------------------------------------------------------------------

//namespace ContinuousSeo.Core.Diagnostics
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;

//    /// <summary>
//    /// TODO: Update summary.
//    /// </summary>
//    public class Stopwatch : IStopwatch
//    {
//        public static Func<DateTime> TimeNow = () => DateTime.UtcNow;

//        public DateTime StartTime { get; private set; }
//        public DateTime EndTime { get; private set; }

//        public void Start()
//        {
//            StartTime = TimeNow();
//        }

//        public void Stop()
//        {
//            EndTime = TimeNow();
//        }

//        public TimeSpan ElapsedTime()
//        {
//            return EndTime - StartTime;
//        }
//    }
//}
