namespace ContinuousSeo.W3cValidation.Runner.UnitTests.UrlProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Core;
    using ContinuousSeo.W3cValidation.Core.Html;
    using ContinuousSeo.W3cValidation.Runner.UrlProcessors;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Output;
    using ContinuousSeo.W3cValidation.Runner.Validators;

    [TestFixture]
    public class HtmlOutputUrlProcessorTests
    {
        private Mock<IRunnerContext> mContext = null;
        private Mock<IValidatorWrapper> mValidator = null;
        private Mock<IValidatorReportWriterFactory> mReportWriterFactory = null;
        private Mock<IFileNameGenerator> mFileNameGenerator = null;
        private MockResourceCopier mResourceCopier = null;
        private Mock<IStreamFactory> mStreamFactory = null;
        private Mock<IHtmlIndexFileWriter> mHtmlIndexFileWriter = null;


        //protected IFileNameGenerator mFileNameGenerator; // HTML only
        //protected ResourceCopier mResourceCopier; // HTML only
        //protected IStreamFactory mStreamFactory;
        //protected IHtmlIndexFileWriter mHtmlIndexFileWriter;

        //protected IRunnerContext mContext;
        //protected IValidatorWrapper mValidator;
        //protected IValidatorReportWriterFactory mReportWriterFactory;

        #region SetUp / TearDown

        [SetUp]
        public void Init()
        {
            mContext = new Mock<IRunnerContext>();
            mValidator = new Mock<IValidatorWrapper>();
            mReportWriterFactory = new Mock<IValidatorReportWriterFactory>();
            mFileNameGenerator = new Mock<IFileNameGenerator>();
            mResourceCopier = new MockResourceCopier();
            mStreamFactory = new Mock<IStreamFactory>();
            mHtmlIndexFileWriter = new Mock<IHtmlIndexFileWriter>();

            //mContext
            //    .Setup(x => x.ValidatorUrl)
            //    .Returns("http://www.whereever.com/validator");
            mContext
                .Setup(x => x.Announcer)
                .Returns(new Mock<IAnnouncer>().Object);
            mContext
                .Setup(x => x.TotalTimeStopwatch)
                .Returns(new Mock<Stopwatch>().Object);

            mValidator
                .Setup(x => x.IsDefaultValidatorUrl())
                .Returns(false);

            mValidator
                .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
                .Returns(new Mock<IValidatorReportItem>().Object);
            

            mReportWriterFactory
                .Setup(x => x.GetTextWriter(It.IsAny<Stream>(), It.IsAny<Encoding>()))
                .Returns((Stream x, Encoding y) => (IValidatorReportTextWriter)new Mock<IValidatorReportTextWriter>().Object);

            mFileNameGenerator
                .Setup(x => x.GenerateFileName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("hello.html");

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
            mContext = null;
            mValidator = null;
            mReportWriterFactory = null;
            mFileNameGenerator = null;
            mResourceCopier = null;
            mStreamFactory = null;
            mHtmlIndexFileWriter = null;
            mTotalValidatorValidateUrlCalls = 0;
        }

        private MockHtmlOutputUrlProcessor NewHtmlOutputUrlProcessorInstance()
        {
            return new MockHtmlOutputUrlProcessor(
                mContext.Object,
                mValidator.Object,
                mReportWriterFactory.Object,
                mFileNameGenerator.Object,
                mResourceCopier,
                mStreamFactory.Object,
                mHtmlIndexFileWriter.Object);
        }

        private int mTotalValidatorValidateUrlCalls = 0;

        private void SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads()
        {
            mValidator
                .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
                .Callback(() => mTotalValidatorValidateUrlCalls++)
                .Returns(new Mock<IValidatorReportItem>().Object);
        }

        #endregion

        #region Process Method

        [Test]
        public void Process_NoUrlsProvided_ShouldCallValidatorValidate0Times()
        {
            // arrange
            var urls = new List<string>() { };
            var outputPath = @"F:\Whereever\";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlOutputUrlProcessorInstance();

            // act
            target.Process(urls, outputPath);

            // assert
            var actual = mTotalValidatorValidateUrlCalls;
            var expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Process_3UrlsProvided_ShouldCallValidatorValidate3Times()
        {
            // arrange
            var urls = new List<string>() { "http://www.google.com/", "http://www.yahoo.com/", "http://www.bing.com/" };
            var outputPath = @"F:\Whereever\";
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            var target = NewHtmlOutputUrlProcessorInstance();

            // act
            target.Process(urls, outputPath);

            // assert
            var actual = mTotalValidatorValidateUrlCalls;
            var expected = 3;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region FixOutputPath Method

        [Test]
        public void FixOutputPath_ProvidedPathWithFileName_ShouldReturnPathWithoutFileName()
        {
            // arrange
            var urls = new List<string>() { };
            var outputPath = @"F:\Whereever\test.txt";

            var target = NewHtmlOutputUrlProcessorInstance();

            // act
            var result = target.FixOutputPath(outputPath);

            // assert
            var actual = result;
            var expected = @"F:\Whereever";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FixOutputPath_ProvidedPathWithoutFileName_ShouldReturnPathWithoutFileName()
        {
            // arrange
            var urls = new List<string>() { };
            var outputPath = @"F:\Whereever\";

            var target = NewHtmlOutputUrlProcessorInstance();

            // act
            var result = target.FixOutputPath(outputPath);

            // assert
            var actual = result;
            var expected = @"F:\Whereever";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Private Objects

        private class MockResourceCopier : ResourceCopier
        {
            protected override Dictionary<string, string> GetResourceMap()
            {
                return new Dictionary<string, string>();
            }
        }

        private class MockHtmlOutputUrlProcessor : HtmlOutputUrlProcessor
        {
            public MockHtmlOutputUrlProcessor(
                IRunnerContext context,
                IValidatorWrapper validator,
                IValidatorReportWriterFactory reportWriterFactory,
                IFileNameGenerator fileNameGenerator,
                ResourceCopier resourceCopier,
                IStreamFactory streamFactory,
                IHtmlIndexFileWriter htmlIndexFileWriter)
            {
                this.mContext = context;
                this.mValidator = validator;
                this.mReportWriterFactory = reportWriterFactory;
                this.mFileNameGenerator = fileNameGenerator;
                this.mResourceCopier = resourceCopier;
                this.mStreamFactory = streamFactory;
                this.mHtmlIndexFileWriter = htmlIndexFileWriter;
            }

            public override ValidationResult Process(IEnumerable<string> urls, string outputPath)
            {
                return base.Process(urls, outputPath);
            }

            new public string FixOutputPath(string outputPath)
            {
                return base.FixOutputPath(outputPath);
            }

            new public ValidationResult WriteXmlReport(IEnumerable<string> urls, Stream outputXmlReport, string outputPath)
            {
                return base.WriteXmlReport(urls, outputXmlReport, outputPath);
            }

            new public ValidationResult ValidateUrls(IEnumerable<string> urls, IValidatorReportTextWriter writer, string outputPath)
            {
                return base.ValidateUrls(urls, writer, outputPath);
            }

            new public Runner.Validators.IValidatorReportItem ValidateUrl(string url, IValidatorReportTextWriter writer, Stream outputStream)
            {
                return base.ValidateUrl(url, writer, outputStream);
            }
        }

        #endregion

    }
}
