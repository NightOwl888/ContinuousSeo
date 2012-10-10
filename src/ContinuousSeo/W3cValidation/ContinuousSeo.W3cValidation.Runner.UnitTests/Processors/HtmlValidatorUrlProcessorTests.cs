namespace ContinuousSeo.W3cValidation.Runner.UnitTests.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Output;

    [TestFixture]
    public class HtmlValidatorUrlProcessorTests
    {
        private Mock<HtmlValidator> mValidator = null;
        private Mock<HtmlValidatorRunnerContext> mContext = null;
        private Mock<IUrlFileParser> mParser = null;
        private Mock<IUrlAggregator> mAggregator = null;
        private Mock<IOutputPathProvider> mOutputPathProvider = null;

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            mValidator = new Mock<HtmlValidator>();
            mContext = new Mock<HtmlValidatorRunnerContext>();
            mParser = new Mock<IUrlFileParser>();
            mAggregator  = new Mock<IUrlAggregator>();
            mOutputPathProvider = new Mock<IOutputPathProvider>();

            mValidator
                .Setup(x => x.IsDefaultValidatorAddress(It.IsAny<string>()))
                .Returns(false);

            mContext
                .Setup(x => x.ValidatorUrl)
                .Returns("http://www.whereever.com/validator");

            mAggregator
                .Setup(x => x.ProcessLine(It.IsAny<IUrlFileLineInfo>()))
                .Returns((IUrlFileLineInfo x) => new List<string>() { x.Url });

        }

        [TearDown]
        public void Dispose()
        {
            mValidator = null;
            mContext = null;
            mParser = null;
            mAggregator = null;
            mOutputPathProvider = null;
            mTotalValidatorValidateCalls = 0;
        }

        private void SetupValidValidatorValidateReturnStatusWith8Warnings()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
                .Returns(new HtmlValidatorResult("Valid", 0, 8, 1));
        }

        private void SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
                .Returns(new HtmlValidatorResult("Invalid", 4, 11, 1));
        }

        private void SetupValidatorValidateToThrowHttpException()
        {
            mValidator
                .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
                .Throws<System.Web.HttpException>();
        }

        private void SetupUrlFileParserParseFileToReturn4SingleUrls()
        {
            mParser
                .Setup(x => x.ParseFile(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(new List<IUrlFileLineInfo>()
                    {
                        new UrlFileLineInfo("http://www.google.com/", "single"),
                        new UrlFileLineInfo("http://www.google.com/test.aspx","single"),
                        new UrlFileLineInfo("http://www.google.com/test2.aspx", "single"),
                        new UrlFileLineInfo("http://www.google.com/test3.aspx", "single")
                    });
        }

        private int mTotalValidatorValidateCalls = 0;

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

        #region ProcessUrls Method

        [Test]
        public void ProcessUrls_TargetUrlFileWith4SingleUrls_Returns4Reports()
        {
            // arrange
            var targetUrlFiles = new List<string>() { @"C:\Testing\Test.txt" };

            SetupUrlFileParserParseFileToReturn4SingleUrls();
            SetupValidValidatorValidateReturnStatusWith8Warnings();
            mContext.Setup(x => x.TargetUrlFiles).Returns(targetUrlFiles);

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrls();

            // assert
            var actual = result.Count();
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrls_TargetUrlFileWith4SingleUrls_CallsValidatorValidate4Times()
        {
            // arrange
            var targetUrlFiles = new List<string>() { @"C:\Testing\Test.txt" };

            SetupUrlFileParserParseFileToReturn4SingleUrls();
            SetupValidValidatorValidateReturnStatusWith8Warnings();
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();
            mContext.Setup(x => x.TargetUrlFiles).Returns(targetUrlFiles);

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrls();

            // assert
            var actual = mTotalValidatorValidateCalls;
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrls_ContextTargetUrls2Provided_CallsValidatorValidate2Times()
        {
            // arrange
            var targetUrls = new List<string>() 
            {
                @"http://www.mydomain.com/url.html",
                @"http://www.mydomain2.com/url2.html"
            };

            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();
            mContext.Setup(x => x.TargetUrls).Returns(targetUrls);

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrls();

            // assert
            var actual = mTotalValidatorValidateCalls;
            var expected = 2;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrls_ContextTargetUrls2ProvidedWithReplacableTokens_ReplacesBothTokens()
        {
            // arrange
            var targetUrls = new List<string>() 
            {
                @"http://{0}/url.html",
                @"http://{1}/url2.html"
            };
            var urlReplacementArgs = new List<string>()
            {
                @"www.mydomain1.com",
                @"www.mydomain2.com"
            };

            mContext.Setup(x => x.TargetUrls).Returns(targetUrls);
            mContext.Setup(x => x.UrlReplacementArgs).Returns(urlReplacementArgs);
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrls();

            // assert
            var expectedUrl1 = @"http://www.mydomain1.com/url.html";
            var expectedUrl2 = @"http://www.mydomain2.com/url2.html";

            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), expectedUrl1, It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()), 
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<System.IO.Stream>(), It.IsAny<OutputFormat>(), expectedUrl1, It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), expectedUrl2, It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()),
                Times.AtMostOnce());
            mValidator
                .Verify(x => x.Validate(It.IsAny<System.IO.Stream>(), It.IsAny<OutputFormat>(), expectedUrl2, It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()),
                Times.AtMostOnce());

            var actual = mTotalValidatorValidateCalls;
            var expected = 2;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrls_NoTargetUrlsOrTargetFileProvided_CallsValidatorValidate0Times()
        {
            // arrange
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrls();

            // assert
            var actual = mTotalValidatorValidateCalls;
            var expected = 0;

            Assert.AreEqual(expected, actual);
        }

        public void ProcessUrls_ContextTargetSitemapFiles3Provided_CallsAggregatorProcessLine3Times()
        {
            // arrange
            mContext.Setup(x => x.TargetSitemapsFiles).Returns(new List<string>() {
                "http://mysite.com/sitemaps1.xml",
                "http://myothersite.com/sitemaps.sitemap",
                "http://www.whatever.com/sitemaps.file"
            });

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrls();

            // assert
            mAggregator
                .Verify(x => x.ProcessLine(It.IsAny<IUrlFileLineInfo>()), Times.Exactly(3));
        }

        [Test]
        public void ProcessUrls_ContextTargetSitemapFiles2ProvidedWithReplacableTokens_ReplacesBothTokens()
        {
            // arrange
            var targetSitemapsFiles = new List<string>() 
            {
                @"http://{0}/sitemap.xml",
                @"http://{1}/sitemap.xml"
            };
            var urlReplacementArgs = new List<string>()
            {
                @"www.mydomain1.com",
                @"www.mydomain2.com"
            };

            mContext.Setup(x => x.TargetSitemapsFiles).Returns(targetSitemapsFiles);
            mContext.Setup(x => x.UrlReplacementArgs).Returns(urlReplacementArgs);
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrls();

            // assert
            var expectedUrl1 = @"http://www.mydomain1.com/sitemap.xml";
            var expectedUrl2 = @"http://www.mydomain2.com/sitemap.xml";

            mAggregator
                .Verify(x => x.ProcessLine(It.Is<IUrlFileLineInfo>(p => p.Url == expectedUrl1)), 
                Times.Once());
            mAggregator
                .Verify(x => x.ProcessLine(It.Is<IUrlFileLineInfo>(p => p.Url == expectedUrl2)), 
                Times.Once());
        }

        #endregion

        #region ProcessUrl Method

        [Test]
        public void ProcessUrl_ValidUrl_ReturnsDomainName()
        {
            // arrange
            var url = "http://www.google.com/";
            SetupValidValidatorValidateReturnStatusWith8Warnings();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.DomainName;
            var expected = "www.google.com";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidUrlWithNonStandardPort_ReturnsDomainNameAndPort()
        {
            // arrange
            var url = "http://www.google.com:9090/test.aspx";
            SetupValidValidatorValidateReturnStatusWith8Warnings();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.DomainName;
            var expected = "www.google.com:9090";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_InvalidUrl_ReturnsValidFalse()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_InvalidUrl_ReturnsInvalidUrlExceptionMessage()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.ErrorMessage;
            var expected = "The url is not in a valid format.";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_InvalidUrl_ReturnsUrl()
        {
            // arrange
            var url = "http:/www.google.com/test.aspx";

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidUrl_ReturnsUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidValidatorValidateReturnStatusWith8Warnings();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_HttpExceptionThrown_ReturnsUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToThrowHttpException();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.Url;
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ContextOutputFormatHtml_CallsValidatorValidateWithHtmlFormat()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            mContext.Setup(x => x.OutputFormat).Returns("html");
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

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
        public void ProcessUrl_ContextOutputFormatXml_CallsValidatorValidateWithSoap12Format()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            mContext.Setup(x => x.OutputFormat).Returns("xml");
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

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
        public void ProcessUrl_ContextOutputFormatNull_CallsValidatorValidateWithHtmlFormat()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            // By default the mContext.Object.OutputFormat return value is null

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

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
        public void ProcessUrl_ContextValidatorUrlProvided_CallsValidatorValidateWithSameValidatorUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

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
        public void ProcessUrl_OutputPathProviderReturnedValue_CallsValidatorValidateWithSameOutputPath()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            var path = @"C:\TestDir\TestFile.html";

            mOutputPathProvider.Setup(x => x.GetOutputPath(url)).Returns(path);

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var expectedValue = path;
            mValidator
                .Verify(x => x.Validate(expectedValue, It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()), 
                Times.AtMostOnce());
        }

        [Test]
        public void ProcessUrl_ValidUrl_CallsValidatorValidateWithSameUrl()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

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
        public void ProcessUrl_ValidatorValidateReturnsValidStatus_ReportsValidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidValidatorValidateReturnStatusWith8Warnings();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.IsValid;
            var expected = true;
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidatorValidateReturnsInvalidStatus_ReportsInvalidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidatorValidateReturns8Warnings_Reports8Warnings()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidValidatorValidateReturnStatusWith8Warnings();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.Warnings;
            var expected = 8;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidatorValidateReturns4Errors_Reports4Errors()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.Errors;
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_HttpExceptionThrown_ReportsInvalidStatus()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";

            this.SetupValidatorValidateToThrowHttpException();

            HtmlValidatorUrlProcessor target = new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mParser.Object,
                mAggregator.Object,
                mOutputPathProvider.Object);

            // act
            var result = target.ProcessUrl(url);

            // assert
            var actual = result.IsValid;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
