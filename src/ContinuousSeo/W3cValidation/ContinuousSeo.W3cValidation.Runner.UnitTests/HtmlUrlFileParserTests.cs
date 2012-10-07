using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContinuousSeo.W3cValidation.Runner.Parser;

namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    [TestFixture]
    public class HtmlUrlFileParserTests
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


        [Test]
        public void ParseLine_ValidLine_ShouldReturnUrlFromLine()
        {
            // arrange
            string line = "http://www.google.com/\tsingle";
            string domain = string.Empty;
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);
            

            // assert
            var actual = result.Url;
            var expected = "http://www.google.com/";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLine_ShouldReturnModeFromLine()
        {
            // arrange
            string line = "http://www.google.com/\t\ttestmode";
            string domain = string.Empty;
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

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
            string domain = string.Empty;
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

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
            string domain = string.Empty;
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

            // assert
            var actual = result.Mode;
            var expected = "single";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLineAndDomain_ShouldReturnJoinedUrl()
        {
            // arrange
            string line = "/test.aspx\t";
            string domain = "www.google.com";
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

            // assert
            var actual = result.Url;
            var expected = "http://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_LineUrlWithoutPreceedingSlash_ShouldReturnUrlWithSlash()
        {
            // arrange
            string line = "test.aspx";
            string domain = "www.google.com";
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

            // assert
            var actual = result.Url;
            var expected = "http://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLineWithHttpsProtocol_ShouldAppendHttpsProtocolToUrl()
        {
            // arrange
            string line = "/test.aspx\thttps\t";
            string domain = "www.google.com";
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

            // assert
            var actual = result.Url;
            var expected = "https://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_ValidLineWithoutProtocol_ShouldAppendHttpProtocolToUrl()
        {
            // arrange
            string line = "/test.aspx\t\tsingle";
            string domain = "www.google.com";
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

            // assert
            var actual = result.Url;
            var expected = "http://www.google.com/test.aspx";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_NullLine_ShouldReturnNullFileLineInfo()
        {
            // arrange
            string line = null;
            string domain = "www.google.com";
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

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
            string domain = "www.google.com";
            HtmlUrlFileParser target = new HtmlUrlFileParser();

            // act
            IUrlFileLineInfo result = target.ParseLine(line, domain);

            // assert
            IUrlFileLineInfo actual = result;
            IUrlFileLineInfo expected = null;

            Assert.AreEqual(expected, actual);
        }

        #endregion

    }
}
