using System;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    internal class DateTimeGenerator : IDateTimeGenerator
    {
        public DateTime GenerateDateTime() 
            => DateTime.Now;
    }
}