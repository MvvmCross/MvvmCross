using System;

namespace MyApplication.Core.Interfaces.First
{
    public class SimpleItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public DateTime When { get; set; }
    }
}