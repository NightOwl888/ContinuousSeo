using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using ContinuousSeo.W3cValidation.Runner.Initialization;
using ContinuousSeo.W3cValidation.Runner.Processors;

namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    [TestFixture]
    public class HtmlValidationRunnerTests
    {
        #region SetUp / TearDown

        private Mock<HtmlValidatorRunnerContext> mContext;
        private Mock<IUrlAggregator> mUrlAggregator;
        private Mock<IUrlProcessor> mProcessor;

        [SetUp]
        public void Init()
        {
            mContext = new Mock<HtmlValidatorRunnerContext>();
            mUrlAggregator = new Mock<IUrlAggregator>();
            mProcessor = new Mock<IUrlProcessor>();
        }

        [TearDown]
        public void Dispose()
        {
            mContext = null;
            mUrlAggregator = null;
            mProcessor = null;
        }

        private HtmlValidatorRunner NewHtmlValidationRunnerInstance()
        {
            return new HtmlValidatorRunner(
                mContext.Object, 
                mUrlAggregator.Object, 
                mProcessor.Object);
        }

        #endregion

        #region Execute Method

        [Test]
        public void Execute_Called_ShouldCallProcessorProcessUrls1Time()
        {
            // arrange
            var target = NewHtmlValidationRunnerInstance();

            // act
            target.Execute();

            // assert
            mProcessor
                .Verify(x => x.ProcessUrls(It.IsAny<IEnumerable<string>>()), 
                Times.Once());

        }

        #endregion
    }
}
