using System;

namespace Cirrious.Conference.Core.Models.Raw
{
    public class Session
    {
        public string Key { get; set; }
        public DateTime When { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SpeakerKey { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string Where { get; set; }
    }
}
