// -----------------------------------------------------------------------
// <copyright file="IValidatorMessage.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IValidatorMessage
    {
        long? Line { get; }
        long? Column { get; }
        string Message { get; }
        string MessageId { get; }
        string Explanation { get; }
        string Source { get; }
    }
}
