using System;
using TodoList.Contracts.Services;

namespace TodoList.Services
{
    public class ListItemDateTimeGenerator : IListItemDateTimeGenerator
    {
        public DateTime GenerateDateTime()
        {
            return DateTime.Now;
        }
    }
}