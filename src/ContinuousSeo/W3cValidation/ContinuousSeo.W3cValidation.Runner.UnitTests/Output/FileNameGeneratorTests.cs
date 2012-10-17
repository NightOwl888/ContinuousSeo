namespace ContinuousSeo.W3cValidation.Runner.UnitTests.Output
{
    using System;
    using NUnit.Framework;
    using Moq;
    using ContinuousSeo.Core;
    using ContinuousSeo.W3cValidation.Runner.Output;

    [TestFixture]
    public class FileNameGeneratorTests
    {
        #region SetUp / TearDown

        [SetUp]
        public void Init()
        { }

        [TearDown]
        public void Dispose()
        { }

        #endregion

        #region GenerateFileName Method

        //[Test]
        //public void GenerateFileName_ValidUrlAndExtension_ShouldReturnGuidAndExtension()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    var guid = new Guid("e51753ef-d00f-4336-8271-3e30c0017829");
        //    var extension = "html";

        //    var guidProvider = new Mock<GuidProvider>();
        //    guidProvider.Setup(x => x.NewGuid).Returns(guid);

        //    FileNameGenerator target = new FileNameGenerator(guidProvider.Object);

        //    // act
        //    var result = target.GenerateFileName(url, extension);

        //    // assert
        //    var actual = result;
        //    var expected = guid.ToString() + "." + extension;

        //    Assert.AreEqual(expected, actual);
        //}

        #endregion
    }
}
