using System;

namespace TodoList.Contracts.Models
{
    public class ListItem
    {
        public ListItem(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
        
        public Guid Id { get; }
       
        public string Text { get; set; }

        public override string ToString()
            => $"ID: {Id}, Text:{Text}";
    }
}