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

namespace ContinuousSeo.W3cValidation.Runner.UnitTests.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.Core.Net;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Validators;

    [TestFixture]
    public class HtmlValidatorWrapperTests
    {
        private Mock<HtmlValidator> mValidator = null;
        private Mock<IHtmlValidatorRunnerContext> mContext = null;
        private Mock<IHttpClient> mHttpClient = null;
        private int mTotalValidatorValidateCalls = 0;

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            mValidator = new Mock<HtmlValidator>();
            mContext = new Mock<IHtmlValidatorRunnerContext>();
            mHttpClient = new Mock<IHttpClient>();

            mValidator
                .Setup(x => x.IsDefaultValidatorAddress(It.IsAny<string>()))
                .Returns(false);

            mContext
                .Setup(x => x.ValidatorUrl)
                .Returns("http://www.whereever.com/validator");

            mContext
                .Setup(x => x.Announcer)
                .Returns(new Mock<IAnnouncer>().Object);
        }

        [TearDown]
        public void Dispose()
        {
            mValidator = null;
            mContext = null;
            mHttpClient = null;
            mTotalValidatorValidateCalls = 0;
        }

        private HtmlValidatorWrapper NewHtmlValidatorWrapperInstance()
        {
            return new HtmlValidatorWrapper(mValidator.Object, mContext.Object, mHttpClient.Object);
        }

        private void SetupValidValidatorValidateReturnStatusWith8Warnings()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), mContext.Object, It.IsAny<string>()))
                .Returns(new HtmlValidatorResult("Valid", 0, 8, 1));
        }

        private void SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), mContext.Object, It.IsAny<string>()))
                .Returns(new HtmlValidatorResult("Invalid", 4, 11, 1));
        }

        private void SetupValidatorValidateToThrowHttpException()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), mContext.Object, It.IsAny<string>()))
                .Throws<System.Web.HttpException>();
        }



        

        private void SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()))
                .Callback(() => mTotalValidatorValidateCalls++);
            mValidator
                .Setup(x => x.Validate(It.IsAny<System.IO.Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()))
                .Callback(() => mTotalValidatorValidateCalls++);
        }

        #endregion

        #region ValidateUrl Method

        [Test]
        public void ValidateUrl_ValidUrl_ShouldReturnDomainName()
        {
            // arrange
            var url = "http://www.google.com/";
            SetupValidValidatorValidateReturnStatusWith8Warnings();


            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }


            // assert
            var actual = result.DomainName;
            var expected = "www.google.com";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidUrlWithNonStandardPort_ShouldReturnDomainNameAndPort()
        {
            // arrange
            var url = "http://www.google.com:9090/test.aspx";
            SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.DomainName;
            var expected = "www.google.com:9090";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_InvalidUrl_ShouldReturnValidFalse()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_InvalidUrl_ShouldReturnInvalidUrlExceptionMessage()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.ErrorMessage;
            var expected = "The url is not in a valid format.";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_InvalidUrl_ShouldReturnUrl()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidUrl_ShouldReturnUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_HttpExceptionThrown_ShouldReturnUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToThrowHttpException();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextOutputFormatHtml_ShouldCallValidatorValidateWithHtmlFormat()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            mContext.Setup(x => x.OutputFormat).Returns("html");
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = OutputFormat.Html;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextOutputFormatXml_ShouldCallValidatorValidateWithSoap12Format()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            mContext.Setup(x => x.OutputFormat).Returns("xml");
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = OutputFormat.Soap12;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextOutputFormatNull_ShouldCallValidatorValidateWithHtmlFormat()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            // By default the mContext.Object.OutputFormat return value is null

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = OutputFormat.Html;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextValidatorUrlProvided_ShouldCallValidatorValidateWithSameValidatorUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = "http://www.whereever.com/validator";
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, expectedValue),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, expectedValue),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidUrl_ShouldCallValidatorValidateWithSameUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var expectedValue = url;
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), expectedValue, InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), expectedValue, InputFormat.Uri, mContext.Object, It.IsAny<string>()),
                Times.AtMostOnce());

            var expected = 1;
            var actual = mTotalValidatorValidateCalls;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturnsValidStatus_ShouldReportValidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturnsInvalidStatus_ShouldReportInvalidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturns8Warnings_ShouldReport8Warnings()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidValidatorValidateReturnStatusWith8Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Warnings;
            var expected = 8;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ValidatorValidateReturns4Errors_ShouldReport4Errors()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.Errors;
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_HttpExceptionThrown_ShouldReportInvalidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidatorValidateToThrowHttpException();

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ValidateUrl_ContextDirectInputModeTrue_ShouldCallHttpClientGetResponseText()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            mContext
                .Setup(x => x.DirectInputMode)
                .Returns(true);

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            mHttpClient
                .Verify(x => x.GetResponseText(url), 
                Times.Once());
        }

        [Test]
        public void ValidateUrl_ContextDirectInputModeFalse_ShouldNotCallHttpClientGetResponseText()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            mContext
                .Setup(x => x.DirectInputMode)
                .Returns(false);

            var target = NewHtmlValidatorWrapperInstance();

            // act
            IValidatorReportItem result;
            using (var output = new MemoryStream())
            {
                result = target.ValidateUrl(url, output, OutputFormat.Html);
            }

            // assert
            mHttpClient
                .Verify(x => x.GetResponseText(url),
                Times.Never());
        }

        #endregion
    }
}
