using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using ContinuousSeo.W3cValidation.Core;
using ContinuousSeo.W3cValidation.Core.Html;

namespace ContinuousSeo.W3cValidation.Tests.Core
{
    [TestFixture]
    public class HtmlValidatorTests
    {
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        [Test]
        public void ShouldGetValidationResult()
        {
            HtmlValidator validator = new HtmlValidator();

            HtmlValidatorSettings settings = new HtmlValidatorSettings();

            FileStream output = new FileStream(@"F:\TestW3C\ResponseNew.html", FileMode.Create);

            HtmlValidatorResult result = validator.Validate(output, OutputFormat.Html, "http://www.shuttercontractor.com/", InputFormat.Uri, settings);

            NUnit.Framework.Assert.IsNotNullOrEmpty(result.Status);
        }

        #endregion
    }
}
