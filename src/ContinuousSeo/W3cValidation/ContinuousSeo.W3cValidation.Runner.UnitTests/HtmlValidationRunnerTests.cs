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
        private Mock<IUrlProcessor> mProcessor;

        [SetUp]
        public void Init()
        {
            mContext = new Mock<HtmlValidatorRunnerContext>();
            mProcessor = new Mock<IUrlProcessor>();
        }

        [TearDown]
        public void Dispose()
        {
            mContext = null;
            mProcessor = null;
        }

        private HtmlValidatorRunner NewHtmlValidationRunnerInstance()
        {
            return new HtmlValidatorRunner(mContext.Object, mProcessor.Object);
        }

        #endregion

        #region Execute Method

        [Test]
        public void Execute_Called_CallsProcessorProcessUrls1Time()
        {
            // arrange
            var target = NewHtmlValidationRunnerInstance();

            // act
            target.Execute();

            // assert
            mProcessor
                .Verify(x => x.ProcessUrls(), 
                Times.Once());

        }

        #endregion
    }
}
