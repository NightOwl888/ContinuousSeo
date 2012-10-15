// -----------------------------------------------------------------------
// <copyright file="IStreamFactory.cs" company="">
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
    public interface IStreamFactory
    {
        Stream GetFileStream(string path, FileMode fileMode, FileAccess fileAccess);
        Stream GetMemoryStream();
    }
}
