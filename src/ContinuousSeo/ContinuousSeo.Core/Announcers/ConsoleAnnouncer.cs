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
        public override void Header(string toolName)
        {
            if (string.IsNullOrEmpty(toolName))
                throw new ArgumentNullException("toolName");

            int headingLength = toolName.Length;
            int headingRemaining = 79 - headingLength;
            int headingRight = (int)Math.Round(((double)headingRemaining / (double)2), MidpointRounding.AwayFromZero);
            int headingLeft = headingRemaining - headingRight;

            Console.ForegroundColor = ConsoleColor.Green;
            HorizontalRule();
            WriteLine("|                               Continuous SEO                                |");
            WriteLine("|".PadRight(headingLeft, ' ') + toolName + ("|".PadLeft(headingRight, ' ')));
            HorizontalRule();
            Write("|");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Write("  Source Code:                                                               ");
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine("|");
            Write("|");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Write("    http://github.com/NightOwl888/continuousseo                              ");
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine("|");
            HorizontalRule();
            Console.ResetColor();
        }

        public void HorizontalRule()
        {
            WriteLine("".PadRight(79, '-'));
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
            Console.Out.Write(message);
        }

        public void WriteLine(string message)
        {
            WriteLine(message, true);
        }

        public override void WriteLine(string message, bool escaped)
        {
            Console.Out.WriteLine(message);
        }
    }
}
