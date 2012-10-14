// -----------------------------------------------------------------------
// <copyright file="HtmlValidatorSoap12ResponseParser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Core.Html
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Xml;
    using ContinuousSeo.W3cValidation.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HtmlValidatorSoap12ResponseParser : IValidatorSoap12ResponseParser
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

            return new HtmlValidatorReport(uri, checkedBy, doctype, charset, validity, parsedErrors, parsedWarnings);
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

            return new HtmlValidatorMessage(line, col, message, messageId, explanation, source);
        }
    }
}
