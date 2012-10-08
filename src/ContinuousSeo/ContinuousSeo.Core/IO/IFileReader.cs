// -----------------------------------------------------------------------
// <copyright file="IFileReader.cs" company="">
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
    public interface IFileReader
    {
        Stream GetFileStream(string path);
    }
}
