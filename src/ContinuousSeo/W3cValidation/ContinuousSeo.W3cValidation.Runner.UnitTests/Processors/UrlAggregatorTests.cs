namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using Moq;
    using NUnit.Framework;
    using ContinuousSeo.Core.Announcers;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Processors;
    using ContinuousSeo.W3cValidation.Runner.Initialization;

    [TestFixture]
    public class UrlAggregatorTests
    {
        #region SetUp / TearDown

        private Mock<IHtmlValidatorRunnerContext> mContext = null;
        private Mock<IProjectFileParser> mProjectFileParser = null;
        private Mock<ISitemapsParser> mSitemapsParser = null;
        private int mTotalProjectFileParserParseFileCalls = 0;
        private int mTotalSitemapsParserParseFileCalls = 0;
        

        [SetUp]
        public void Init()
        {
            mContext = new Mock<IHtmlValidatorRunnerContext>();
            mProjectFileParser = new Mock<IProjectFileParser>();
            mSitemapsParser = new Mock<ISitemapsParser>();

            mContext
                .Setup(x => x.Announcer)
                .Returns(new Mock<IAnnouncer>().Object);
            mContext
                .Setup(x => x.TotalTimeStopwatch)
                .Returns(new Mock<Stopwatch>().Object);
        }

        [TearDown]
        public void Dispose()
        {
            mContext = null;
            mProjectFileParser = null;
            mSitemapsParser = null;
            mTotalProjectFileParserParseFileCalls = 0;
            mTotalSitemapsParserParseFileCalls = 0;
        }


        private IUrlAggregator NewUrlAggregatorInstance()
        {
            return new UrlAggregator(
                mContext.Object, 
                mProjectFileParser.Object, 
                mSitemapsParser.Object);
        }

        private void SetupSitemapsParserParseFileToTrackNumberOfCallsToAllOverloads()
        {
            mSitemapsParser
                .Setup(x => x.ParseUrlsFromFile(It.IsAny<string>()))
                .Callback(() => mTotalSitemapsParserParseFileCalls++)
                .Returns((string fileOrUrl) => new string[] { string.Empty });
            mSitemapsParser
                .Setup(x => x.ParseUrlsFromFile(It.IsAny<Stream>()))
                .Callback(() => mTotalSitemapsParserParseFileCalls++)
                .Returns((string fileOrUrl) => new string[] { string.Empty });
        }

        private void SetupProjectFileParserParseFileToTrackNumberOfCallsToAllOverloads()
        {
            mProjectFileParser
                .Setup(x => x.ParseFile(It.IsAny<Stream>(), It.IsAny<string[]>()))
                .Callback(() => mTotalProjectFileParserParseFileCalls++)
                .Returns((Stream file, string[] args) => new List<IProjectFileLineInfo>());
            mProjectFileParser
                .Setup(x => x.ParseFile(It.IsAny<string>(), It.IsAny<string[]>()))
                .Callback(() => mTotalProjectFileParserParseFileCalls++)
                .Returns((string path, string[] args) => new List<IProjectFileLineInfo>());
        }

        private void SetupProjectFileParserParseFileToReturn4SingleUrls()
        {
            mProjectFileParser
                .Setup(x => x.ParseFile(It.IsAny<string>(), It.IsAny<string[]>()))
                .Returns(new List<IProjectFileLineInfo>()
                    {
                        new ProjectFileLineInfo("http://www.google.com/", "single"),
                        new ProjectFileLineInfo("http://www.google.com/test.aspx","single"),
                        new ProjectFileLineInfo("http://www.google.com/test2.aspx", "single"),
                        new ProjectFileLineInfo("http://www.google.com/test3.aspx", "single")
                    });
        }


        #endregion

        #region AggregateUrls Method

        [Test]
        public void AggregateUrls_ContextTargetSitemapFiles3Provided_ShouldCallSitemapsParserProcessFile3Times()
        {
            // arrange
            mContext.Setup(x => x.TargetSitemapsFiles).Returns(new List<string>() {
                "http://mysite.com/sitemaps1.xml",
                "http://myothersite.com/sitemaps.sitemap",
                "http://www.whatever.com/sitemaps.file"
            });
            SetupSitemapsParserParseFileToTrackNumberOfCallsToAllOverloads();

            var target = NewUrlAggregatorInstance();

            // act
            var result = target.AggregateUrls();

            // assert
            var actual = mTotalSitemapsParserParseFileCalls;
            var expected = 3;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AggregateUrls_ContextTargetSitemapFiles2ProvidedWithReplacableTokens_ShouldReplaceBothTokens()
        {
            // arrange
            var targetSitemapsFiles = new List<string>() 
            {
                @"http://{0}/sitemap.xml",
                @"http://{1}/sitemap.xml"
            };
            var urlReplacementArgs = new List<string>()
            {
                @"www.mydomain1.com",
                @"www.mydomain2.com"
            };
            mContext.Setup(x => x.TargetSitemapsFiles).Returns(targetSitemapsFiles);
            mContext.Setup(x => x.UrlReplacementArgs).Returns(urlReplacementArgs);
            SetupSitemapsParserParseFileToTrackNumberOfCallsToAllOverloads();

            var target = NewUrlAggregatorInstance();

            // act
            var result = target.AggregateUrls();

            // assert
            var expectedUrl1 = @"http://www.mydomain1.com/sitemap.xml";
            var expectedUrl2 = @"http://www.mydomain2.com/sitemap.xml";

            mSitemapsParser
                .Verify(x => x.ParseUrlsFromFile(It.Is<string>(p => p == expectedUrl1)),
                Times.Once());
            mSitemapsParser
                .Verify(x => x.ParseUrlsFromFile(It.Is<string>(p => p == expectedUrl2)),
                Times.Once());
        }

        [Test]
        public void AggregateUrls_ContextTargetUrls2Provided_ShouldReturnSameUrls()
        {
            // arrange
            var targetUrls = new List<string>() 
            {
                @"http://www.mydomain.com/url.html",
                @"http://www.mydomain2.com/url2.html"
            };
            mContext.Setup(x => x.TargetUrls).Returns(targetUrls);
            
            var target = NewUrlAggregatorInstance();

            // act
            var result = target.AggregateUrls();

            // assert
            var actual1 = result.ElementAt(0);
            var expected1 = @"http://www.mydomain.com/url.html";

            var actual2 = result.ElementAt(1);
            var expected2 = @"http://www.mydomain2.com/url2.html";

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        [Test]
        public void AggregateUrls_ContextTargetUrls2ProvidedWithReplacableTokens_ShouldReplaceBothTokens()
        {
            // arrange
            var targetUrls = new List<string>() 
            {
                @"http://{0}/url.html",
                @"http://{1}/url2.html"
            };
            var urlReplacementArgs = new List<string>()
            {
                @"www.mydomain1.com",
                @"www.mydomain2.com"
            };

            mContext.Setup(x => x.TargetUrls).Returns(targetUrls);
            mContext.Setup(x => x.UrlReplacementArgs).Returns(urlReplacementArgs);

            var target = NewUrlAggregatorInstance();

            // act
            var result = target.AggregateUrls();

            // assert
            var actual1 = result.ElementAt(0);
            var expected1 = @"http://www.mydomain1.com/url.html";

            var actual2 = result.ElementAt(1);
            var expected2 = @"http://www.mydomain2.com/url2.html";

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        [Test]
        public void AggregateUrls_ContextTargetProjectFileWith4SingleUrls_ShouldCallUrlFileParserParseFile1Time()
        {
            // arrange
            var targetUrlFiles = new List<string>() { @"C:\Testing\Test.txt" };

            SetupProjectFileParserParseFileToReturn4SingleUrls();

            mContext.Setup(x => x.TargetProjectFiles).Returns(targetUrlFiles);

            var target = NewUrlAggregatorInstance();

            // act
            var result = target.AggregateUrls();

            // assert
            mProjectFileParser
                .Verify(x => x.ParseFile(It.IsAny<string>(), It.IsAny<string[]>()), 
                Times.Once());
        }

        [Test]
        public void AggregateUrls_ContextTargetProjectFileWith4SingleUrls_ShouldReturn4Urls()
        {
            // arrange
            var targetProjectFiles = new List<string>() { @"C:\Testing\Test.txt" };

            SetupProjectFileParserParseFileToReturn4SingleUrls();

            mContext.Setup(x => x.TargetProjectFiles).Returns(targetProjectFiles);

            var target = NewUrlAggregatorInstance();

            // act
            var result = target.AggregateUrls();

            // assert
            var expected = 4;
            var actual = result.Count();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        //#region ProcessLine Method

        //[Test]
        //public void ProcessLine_ValidFileLineInfoWithoutMode_ShouldReturnSameUrl()
        //{
        //    // arrange
        //    IEnumerable<string> result;
        //    var url = "http://www.google.com/test.aspx";
        //    var info = new Mock<IProjectFileLineInfo>();
        //    info.Setup(x => x.Url).Returns(url);

        //    var sitemapsParser = new Mock<ISitemapsParser>();
            
        //    mUrlAggregator target = new mUrlAggregator(sitemapsParser.Object);

        //    // act
        //    result = target.ProcessLine(info.Object);

        //    // assert
        //    var actual = result.First();
        //    var expected = url;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessLine_ValidFileLineInfoWithNullMode_ShouldReturnSameUrl()
        //{
        //    // arrange
        //    IEnumerable<string> result;
        //    var url = "http://www.google.com/test.aspx";
        //    var info = new Mock<IProjectFileLineInfo>();
        //    info.Setup(x => x.Url).Returns(url);
        //    info.Setup(x => x.Mode).Returns((string)null);

        //    var sitemapsParser = new Mock<ISitemapsParser>();

        //    mUrlAggregator target = new mUrlAggregator(sitemapsParser.Object);

        //    // act
        //    result = target.ProcessLine(info.Object);

        //    // assert
        //    var actual = result.First();
        //    var expected = url;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessLine_ValidFileLineInfoWithSingleMode_ShouldReturnSameUrl()
        //{
        //    // arrange
        //    IEnumerable<string> result;
        //    var url = "http://www.google.com/test.aspx";
        //    var info = new Mock<IProjectFileLineInfo>();
        //    info.Setup(x => x.Url).Returns(url);
        //    info.Setup(x => x.Mode).Returns("single");

        //    var sitemapsParser = new Mock<ISitemapsParser>();

        //    mUrlAggregator target = new mUrlAggregator(sitemapsParser.Object);

        //    // act
        //    result = target.ProcessLine(info.Object);

        //    // assert
        //    var actual = result.First();
        //    var expected = url;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessLine_ValidFileLineInfoWithSitemapsModeAnd2Urls_ShouldReturn2Urls()
        //{
        //    // arrange
        //    IEnumerable<string> result;
        //    var url = "http://www.google.com/sitemaps.xml";
        //    var info = new Mock<IProjectFileLineInfo>();
        //    info.Setup(x => x.Url).Returns(url);
        //    info.Setup(x => x.Mode).Returns("sitemaps");

        //    var urlList = new List<string>();
        //    urlList.Add("http://www.google.com/");
        //    urlList.Add("http://www.google.com/test1.aspx");
        //    var sitemapsParser = new Mock<ISitemapsParser>();
        //    sitemapsParser.Setup(x => x.ParseUrlsFromFile(url)).Returns(urlList);

        //    mUrlAggregator target = new mUrlAggregator(sitemapsParser.Object);

        //    // act
        //    result = target.ProcessLine(info.Object);

        //    // assert
        //    var actual = result.Count();
        //    var expected = 2;

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessLine_ValidFileLineInfoWithSitemapsModeAnd2Urls_ShouldMatch1stUrl()
        //{
        //    // arrange
        //    IEnumerable<string> result;
        //    var url = "http://www.google.com/sitemaps.xml";
        //    var info = new Mock<IProjectFileLineInfo>();
        //    info.Setup(x => x.Url).Returns(url);
        //    info.Setup(x => x.Mode).Returns("sitemaps");

        //    var urlList = new List<string>();
        //    urlList.Add("http://www.google.com/");
        //    urlList.Add("http://www.google.com/test1.aspx");
        //    var sitemapsParser = new Mock<ISitemapsParser>();
        //    sitemapsParser.Setup(x => x.ParseUrlsFromFile(url)).Returns(urlList);

        //    mUrlAggregator target = new mUrlAggregator(sitemapsParser.Object);

        //    // act
        //    result = target.ProcessLine(info.Object);

        //    // assert
        //    var actual = result.First();
        //    var expected = "http://www.google.com/";

        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ProcessLine_ValidFileLineInfoWithSitemapsModeAnd2Urls_ShouldMatch2ndUrl()
        //{
        //    // arrange
        //    IEnumerable<string> result;
        //    var url = "http://www.google.com/sitemaps.xml";
        //    var info = new Mock<IProjectFileLineInfo>();
        //    info.Setup(x => x.Url).Returns(url);
        //    info.Setup(x => x.Mode).Returns("sitemaps");

        //    var urlList = new List<string>();
        //    urlList.Add("http://www.google.com/");
        //    urlList.Add("http://www.google.com/test1.aspx");
        //    var sitemapsParser = new Mock<ISitemapsParser>();
        //    sitemapsParser.Setup(x => x.ParseUrlsFromFile(url)).Returns(urlList);

        //    mUrlAggregator target = new mUrlAggregator(sitemapsParser.Object);

        //    // act
        //    result = target.ProcessLine(info.Object);

        //    // assert
        //    var actual = result.ElementAt(1);
        //    var expected = "http://www.google.com/test1.aspx";

        //    Assert.AreEqual(expected, actual);
        //}

        //#endregion

         #region Mock Classes

        //private class MockUrlAggregator : mUrlAggregator
        //{
            
        //    public MockUrlAggregator(IProjectFileParser projectFileParser, ISitemapsParser sitemapsParser) 
        //        : base(projectFileParser, sitemapsParser)
        //    {
        //    }


        //    public override IEnumerable<string> ProcessLine(IProjectFileLineInfo urlInfo)
        //    {
        //        return base.ProcessLine(urlInfo);
        //    }
        //}

        #endregion

    }
}
