using System;

namespace TodoList.Contracts.Services
{
    public interface IListItemDateTimeGenerator
    {
        DateTime GenerateDateTime();
    }
}