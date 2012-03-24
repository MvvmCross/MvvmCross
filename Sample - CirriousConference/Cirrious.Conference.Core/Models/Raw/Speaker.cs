using System.Collections.Generic;

namespace Cirrious.Conference.Core.Models.Raw
{
#warning - not used :/
    public class Speaker
    {
        public string Key { get; set; }
        public string Employer { get; set; }
        public string Url { get; set; }
        public string RssUrl { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<string> SessionKeys { get; set; }
    }
}