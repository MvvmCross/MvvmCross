using System;

namespace TwitterSearch.Core.Models
{
    public class Tweet
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime Timestamp { get; set; }
    }
}