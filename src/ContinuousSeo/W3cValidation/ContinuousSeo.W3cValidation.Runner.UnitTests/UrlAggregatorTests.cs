namespace ContinuousSeo.W3cValidation.Runner.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Moq;
    using NUnit.Framework;
    using ContinuousSeo.W3cValidation.Runner.Parsers;
    using ContinuousSeo.W3cValidation.Runner.Processors;

    [TestFixture]
    public class UrlAggregatorTests
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
        public void ProcessUrl_ValidFileLineInfoWithoutMode_ShouldReturnSameUrl()
        {
            // arrange
            IEnumerable<string> result;
            var url = "http://www.google.com/test.aspx";
            var info = new Mock<IUrlFileLineInfo>();
            info.Setup(x => x.Url).Returns(url);

            var sitemapsParser = new Mock<ISitemapsParser>();
            
            UrlAggregator target = new UrlAggregator(sitemapsParser.Object);

            // act
            result = target.ProcessUrl(info.Object);

            // assert
            var actual = result.First();
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidFileLineInfoWithNullMode_ShouldReturnSameUrl()
        {
            // arrange
            IEnumerable<string> result;
            var url = "http://www.google.com/test.aspx";
            var info = new Mock<IUrlFileLineInfo>();
            info.Setup(x => x.Url).Returns(url);
            info.Setup(x => x.Mode).Returns((string)null);

            var sitemapsParser = new Mock<ISitemapsParser>();

            UrlAggregator target = new UrlAggregator(sitemapsParser.Object);

            // act
            result = target.ProcessUrl(info.Object);

            // assert
            var actual = result.First();
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidFileLineInfoWithSingleMode_ShouldReturnSameUrl()
        {
            // arrange
            IEnumerable<string> result;
            var url = "http://www.google.com/test.aspx";
            var info = new Mock<IUrlFileLineInfo>();
            info.Setup(x => x.Url).Returns(url);
            info.Setup(x => x.Mode).Returns("single");

            var sitemapsParser = new Mock<ISitemapsParser>();

            UrlAggregator target = new UrlAggregator(sitemapsParser.Object);

            // act
            result = target.ProcessUrl(info.Object);

            // assert
            var actual = result.First();
            var expected = url;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidFileLineInfoWithSitemapsModeAnd2Urls_ShouldReturn2Urls()
        {
            // arrange
            IEnumerable<string> result;
            var url = "http://www.google.com/sitemaps.xml";
            var info = new Mock<IUrlFileLineInfo>();
            info.Setup(x => x.Url).Returns(url);
            info.Setup(x => x.Mode).Returns("sitemaps");

            var urlList = new List<string>();
            urlList.Add("http://www.google.com/");
            urlList.Add("http://www.google.com/test1.aspx");
            var sitemapsParser = new Mock<ISitemapsParser>();
            sitemapsParser.Setup(x => x.ParseUrlsFromFile(url)).Returns(urlList);

            UrlAggregator target = new UrlAggregator(sitemapsParser.Object);

            // act
            result = target.ProcessUrl(info.Object);

            // assert
            var actual = result.Count();
            var expected = 2;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidFileLineInfoWithSitemapsModeAnd2Urls_ShouldMatch1stUrl()
        {
            // arrange
            IEnumerable<string> result;
            var url = "http://www.google.com/sitemaps.xml";
            var info = new Mock<IUrlFileLineInfo>();
            info.Setup(x => x.Url).Returns(url);
            info.Setup(x => x.Mode).Returns("sitemaps");

            var urlList = new List<string>();
            urlList.Add("http://www.google.com/");
            urlList.Add("http://www.google.com/test1.aspx");
            var sitemapsParser = new Mock<ISitemapsParser>();
            sitemapsParser.Setup(x => x.ParseUrlsFromFile(url)).Returns(urlList);

            UrlAggregator target = new UrlAggregator(sitemapsParser.Object);

            // act
            result = target.ProcessUrl(info.Object);

            // assert
            var actual = result.First();
            var expected = "http://www.google.com/";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ProcessUrl_ValidFileLineInfoWithSitemapsModeAnd2Urls_ShouldMatch2ndUrl()
        {
            // arrange
            IEnumerable<string> result;
            var url = "http://www.google.com/sitemaps.xml";
            var info = new Mock<IUrlFileLineInfo>();
            info.Setup(x => x.Url).Returns(url);
            info.Setup(x => x.Mode).Returns("sitemaps");

            var urlList = new List<string>();
            urlList.Add("http://www.google.com/");
            urlList.Add("http://www.google.com/test1.aspx");
            var sitemapsParser = new Mock<ISitemapsParser>();
            sitemapsParser.Setup(x => x.ParseUrlsFromFile(url)).Returns(urlList);

            UrlAggregator target = new UrlAggregator(sitemapsParser.Object);

            // act
            result = target.ProcessUrl(info.Object);

            // assert
            var actual = result.ElementAt(1);
            var expected = "http://www.google.com/test1.aspx";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
