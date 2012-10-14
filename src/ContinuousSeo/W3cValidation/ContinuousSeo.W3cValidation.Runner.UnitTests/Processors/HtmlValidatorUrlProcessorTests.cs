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
            mFileNameGenerator = null;
            mTotalValidatorValidateUrlCalls = 0;
        }

        private HtmlValidatorUrlProcessor NewHtmlValidatorUrlProcessorInstance()
        {
            return new HtmlValidatorUrlProcessor(
                mValidator.Object,
                mContext.Object,
                mAggregator.Object,
                mFileNameGenerator.Object,
                mResourceCopier,
                mReportWriterFactory.Object,
                mStreamFactory.Object);
        }

        private int mTotalValidatorValidateUrlCalls = 0;

        private void SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads()
        {
            mValidator
                .Setup(x => x.ValidateUrl(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<OutputFormat>()))
                .Callback(() => mTotalValidatorValidateUrlCalls++);
        }

        #endregion

        #region ProcessUrls Method

        [Test]
        public void ProcessUrls_NoTargetUrlsOrTargetFileProvided_CallsValidatorValidate0Times()
        {
            // arrange
            SetupValidatorValidateToTrackNumberOfCallsToAllValidOverloads();

            HtmlValidatorUrlProcessor target = NewHtmlValidatorUrlProcessorInstance();

            // act
            target.ProcessUrls();

            // assert
            var actual = mTotalValidatorValidateUrlCalls;
            var expected = 0;

            Assert.AreEqual(expected, actual);
        }

        #endregion

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
