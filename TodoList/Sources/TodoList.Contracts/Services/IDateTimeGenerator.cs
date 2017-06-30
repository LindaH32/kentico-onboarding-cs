using System;

namespace TodoList.Contracts.Services
{
    public interface IDateTimeGenerator
    {
        DateTime GenerateDateTime();
    }
}