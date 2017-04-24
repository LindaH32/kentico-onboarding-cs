using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoList.Api.Models
{
    public class ListItem
    {
        public ListItem(string id, string text)
        {
            Id = id;
            Text = text;
        }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Text { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Text:{Text}";
        }
    }
}