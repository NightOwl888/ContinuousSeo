// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorMessage.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents error or warning data from the W3C HTML Validator.
    /// </summary>
    public class HtmlValidatorMessage : IValidatorMessage
    {
        public HtmlValidatorMessage(long? line, long? column, string message, string messageId, string explanation, string source)
		{
			this.Line = line;
			this.Column = column;
			this.Message = message;
			this.MessageId = messageId;
			this.Explanation = explanation;
			this.Source = source;
		}

        public long? Line { get; private set; }
        public long? Column { get; private set; }
        public string Message { get; private set; }
        public string MessageId { get; private set; }
        public string Explanation { get; private set; }
        public string Source { get; private set; }
    }
}
