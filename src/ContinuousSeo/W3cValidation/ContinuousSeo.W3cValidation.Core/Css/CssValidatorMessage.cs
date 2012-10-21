#region Copyright
// -----------------------------------------------------------------------
//
// Copyright (c) 2012, Shad Storhaug <shad@shadstorhaug.com>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// -----------------------------------------------------------------------
#endregion

namespace ContinuousSeo.W3cValidation.Core.Css
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents error or warning data from the W3C CSS Validator.
    /// </summary>
    public class CssValidatorMessage : IValidatorMessage
    {
        public CssValidatorMessage(long? line, long? column, string message, string messageId, string explanation, string source)
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
