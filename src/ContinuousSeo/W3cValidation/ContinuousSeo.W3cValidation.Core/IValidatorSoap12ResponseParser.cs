// -----------------------------------------------------------------------
// <copyright file="IValidatorSoap12ResponseParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core
{
    using System;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorSoap12ResponseParser
    {
        IValidatorReport ParseResponse(Stream response);
    }
}
