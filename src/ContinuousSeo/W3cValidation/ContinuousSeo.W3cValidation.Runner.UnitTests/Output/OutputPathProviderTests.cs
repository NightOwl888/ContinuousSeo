namespace ContinuousSeo.W3cValidation.Runner.UnitTests.Output
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.W3cValidation.Runner.Initialization;
    using ContinuousSeo.W3cValidation.Runner.Output;

    [TestFixture]
    public class OutputPathProviderTests
    {
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region GetOutputFileNameExtension Method

        [Test]
        public void GetOutputFileNameExtension_ContextOutputFormatHtmlLowerCase_ReturnsHtmlExtension()
        {
            // arrange
            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("html");
            var fileNameGenerator = new Mock<IFileNameGenerator>();

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputFileNameExtension();

            // assert
            var actual = result;
            var expected = "html";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetOutputFileNameExtension_ContextOutputFormatHtmlUpperCase_ReturnsHtmlExtension()
        {
            // arrange
            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("HTML");
            var fileNameGenerator = new Mock<IFileNameGenerator>();

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputFileNameExtension();

            // assert
            var actual = result;
            var expected = "html";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetOutputFileNameExtension_ContextOutputFormatXmlLowerCase_ReturnsXmlExtension()
        {
            // arrange
            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("xml");
            var fileNameGenerator = new Mock<IFileNameGenerator>();

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputFileNameExtension();

            // assert
            var actual = result;
            var expected = "xml";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetOutputFileNameExtension_ContextOutputFormatXmlUpperCase_ReturnsXmlExtension()
        {
            // arrange
            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("XML");
            var fileNameGenerator = new Mock<IFileNameGenerator>();

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputFileNameExtension();

            // assert
            var actual = result;
            var expected = "xml";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region GetOutputFileName Method

        [Test]
        public void GetOutputFileName_ContextOutputFormatHtml_ReturnsGeneratedFileName()
        {
            // arrange
            var url = "http://www.google.com/";

            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("html");
            var fileNameGenerator = new Mock<IFileNameGenerator>();
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "html")).Returns("Testing.html");
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "xml")).Returns("Testing.xml");

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputFileName(url);

            // assert
            var actual = result;
            var expected = "Testing.html";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetOutputFileName_ContextOutputFormatXml_ReturnsGeneratedFileName()
        {
            // arrange
            var url = "http://www.google.com/";

            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("xml");
            var fileNameGenerator = new Mock<IFileNameGenerator>();
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "html")).Returns("Testing.html");
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "xml")).Returns("Testing.xml");

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputFileName(url);

            // assert
            var actual = result;
            var expected = "Testing.xml";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region GetOuputPath Method

        [Test]
        public void GetOutputPath_ContextOutputPathNull_ReturnsGeneratedFileNameOnly()
        {
            // arrange
            var url = "http://www.google.com/";

            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("html");
            var fileNameGenerator = new Mock<IFileNameGenerator>();
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "html")).Returns("Testing.html");
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "xml")).Returns("Testing.xml");

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputPath(url);

            // assert
            var actual = result;
            var expected = "Testing.html";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetOutputPath_ContextOutputPathProvided_ReturnsConcatenatedPath()
        {
            // arrange
            var url = "http://www.google.com/";

            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("html");
            context.Setup(x => x.OutputPath).Returns(@"C:\Test");
            var fileNameGenerator = new Mock<IFileNameGenerator>();
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "html")).Returns("Testing.html");
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "xml")).Returns("Testing.xml");

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputPath(url);

            // assert
            var actual = result;
            var expected = @"C:\Test\Testing.html";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region GetOutputPath Method (no parameters)

        [Test]
        public void GetOutputPathWithoutExtension_ContextOutputPathProvided_ReturnsPath()
        {
            // arrange
            var url = "http://www.google.com/";

            var context = new Mock<IValidatorRunnerContext>();
            context.Setup(x => x.OutputFormat).Returns("html");
            context.Setup(x => x.OutputPath).Returns(@"C:\Test");
            var fileNameGenerator = new Mock<IFileNameGenerator>();
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "html")).Returns("Testing.html");
            fileNameGenerator.Setup(x => x.GenerateFileName(url, "xml")).Returns("Testing.xml");

            OutputPathProvider target = new OutputPathProvider(context.Object, fileNameGenerator.Object);

            // act
            var result = target.GetOutputPath();

            // assert
            var actual = result;
            var expected = @"C:\Test";

            Assert.AreEqual(expected, actual);
        }

        #endregion

    }
}
