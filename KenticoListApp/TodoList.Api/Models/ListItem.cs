﻿using System;

namespace TodoList.Api.Models
{
    public class ListItem
    {
        public ListItem(Guid id, string text)
        {
            Id = id;
            Text = text;
        }

        public ListItem(Guid id) : this(id, null)
        {
        }

        public Guid Id { get; set; }

        public string Text { get; set; }

        public override string ToString()
            => $"ID: {Id}, Text:{Text}";
    }
}