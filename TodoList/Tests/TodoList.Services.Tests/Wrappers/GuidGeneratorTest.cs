using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task GenerateGuid_DoesntReturnDuplicateValues()
        {
            var numberOfIterations = 10;
            List<Guid> generatedGuids = new List<Guid>();

            for (int i = 0; i < numberOfIterations; i++)
            {
                generatedGuids.Add(await _guidGenerator.GenerateGuid());
            }

            Assert.That(generatedGuids, Is.Unique);
        }

        [Test]
        public async Task GenerateGuid_DoesntReturnEmptyValue()
        {
            var generatedGuid = await _guidGenerator.GenerateGuid();

            Assert.That(generatedGuid, Is.Not.EqualTo(Guid.Empty));
        }


    }
}