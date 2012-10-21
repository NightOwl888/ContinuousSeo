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
    using System.IO;
    using System.Xml;
    using ContinuousSeo.W3cValidation.Core;

    /// <summary>
    /// Parses the response from the W3C CSS Validator.
    /// </summary>
    public class CssValidatorSoap12ResponseParser : IValidatorSoap12ResponseParser
    {
        #region IHtmlValidatorResponseParser Members

        public IValidatorReport ParseResponse(Stream response)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(response);

            var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);

            // this is a hack. how to get them auto from the document?
            xmlNamespaceManager.AddNamespace("env", "http://www.w3.org/2003/05/soap-envelope");
            xmlNamespaceManager.AddNamespace("m", "http://www.w3.org/2005/10/markup-validator");

            var validatorResponse = xmlDocument.SelectSingleNode("env:Envelope/env:Body/m:markupvalidationresponse", xmlNamespaceManager);

            var uri = validatorResponse.SelectSingleNode("m:uri", xmlNamespaceManager).InnerText;
            var checkedBy = validatorResponse.SelectSingleNode("m:checkedby", xmlNamespaceManager).InnerText;
            var doctype = validatorResponse.SelectSingleNode("m:doctype", xmlNamespaceManager).InnerText;
            var charset = validatorResponse.SelectSingleNode("m:charset", xmlNamespaceManager).InnerText;
            var validity = bool.Parse(validatorResponse.SelectSingleNode("m:validity", xmlNamespaceManager).InnerText);


            var errors = validatorResponse.SelectSingleNode("m:errors", xmlNamespaceManager);
            var errorCount = int.Parse(errors.SelectSingleNode("m:errorcount", xmlNamespaceManager).InnerText);

            var errorList = errors.SelectNodes("m:errorlist/m:error", xmlNamespaceManager);
            var parsedErrors = new List<IValidatorMessage>(errorCount);
            foreach (XmlNode error in errorList)
            {
                IValidatorMessage validationMessage = ParseMessage(xmlNamespaceManager, error);
                parsedErrors.Add(validationMessage);
            }

            var warnings = validatorResponse.SelectSingleNode("m:warnings", xmlNamespaceManager);
            var warningCount = int.Parse(warnings.SelectSingleNode("m:warningcount", xmlNamespaceManager).InnerText);

            var warningList = warnings.SelectNodes("m:warninglist/m:warning", xmlNamespaceManager);
            var parsedWarnings = new List<IValidatorMessage>(warningCount);
            foreach (XmlNode warning in warningList)
            {
                IValidatorMessage validationMessage = ParseMessage(xmlNamespaceManager, warning);
                parsedWarnings.Add(validationMessage);
            }

            return new CssValidatorReport(uri, checkedBy, doctype, charset, validity, parsedErrors, parsedWarnings);
        }

        #endregion

        /// <summary>
        /// Parses a warning or error
        /// </summary>
        /// <param name="xmlNamespaceManager"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        static IValidatorMessage ParseMessage(XmlNamespaceManager xmlNamespaceManager, XmlNode error)
        {
            var xmlLine = error.SelectSingleNode("m:line", xmlNamespaceManager);
            var line = xmlLine != null ? (int?)int.Parse(xmlLine.InnerText) : null;

            var xmlCol = error.SelectSingleNode("m:col", xmlNamespaceManager);
            var col = xmlCol != null ? (int?)int.Parse(xmlCol.InnerText) : null;

            var xmlMessage = error.SelectSingleNode("m:message", xmlNamespaceManager);
            var message = xmlMessage != null ? xmlMessage.InnerText : null;

            var xmlMessageId = error.SelectSingleNode("m:messageid", xmlNamespaceManager);
            var messageId = xmlMessageId != null ? xmlMessageId.InnerText : null;

            var xmlExplanation = error.SelectSingleNode("m:explanation", xmlNamespaceManager);
            var explanation = xmlExplanation != null ? xmlExplanation.InnerText : null;

            var xmlSource = error.SelectSingleNode("m:source", xmlNamespaceManager);
            var source = xmlSource != null ? xmlSource.InnerText : null;

            return new CssValidatorMessage(line, col, message, messageId, explanation, source);
        }
    }
}
