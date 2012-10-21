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
    using System.Text;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.Core.IO;
    using ContinuousSeo.W3cValidation.Runner.Parsers;

    [TestFixture]
    public class ProjectFileParserTests
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
            var streamFactory = new Mock<IStreamFactory>();
            IProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            IProjectFileLineInfo actual = result;
            IProjectFileLineInfo expected = null;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ParseLine_EmptyLine_ShouldReturnNullFileLineInfo()
        {
            // arrange
            string line = string.Empty;
            string[] args = new string[] { "www.google.com" };
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

            // act
            var result = target.ParseLine(line, args);

            // assert
            IProjectFileLineInfo actual = result;
            IProjectFileLineInfo expected = null;

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
            var streamFactory = new Mock<IStreamFactory>();
            ProjectFileParser target = new ProjectFileParser(streamFactory.Object);
            IEnumerable<IProjectFileLineInfo> result;

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
            IEnumerable<IProjectFileLineInfo> result;
            using (Stream stream = new MemoryStream())
            {
                WriteStreamWith4Urls(stream);

                var streamFactory = new Mock<IStreamFactory>();
                streamFactory.Setup(x => x.GetFileStream(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(stream);
                ProjectFileParser target = new ProjectFileParser(streamFactory.Object);

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
