// -----------------------------------------------------------------------
// <copyright file="StreamFactory.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StreamFactory : IStreamFactory
    {
        #region IStreamFactory Members

        public System.IO.Stream GetFileStream(string path, FileMode fileMode, FileAccess fileAccess)
        {
            return new FileStream(path, fileMode, fileAccess);
        }

        public System.IO.Stream GetMemoryStream()
        {
            return new MemoryStream();
        }

        #endregion
    }
}
