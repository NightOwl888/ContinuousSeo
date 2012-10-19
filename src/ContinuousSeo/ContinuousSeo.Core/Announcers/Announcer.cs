// -----------------------------------------------------------------------
// <copyright file="Announcer.cs" company="">
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
    public abstract class Announcer : IAnnouncer
    {
        public virtual void Header(string toolName)
        {
            WriteLine(toolName, true);
        }
        
        public virtual void Heading(string message)
        {
            WriteLine(message, true);
        }

        public virtual void Say(string message)
        {
            WriteLine(message, true);
        }

        public virtual void ElapsedTime(TimeSpan timeSpan)
        {
            WriteLine(string.Format("=> {0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds), true);
        }

        public virtual void Error(string message)
        {
            WriteLine(string.Format("!!! {0}", message), true);
        }

        public abstract void Write(string message, bool escaped);
        public abstract void WriteLine(string message, bool escaped);

    }
}
