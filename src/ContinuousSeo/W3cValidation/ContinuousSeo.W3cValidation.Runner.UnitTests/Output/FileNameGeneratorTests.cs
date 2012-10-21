namespace ContinuousSeo.W3cValidation.Runner.UnitTests.Output
{
    using System;
    using System.IO;
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

        private IFileNameGenerator NewFileNameGeneratorInstance()
        {
            return new FileNameGenerator();
        }

        #endregion

        #region GenerateFileName Method

        [Test]
        public void GenerateFileName_ExtensionProvided_ShouldReturnValueEndingWithDotAndExtension()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            var extension = "html";

            var target = NewFileNameGeneratorInstance();

            // act
            var result = target.GenerateFileName(url, extension);

            // assert
            Assert.IsTrue(result.EndsWith("." + extension));
        }

        [Test]
        public void GenerateFileName_CalledWithLegalValues_ShouldReturnValueWithNoIllegalCharactersForWindowsFileName()
        {
            // arrange
            var url = "http://www.google.com/test.aspx";
            var extension = "html";

            var target = NewFileNameGeneratorInstance();

            // act
            var result = target.GenerateFileName(url, extension);

            // assert
            Assert.IsTrue(ContainsInvalidFileNameCharacter(result));
        }

        [Test]
        public void GenerateFileName_CalledWithValue300Characters_ShouldReturnValueWith250Characters()
        {
            // arrange
            var url = "http://www.google.com/test.aspx?value=9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
            var extension = "html";

            var target = NewFileNameGeneratorInstance();

            // act
            var result = target.GenerateFileName(url, extension);

            // assert
            Assert.IsTrue(result.Length == 250);
        }

        //[Test]
        //public void GenerateFileName_CalledWithValue300Characters_ShouldReturnValueWith250Characters()
        //{
        //    // arrange
        //    var url = "http://www.google.com/test.aspx";
        //    var extension = "html";

        //    var target = NewFileNameGeneratorInstance();

        //    // act
        //    var result = target.GenerateFileName(url, extension);

        //    // assert
        //    var actual = result;
        //    var expected = guid.ToString() + "." + extension;

        //    Assert.IsTrue(result.Length == 250);
        //}

        #endregion
        
        
        #region Helper Methods

        private bool ContainsInvalidFileNameCharacter(string value)
        {
            char[] chars = value.ToCharArray();
            foreach (char c in chars)
            {
                if (!IsValidFileNameCharacter(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValidFileNameCharacter(char input)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (c.Equals(input))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
