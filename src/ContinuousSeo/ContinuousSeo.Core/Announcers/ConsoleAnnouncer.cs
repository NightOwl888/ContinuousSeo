// -----------------------------------------------------------------------
// <copyright file="ConsoleAnnouncer.cs" company="">
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
    public class ConsoleAnnouncer : Announcer
    {
        public void Header(string toolName)
        {
            if (string.IsNullOrEmpty(toolName))
                throw new ArgumentNullException("toolName");

            int headingLength = (toolName.Length + 2);
            int headingRemaining = 79 - headingLength;
            int headingRight = (int)Math.Round(((double)headingRemaining / (double)2), MidpointRounding.AwayFromZero);
            int headingLeft = headingRemaining - headingRight;

            Console.ForegroundColor = ConsoleColor.Green;
            HorizontalRule();
            Write("=============================== Continuous SEO ================================");
            Write(" ".PadLeft(headingLeft, '=') + toolName + (" ".PadRight(headingRight, '=')));
            HorizontalRule();
            Write("Source Code:");
            Write("  http://github.com/NightOwl888/continuousseo");
            HorizontalRule();
            Console.ResetColor();
        }

        public void HorizontalRule()
        {
            Write("".PadRight(79, '-'));
        }

        public override void Heading(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            HorizontalRule();
            base.Heading(message);
            HorizontalRule();
            Console.ResetColor();
        }

        public override void Say(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            base.Say(string.Format("[+] {0}", message));
            Console.ResetColor();
        }

        public override void ElapsedTime(TimeSpan timeSpan)
        {
            Console.ResetColor();
            base.ElapsedTime(timeSpan);
        }

        public override void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(string.Format("!!! {0}", message));
            Console.ResetColor();
        }

        public void Write(string message)
        {
            Write(message, true);
        }

        public override void Write(string message, bool escaped)
        {
            Console.Out.WriteLine(message);
        }
    }
}
