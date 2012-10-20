// -----------------------------------------------------------------------
// <copyright file="StreamFactory.cs" company="">
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
    public class StreamFactory : IStreamFactory
    {
        #region IStreamFactory Members

        public System.IO.Stream GetFileStream(string path, FileMode fileMode, FileAccess fileAccess)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return new FileStream(path, fileMode, fileAccess);
        }

        public System.IO.Stream GetMemoryStream()
        {
            return new MemoryStream();
        }

        #endregion
    }
}
