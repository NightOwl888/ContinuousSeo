using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
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

        private Mock<IHtmlValidatorRunnerContext> mContext;
        private Mock<IUrlAggregator> mUrlAggregator;
        private Mock<IUrlProcessorFactory> mProcessorFactory;

        [SetUp]
        public void Init()
        {
            mContext = new Mock<IHtmlValidatorRunnerContext>();
            mUrlAggregator = new Mock<IUrlAggregator>();
            mProcessorFactory = new Mock<IUrlProcessorFactory>();

            mContext
                .Setup(x => x.TotalTimeStopwatch)
                .Returns(new Mock<Stopwatch>().Object);
        }

        [TearDown]
        public void Dispose()
        {
            mContext = null;
            mUrlAggregator = null;
            mProcessorFactory = null;
        }

        private HtmlValidatorRunner NewHtmlValidationRunnerInstance()
        {
            return new HtmlValidatorRunner(
                mContext.Object, 
                mUrlAggregator.Object,
                mProcessorFactory.Object);
        }

        #endregion

        #region Execute Method

        [Test]
        public void Execute_Called_ShouldCallProcessorFactoryGetUrlProcessor1Time()
        {
            // arrange
            var target = NewHtmlValidationRunnerInstance();

            // act
            target.Execute();

            // assert
            //mProcessor
            //    .Verify(x => x.ProcessUrls(It.IsAny<IEnumerable<string>>()), 
            //    Times.Once());

            mProcessorFactory
                .Verify(x => x.GetUrlProcessor(It.IsAny<string>()), 
                Times.Once());
        }

        #endregion

    }
}
