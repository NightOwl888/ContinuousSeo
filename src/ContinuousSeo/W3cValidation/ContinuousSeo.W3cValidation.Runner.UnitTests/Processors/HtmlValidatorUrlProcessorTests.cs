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
        private Mock<IValidatorWrapper> mValidator = null;
        private Mock<HtmlValidatorRunnerContext> mContext = null;
        private Mock<IUrlAggregator> mAggregator = null;
        //private Mock<IOutputPathProvider> mOutputPathProvider = null;
        private Mock<IFileNameGenerator> mFileNameGenerator = null;
        private TestResourceCopier mResourceCopier = null;
        private Mock<IValidatorReportWriterFactory> mReportWriterFactory = null;
        private Mock<IStreamFactory> mStreamFactory = null;

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            mValidator = new Mock<IValidatorWrapper>();
            mContext = new Mock<HtmlValidatorRunnerContext>();
            mAggregator  = new Mock<IUrlAggregator>();
            //mOutputPathProvider = new Mock<IOutputPathProvider>();
            mFileNameGenerator = new Mock<IFileNameGenerator>();
            mResourceCopier = new TestResourceCopier();
            mReportWriterFactory = new Mock<IValidatorReportWriterFactory>();
            mStreamFactory = new Mock<IStreamFactory>();
            

            mValidator
                .Setup(x => x.IsDefaultValidatorUrl())
                .Returns(false);

            mContext
                .Setup(x => x.ValidatorUrl)
                .Returns("http://www.whereever.com/validator");

            //mAggregator
            //    .Setup(x => x.ProcessLine(It.IsAny<IUrlFileLineInfo>()))
            //    .Returns((IUrlFileLineInfo x) => new List<string>() { x.Url });

            //mOutputPathProvider
            //    .Setup(x => x.GetOutputPath(It.IsAny<string>()))
            //    .Returns(@"F:\FileToWrite.html");

            mReportWriterFactory
                .Setup(x => x.GetTextWriter(It.IsAny<Stream>(), It.IsAny<Encoding>()))
                .Returns((Stream x, Encoding y) => (IValidatorReportTextWriter)new Mock<IValidatorReportTextWriter>().Object);

            mStreamFactory
                .Setup(x => x.GetMemoryStream())
                .Returns(new MemoryStream());

            mStreamFactory
                .Setup(x => x.GetFileStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
                .Returns(new MemoryStream());
        }

        [TearDown]
        public void Dispose()
        {
            mValidator = null;
            mContext = null;
            mAggregator = null;
            //mOutputPathProvider = null;
            mFileNameGenerator = null;
            mTotalValidatorValidateUrlCalls = 0;
        }

        private HtmlValidatorUrlProcessor NewHtmlValidatorUrlProcessorInstance()
        {
            return new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mAggregator.Object,
                //mOutputPathProvider.Object,
                mFileNameGenerator.Object,
                mResourceCopier,
                mReportWriterFactory.Object,
                mStreamFactory.Object);
        }

        //private void SetupValidValidatorValidateReturnStatusWith8Warnings()
        //{
        //    //mValidator
        //    //    .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
        //    //    .Returns(new HtmlValidatorResult("Valid", 0, 8, 1));

        //    var result = new Mock<IValidatorReportItem>().SetupAllProperties().Object;

        //    result.IsValid = true;
        //    result.Errors = 0;
        //    result.Warnings = 8;

        //    mValidator
        //        .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
        //        .Returns(result);

        //}

        //private void SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings()
        //{
        //    //mValidator
        //    //    .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
        //    //    .Returns(new HtmlValidatorResult("Invalid", 4, 11, 1));

        //    var result = new Mock<IValidatorReportItem>().SetupAllProperties().Object;

        //    result.IsValid = false;
        //    result.Errors = 4;
        //    result.Warnings = 11;

        //    mValidator
        //        .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
        //        .Returns(result);
        //}

        //private void SetupValidatorValidateToThrowHttpException()
        //{
        //    //mValidator
        //    //    .Setup(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()))
        //    //    .Throws<System.Web.HttpException>();


        //    mValidator
        //        .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
        //        .Throws<System.Web.HttpException>();
        //}



        private int mTotalValidatorValidateUrlCalls = 0;

        private void SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads()
        {
            //mValidator
            //    .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()))
            //    .Callback(() => mTotalValidatorValidateCalls++);
            //mValidator
            //    .Setup(x => x.Validate(It.IsAny<System.IO.Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), It.IsAny<InputFormat>(), It.IsAny<IHtmlValidatorSettings>(), It.IsAny<string>()))
            //    .Callback(() => mTotalValidatorValidateCalls++);

            mValidator
                .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
                .Callback(() => mTotalValidatorValidateUrlCalls++);
        }

        #endregion

        #region ProcessUrls Method

        //[Test]
        //public void ProcessUrls_TargetUrlFileWith4SingleUrls_Returns4Reports()
        //{
        //    // arrange
        //    var targetUrlFiles = new List<string>() { @"C:\Testing\Test.txt" };

        //    SetupUrlFileParserParseFileToReturn4SingleUrls();
        //    SetupValidValidatorValidateReturnStatusWith8Warnings();
        //    mContext.Setup(x => x.TargetUrlFiles).Returns(targetUrlFiles);

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    var result = target.ProcessUrls();

        //    // assert
        //    var actual = result.Count();
        //    var expected = 4;

        //    Assert.AreEqual(expected, actual);
        //}

       
        [Test]
        public void ProcessUrls_NoTargetUrlsOrTargetFileProvided_CallsValidatorValidate0Times()
        {
            // arrange
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();
            //mContext
            //    .Setup(x => x.OutputPath)
            //    .Returns(@"C:\Somewhere\Test.html");

            HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

            // act
            target.ProcessUrls();

            // assert
            var actual = mTotalValidatorValidateUrlCalls;
            var expected = 0;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        //#region ProcessUrl Method

        //[Test]
        //public void ProcessUrl_ValidUrl_ReturnsDomainName()
        //{
        //    // arrange
        //    var url = "http://www.google.com/";
        //    SetupValidValidatorValidateReturnStatusWith8Warnings();
            

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }


        //    // assert
        //    var actual = result.DomainName;
        //    var expected = "www.google.com";

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ValidUrlWithNonStandardPort_ReturnsDomainNameAndPort()
        //{
        //    // arrange
        //    var url = "http://www.google.com:9090/test.aspx";
        //    SetupValidValidatorValidateReturnStatusWith8Warnings();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.DomainName;
        //    var expected = "www.google.com:9090";

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_InvalidUrl_ReturnsValidFalse()
        //{
        //    // arrange
        //    var url = "http:/www.google.com/test.aspx";

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.IsValid;
        //    var expected = false;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_InvalidUrl_ReturnsInvalidUrlExceptionMessage()
        //{
        //    // arrange
        //    var url = "http:/www.google.com/test.aspx";

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.ErrorMessage;
        //    var expected = "The url is not in a valid format.";

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_InvalidUrl_ReturnsUrl()
        //{
        //    // arrange
        //    var url = "http:/www.google.com/test.aspx";

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.Url;
        //    var expected = url;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ValidUrl_ReturnsUrl()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    SetupValidValidatorValidateReturnStatusWith8Warnings();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.Url;
        //    var expected = url;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_HttpExceptionThrown_ReturnsUrl()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    SetupValidatorValidateToThrowHttpException();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.Url;
        //    var expected = url;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ContextOutputFormatHtml_CallsValidatorValidateWithHtmlFormat()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";

        //    mContext.Setup(x => x.OutputFormat).Returns("html");
        //    SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var expectedValue = OutputFormat.Html;
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()), 
        //        Times.AtMostOnce());
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
        //        Times.AtMostOnce());

        //    var expected = 1;
        //    var actual = mTotalValidatorValidateCalls;
        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ContextOutputFormatXml_CallsValidatorValidateWithSoap12Format()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";

        //    mContext.Setup(x => x.OutputFormat).Returns("xml");
        //    SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var expectedValue = OutputFormat.Soap12;
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()), 
        //        Times.AtMostOnce());
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
        //        Times.AtMostOnce());

        //    var expected = 1;
        //    var actual = mTotalValidatorValidateCalls;
        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ContextOutputFormatNull_CallsValidatorValidateWithHtmlFormat()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

        //    // By default the mContext.Object.OutputFormat return value is null

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var expectedValue = OutputFormat.Html;
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<string>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()), 
        //        Times.AtMostOnce());
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<Stream>(), expectedValue, It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()),
        //        Times.AtMostOnce());

        //    var expected = 1;
        //    var actual = mTotalValidatorValidateCalls;
        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ContextValidatorUrlProvided_CallsValidatorValidateWithSameValidatorUrl()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var expectedValue = "http://www.whereever.com/validator";
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, expectedValue), 
        //        Times.AtMostOnce());
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, expectedValue),
        //        Times.AtMostOnce());

        //    var expected = 1;
        //    var actual = mTotalValidatorValidateCalls;
        //    Assert.AreEqual(expected, actual);
        //}

        ////[Test]
        ////public void ProcessUrl_OutputPathProviderReturnedValue_CallsValidatorValidateWithSameOutputPath()
        ////{
        ////    // arrange
        ////    var url = "http://www.google.com/test.aspx";
        ////    var path = @"C:\TestDir\TestFile.html";

        ////    mOutputPathProvider.Setup(x => x.GetOutputPath(url)).Returns(path);

        ////    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        ////    // act
        ////    IValidatorReportItem result;
        ////    using (var output = new MemoryStream())
        ////    {
        ////        result = target.ProcessUrl(url, output, OutputFormat.Html);
        ////    }

        ////    // assert
        ////    var expectedValue = path;
        ////    mValidator
        ////        .Verify(x => x.Validate(expectedValue, It.IsAny<OutputFormat>(), It.IsAny<string>(), InputFormat.Uri, mContext.Object, It.IsAny<string>()), 
        ////        Times.AtMostOnce());
        ////}

        //[Test]
        //public void ProcessUrl_ValidUrl_CallsValidatorValidateWithSameUrl()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var expectedValue = url;
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<OutputFormat>(), expectedValue, InputFormat.Uri, mContext.Object, It.IsAny<string>()), 
        //        Times.AtMostOnce());
        //    mValidator
        //        .Verify(x => x.Validate(It.IsAny<Stream>(), It.IsAny<OutputFormat>(), expectedValue, InputFormat.Uri, mContext.Object, It.IsAny<string>()),
        //        Times.AtMostOnce());

        //    var expected = 1;
        //    var actual = mTotalValidatorValidateCalls;
        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ValidatorValidateReturnsValidStatus_ReportsValidStatus()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";

        //    this.SetupValidValidatorValidateReturnStatusWith8Warnings();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.IsValid;
        //    var expected = true;
            
        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ValidatorValidateReturnsInvalidStatus_ReportsInvalidStatus()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";

        //    this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.IsValid;
        //    var expected = false;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ValidatorValidateReturns8Warnings_Reports8Warnings()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";

        //    this.SetupValidValidatorValidateReturnStatusWith8Warnings();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.Warnings;
        //    var expected = 8;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_ValidatorValidateReturns4Errors_Reports4Errors()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";

        //    this.SetupInalidValidatorValidateReturnStatusWith4ErrorsAnd11Warnings();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.Errors;
        //    var expected = 4;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessUrl_HttpExceptionThrown_ReportsInvalidStatus()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";

        //    this.SetupValidatorValidateToThrowHttpException();

        //    HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

        //    // act
        //    IValidatorReportItem result;
        //    using (var output = new MemoryStream())
        //    {
        //        result = target.ProcessUrl(url, output, OutputFormat.Html);
        //    }

        //    // assert
        //    var actual = result.IsValid;
        //    var expected = false;

        //    Assert.AreEqual(expected, actual);
        //}

        //#endregion

        #region Private Objects

        private class TestResourceCopier : ResourceCopier
        {
            protected override Dictionary<string, string> GetResourceMap()
            {
                return new Dictionary<string, string>();
            }
        }

        #endregion

    }
}
