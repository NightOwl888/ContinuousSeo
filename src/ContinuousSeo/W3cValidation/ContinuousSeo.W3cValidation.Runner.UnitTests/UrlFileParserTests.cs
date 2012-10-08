namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Runner.Parsers;

    [TestFixture]
    public class UrlFileParserTests
    {
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region Tests

        #region ParseLine Method

        [Test]
        public void ParseLine_ValidLine_ShouldReturnUrlFromLine()
        {
            // arrange
            string line = "http://www.google.com/\tsingle";
            string[] args = new string[0];
            var fileReader = new Mock<IFileReader>();
            IUrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);
            
            // assert
            var actual = result.Url;
            var expected = "http://www.google.com/";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLine_ShouldReturnModeFromLine()
        {
            // arrange
            string line = "http://www.google.com/\ttestmode";
            string[] args = new string[0];
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            var actual = result.Mode;
            var expected = "testmode";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLine_ShouldReturnLowercaseModeFromLine()
        {
            // arrange
            string line = "http://www.google.com/\tTESTMODE";
            string[] args = new string[0];
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            var actual = result.Mode;
            var expected = "testmode";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLineWithoutMode_ShouldReturnSingleMode()
        {
            // arrange
            string line = "http://www.google.com/";
            string[] args = new string[0];
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            var actual = result.Mode;
            var expected = "single";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLineWithEmptyMode_ShouldReturnSingleMode()
        {
            // arrange
            string line = "http://www.google.com/\t";
            string[] args = new string[0];
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            var actual = result.Mode;
            var expected = "single";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_LineUrlWithReplacementDomain_ShouldReturnUrlWithDomain()
        {
            // arrange
            string line = "http://{0}/test.aspx";
            string[] args = new string[] { "www.google.com" };
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            var actual = result.Url;
            var expected = "http://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_LineUrlWith2ReplacementDomains_ShouldReturnUrlWith2ndDomain()
        {
            // arrange
            string line = "http://{1}/test.aspx";
            string[] args = new string[] { "www.google.com", "www.mydomain.com" };
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            var actual = result.Url;
            var expected = "http://www.mydomain.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_NullLine_ShouldReturnNullFileLineInfo()
        {
            // arrange
            string line = null;
            string[] args = new string[] { "www.google.com" };
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            IUrlFileLineInfo actual = result;
            IUrlFileLineInfo expected = null;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_EmptyLine_ShouldReturnNullFileLineInfo()
        {
            // arrange
            string line = string.Empty;
            string[] args = new string[] { "www.google.com" };
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            IUrlFileLineInfo actual = result;
            IUrlFileLineInfo expected = null;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ParseFile Method

        private static void WriteStreamWith4Urls(Stream stream)
        {
            List<String> lines = new List<String>();
            lines.Add("http://www.google.com/\tsingle");
            lines.Add("http://www.google.com/test.aspx\tsingle");
            lines.Add("http://www.google.com/test2.aspx\tsingle");
            lines.Add("http://www.google.com/test3.aspx\tsingle");
            Utilities.WriteLinesToStream(stream, lines.ToArray());
        }

        [Test]
        public void ParseFile_ValidStreamWith4Urls_ShouldReturn4Urls()
        {
            // arrange
            string[] args = new string[] {};
            var fileReader = new Mock<IFileReader>();
            UrlFileParser target = new UrlFileParser(fileReader.Object);
            IEnumerable<IUrlFileLineInfo> result;

            using (Stream file = new MemoryStream())
            {
                WriteStreamWith4Urls(file);

            // act
                result = target.ParseFile(file, args);
            }


            // assert
            var actual = result.Count();
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseFile_ValidFilePathWith4Urls_ShouldReturn4Urls()
        {
            // arrange
            string[] args = new string[] {};
            IEnumerable<IUrlFileLineInfo> result;
            using (Stream stream = new MemoryStream())
            {
                WriteStreamWith4Urls(stream);

                var fileReader = new Mock<IFileReader>();
                fileReader.Setup(x => x.GetFileStream(string.Empty)).Returns(stream);
                UrlFileParser target = new UrlFileParser(fileReader.Object);

                // act
                result = target.ParseFile(stream, args);
            }


            // assert
            var actual = result.Count();
            var expected = 4;

            Assert.AreEqual(expected, actual);
        }


        #endregion

        #endregion

    }
}