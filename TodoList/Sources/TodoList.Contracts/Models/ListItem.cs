using System;

namespace TodoList.Contracts.Models
{
    public class ListItem
    {
        public Guid Id { get; set; }
       
        public string Text { get; set; }

        public DateTime CreationDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public override string ToString()
            => $"ID: {Id}, Text:{Text}";
    }
}