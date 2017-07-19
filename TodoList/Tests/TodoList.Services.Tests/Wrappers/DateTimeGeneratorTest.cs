using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            const int waitingTimeInMilliseconds = 20;
            const int precisionDeviationInMilliseconds = 3;

            var firstTime = await _dateTimeGenerator.GetCurrentDateTime();
            WaitInMilliseconds(waitingTimeInMilliseconds);
            var secondTime = await _dateTimeGenerator.GetCurrentDateTime();
            
            Assert.That(firstTime.AddMilliseconds(waitingTimeInMilliseconds), Is.EqualTo(secondTime).Within(precisionDeviationInMilliseconds).Milliseconds);
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

        private void WaitInMilliseconds(int milliseconds)
        {
            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds < milliseconds)
            {
                //waiting
            }
        }
    }
}