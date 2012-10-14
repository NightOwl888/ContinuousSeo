// -----------------------------------------------------------------------
// <copyright file="ValidatorReportWriterFactory.cs" company="">
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
    using ContinuousSeo.W3cValidation.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ValidatorReportWriterFactory : IValidatorReportWriterFactory
    {
        private readonly IValidatorSoap12ResponseParser mParser;

        public ValidatorReportWriterFactory(IValidatorSoap12ResponseParser parser)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");

            mParser = parser;
        }

        public IValidatorReportTextWriter GetTextWriter(Stream stream, Encoding encoding)
        {
            return new ValidatorReportXmlTextWriter(stream, encoding, mParser);
        }

        public IValidatorReportTextWriter GetTextWriter(string filename, Encoding encoding)
        {
            return new ValidatorReportXmlTextWriter(filename, encoding, mParser);
        }
    }
}
