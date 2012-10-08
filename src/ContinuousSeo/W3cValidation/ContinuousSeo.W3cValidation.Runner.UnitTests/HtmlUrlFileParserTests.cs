using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ContinuousSeo.W3cValidation.Runner.Parser;

namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
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
            UrlFileLineInfo actual = result;
            UrlFileLineInfo expected = null;

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
            UrlFileLineInfo actual = result;
            UrlFileLineInfo expected = null;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region ParseFile Method



        #endregion

        #endregion

    }
}
