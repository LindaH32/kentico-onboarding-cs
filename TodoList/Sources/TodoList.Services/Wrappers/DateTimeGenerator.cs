using System;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    public class DateTimeGenerator : IDateTimeGenerator
    {
        public DateTime GenerateDateTime() 
            => DateTime.Now;
    }
}