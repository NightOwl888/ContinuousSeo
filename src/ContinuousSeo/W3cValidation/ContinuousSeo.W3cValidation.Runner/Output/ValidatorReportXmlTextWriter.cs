// -----------------------------------------------------------------------
// <copyright file="ValidationReportXmlTextWriter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ContinuousSeo.W3cValidation.Runner.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.IO;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Runner.Processors;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidatorReportXmlTextWriter : MarshalByRefObject, IDisposable, IValidatorReportTextWriter
    {
        private XmlTextWriter mWriter;
        private IValidatorSoap12ResponseParser mParser;

        public ValidatorReportXmlTextWriter(Stream stream, Encoding encoding, IValidatorSoap12ResponseParser parser)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (parser == null)
                throw new ArgumentNullException("parser");

            mWriter = new XmlTextWriter(stream, encoding);
            mWriter.Formatting = Formatting.Indented;
            mParser = parser;
        }

        public ValidatorReportXmlTextWriter(string filename, Encoding encoding, IValidatorSoap12ResponseParser parser)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");
            if (encoding == null)
                throw new ArgumentNullException("encoding");
            if (parser == null)
                throw new ArgumentNullException("parser");

            mWriter = new XmlTextWriter(filename, encoding);
            mWriter.Formatting = Formatting.Indented;
            mParser = parser;
        }

        public void WriteStartDocument()
        {
            mWriter.WriteStartDocument(true);
            mWriter.WriteStartElement("w3cValidatorResults");
        }

        public void WriteEndDocument()
        {
            mWriter.WriteEndElement();
            mWriter.WriteEndDocument();
        }

        public void WriteUrlElement(IValidatorReportItem urlReport)
        {
            WriteStartUrlElement(urlReport);
            WriteEndUrlElement();
        }

        public void WriteUrlElement(IValidatorReportItem urlReport, Stream response)
        {
            WriteStartUrlElement(urlReport);
            WriteUrlElementValue(response);
            WriteEndUrlElement();
        }

        public void Flush()
        {
            mWriter.Flush();
        }

        #region Private Members

        private void WriteStartUrlElement(IValidatorReportItem urlReport)
        {
            mWriter.WriteStartElement("validationResult");

            mWriter.WriteStartAttribute("url");
            mWriter.WriteValue(urlReport.Url);
            mWriter.WriteEndAttribute();

            mWriter.WriteStartAttribute("domainName");
            mWriter.WriteValue(urlReport.DomainName);
            mWriter.WriteEndAttribute();

            mWriter.WriteStartAttribute("isValid");
            mWriter.WriteValue(urlReport.IsValid);
            mWriter.WriteEndAttribute();

            mWriter.WriteStartAttribute("errors");
            mWriter.WriteValue(urlReport.Errors);
            mWriter.WriteEndAttribute();

            mWriter.WriteStartAttribute("warnings");
            mWriter.WriteValue(urlReport.Warnings);
            mWriter.WriteEndAttribute();

            if (!string.IsNullOrEmpty(urlReport.ErrorMessage))
            {
                mWriter.WriteStartAttribute("errorMessage");
                mWriter.WriteValue(urlReport.ErrorMessage);
                mWriter.WriteEndAttribute();
            }

            if (!string.IsNullOrEmpty(urlReport.FileName))
            {
                mWriter.WriteStartAttribute("fileName");
                mWriter.WriteValue(urlReport.FileName);
                mWriter.WriteEndAttribute();
            }
        }

        private void WriteEndUrlElement()
        {
            mWriter.WriteEndElement();
        }

        private void WriteUrlElementValue(Stream response)
        {
            IValidatorReport report = mParser.ParseResponse(response);

            mWriter.WriteStartElement("errors");
            foreach (var error in report.Errors)
            {
                WriteErrorElement(error);
            }
            mWriter.WriteEndElement();

            mWriter.WriteStartElement("warnings");
            foreach (var warning in report.Warnings)
            {
                WriteWarningElement(warning);
            }
            mWriter.WriteEndElement();
         
        }

        private void WriteErrorElement(IValidatorMessage message)
        {
            mWriter.WriteStartElement("error");
            WriteMessageElements(message);
            mWriter.WriteEndElement();
        }

        private void WriteWarningElement(IValidatorMessage message)
        {
            mWriter.WriteStartElement("warning");
            WriteMessageElements(message);
            mWriter.WriteEndElement();
        }

        private void WriteMessageElements(IValidatorMessage message)
        {
            if (message.Line != null)
            {
                mWriter.WriteStartElement("line");
                mWriter.WriteValue(message.Line);
                mWriter.WriteEndElement();
            }

            if (message.Column != null)
            {
                mWriter.WriteStartElement("column");
                mWriter.WriteValue(message.Line);
                mWriter.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(message.Message))
            {
                mWriter.WriteStartElement("message");
                mWriter.WriteCData(message.Message);
                mWriter.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(message.MessageId))
            {
                mWriter.WriteStartElement("messageId");
                mWriter.WriteValue(message.MessageId);
                mWriter.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(message.Explanation))
            {
                mWriter.WriteStartElement("explanation");
                mWriter.WriteCData(message.Explanation);
                mWriter.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(message.Source))
            {
                mWriter.WriteStartElement("source");
                mWriter.WriteCData(message.Source);
                mWriter.WriteEndElement();
            }
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            mWriter.Close();
        }

        #endregion
    }
}
