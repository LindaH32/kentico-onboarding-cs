using System;
using System.Threading.Tasks;

namespace TodoList.Contracts.Services
{
    public interface IDateTimeGenerator
    {
        Task<DateTime> GetCurrentDateTime();
    }
}