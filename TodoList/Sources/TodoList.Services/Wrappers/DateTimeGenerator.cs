using System;
using TodoList.Contracts.Services;
using System.Threading.Tasks;


namespace TodoList.Services.Wrappers
{
    internal class DateTimeGenerator : IDateTimeGenerator
    {        
        public async Task<DateTime> GetCurrentDateTime()
            => await Task.FromResult(DateTime.Now);
        
    }
}