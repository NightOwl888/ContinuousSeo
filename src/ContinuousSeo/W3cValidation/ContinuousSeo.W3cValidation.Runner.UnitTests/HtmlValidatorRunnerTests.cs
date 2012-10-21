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

namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Validators;
    using ContinuousSeo.W3cValidation.Runner.UrlAggregators;
    using ContinuousSeo.W3cValidation.Runner.UrlProcessors;

    [TestFixture]
    public class HtmlValidatorRunnerTests
    {
        #region SetUp / TearDown

        private Mock<IValidatorRunnerContext> mContext;
        private Mock<IUrlAggregator> mUrlAggregator;
        private Mock<IUrlProcessorFactory> mProcessorFactory;

        [SetUp]
        public void Init()
        {
            mContext = new Mock<IValidatorRunnerContext>();
            mUrlAggregator = new Mock<IUrlAggregator>();
            mProcessorFactory = new Mock<IUrlProcessorFactory>();

            mContext
                .Setup(x => x.TotalTimeStopwatch)
                .Returns(new Mock<Stopwatch>().Object);
            mContext
                .Setup(x => x.Announcer)
                .Returns(new Mock<IAnnouncer>().Object);

            mProcessorFactory
                .Setup(x => x.GetUrlProcessor(It.IsAny<string>()))
                .Returns(new Mock<UrlProcessor>().Object);
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
