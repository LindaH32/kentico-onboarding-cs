using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task GetCurrentDateTime_ReturnsDelayedDateTimesCorrectly()
        {
            var waitingTime = 100;
            var precisionDeviation = 20;

            var firstTime = await _dateTimeGenerator.GetCurrentDateTime();
            Thread.Sleep(waitingTime);
            var secondTime = await _dateTimeGenerator.GetCurrentDateTime();
            
            Assert.That(firstTime.AddMilliseconds(waitingTime), Is.EqualTo(secondTime).Within(precisionDeviation).Milliseconds);
        }

        [Test]
        public async Task GetCurrentDateTime_DoesntReturnDuplicateValues()
        {
            var numberOfIterations = 10;
            List<DateTime> dateTimes = new List<DateTime>();

            for (int i = 0; i < numberOfIterations; i++)
            {
                Thread.Sleep(1);
                dateTimes.Add(await _dateTimeGenerator.GetCurrentDateTime());
            }

            Assert.That(dateTimes, Is.Unique);
        }

        [Test]
        public async Task GetCurrentDateTime_DoesntReturnMinOrMaxValue()
        {
            var generatedDateTime = await _dateTimeGenerator.GetCurrentDateTime();

            Assert.That(generatedDateTime, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(generatedDateTime, Is.Not.EqualTo(DateTime.MaxValue));
        }
    }
}