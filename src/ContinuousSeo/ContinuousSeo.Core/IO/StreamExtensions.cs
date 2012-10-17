// -----------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.Core.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class StreamExtensions
    {
        public static void CopyTo(this Stream source, Stream destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            if (!source.CanRead && !source.CanWrite)
            {
                throw new ObjectDisposedException(null, "Cannot access a closed stream.");
            }
            if (!destination.CanRead && !destination.CanWrite)
            {
                throw new ObjectDisposedException("destination", "Cannot access a closed stream.");
            }
            if (!source.CanRead)
            {
                throw new NotSupportedException("Source stream is not readable.");
            }
            if (!destination.CanWrite)
            {
                throw new NotSupportedException("Destination stream is not writable.");
            }

            int num;
            byte[] buffer = new byte[4096];
            while ((num = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, num);
            }
        }
    }
}
