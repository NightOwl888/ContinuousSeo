namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using ContinuousSeo.W3cValidation.Runner.Parser;

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

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
            UrlFileParser target = new UrlFileParser();

            // act
            var result = target.ParseLine(line, args);

            // assert
            IUrlFileLineInfo actual = result;
            IUrlFileLineInfo expected = null;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ParseFile Method

        private void WriteLinesToStream(Stream stream, string[] lines)
        {
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
            // Flush the writer
            writer.Flush();

            // Leave StreamWriter open or the underlying stream will be closed

            // Reset stream to beginning
            stream.Position = 0;
        }

        private void WriteStreamWith4Urls(Stream stream)
        {
            List<String> lines = new List<String>();
            lines.Add("http://www.google.com/\tsingle");
            lines.Add("http://www.google.com/test.aspx\tsingle");
            lines.Add("http://www.google.com/test2.aspx\tsingle");
            lines.Add("http://www.google.com/test3.aspx\tsingle");
            WriteLinesToStream(stream, lines.ToArray());
        }

        [Test]
        public void ParseFile_ValidStreamWith4Urls_ShouldReturn4Urls()
        {
            // arrange
            string[] args = new string[] { };
            UrlFileParser target = new UrlFileParser();
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

        #endregion

        #endregion

    }
}
