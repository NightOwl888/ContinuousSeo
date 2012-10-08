// -----------------------------------------------------------------------
// <copyright file="Utilities.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Utilities
    {

        public static void WriteLinesToStream(Stream stream, string[] lines)
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
            // Flush the writer
            writer.Flush();

            // Leave StreamWriter open or the underlying stream will be closed

            // Reset stream to beginning
            stream.Position = 0;
        }

    }
}
