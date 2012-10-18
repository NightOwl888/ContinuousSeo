// -----------------------------------------------------------------------
// <copyright file="IAnnouncer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.Core.Announcers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAnnouncer
    {
        void Heading(string message);
        void Say(string message);
        void ElapsedTime(TimeSpan timeSpan);
        void Error(string message);
        void Write(string message, bool escaped);
    }
}
