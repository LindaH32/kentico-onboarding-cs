using System;
using NUnit.Framework;
using TodoList.Contracts.Services;
using TodoList.Services.Wrappers;

namespace TodoList.Services.Tests.Wrappers
{
    [TestFixture]
    public class DateTimeGeneratorTest
    {
        private IDateTimeGenerator _dateTimeGenerator;

        [SetUp]
        public void Init()
        {
            _dateTimeGenerator = new DateTimeGenerator();
        }

        [Test]
        public void GenerateDateTime_ReturnsCorrectDateTime()
        {
            Assert.That(_dateTimeGenerator.GenerateDateTime().Result, Is.EqualTo(DateTime.Now));
        }
        
    }
}