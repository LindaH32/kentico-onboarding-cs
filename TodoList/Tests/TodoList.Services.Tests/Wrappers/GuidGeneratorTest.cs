using System;
using NUnit.Framework;
using TodoList.Contracts.Services;
using TodoList.Services.Wrappers;

namespace TodoList.Services.Tests.Wrappers
{
    [TestFixture]
    public class GuidGeneratorTest
    {
        private IGuidGenerator _guidGenerator;
        [SetUp]
        public void Init()
        {
            _guidGenerator = new GuidGenerator();
        }

        [Test]
        public void GenerateGuid_ReturnsCorrectGuid()
        {
            var generatedGuid = _guidGenerator.GenerateGuid().ToString();
            Guid guidOutput;
            var isValid = Guid.TryParse(generatedGuid, out guidOutput);
            Assert.IsTrue(isValid);
        }
    }
}