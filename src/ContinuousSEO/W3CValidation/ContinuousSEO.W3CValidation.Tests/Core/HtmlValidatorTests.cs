using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using ContinuousSEO.W3CValidation.Core;
using ContinuousSEO.W3CValidation.Core.Html;

namespace ContinuousSEO.W3CValidation.Tests.Core
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
