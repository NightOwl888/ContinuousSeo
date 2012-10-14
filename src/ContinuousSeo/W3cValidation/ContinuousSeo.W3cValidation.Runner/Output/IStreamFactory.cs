// -----------------------------------------------------------------------
// <copyright file="IStreamFactory.cs" company="">
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
    public interface IStreamFactory
    {
        Stream GetFileStream(string path, FileMode fileMode, FileAccess fileAccess);
        Stream GetMemoryStream();
    }
}
