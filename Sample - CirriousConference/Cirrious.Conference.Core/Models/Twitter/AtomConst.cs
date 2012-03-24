using System.Xml.Linq;

namespace Cirrious.Conference.Core.Models.Twitter
{
    public static class AtomConst
    {
        private const string AtomNamespace = "http://www.w3.org/2005/Atom";

        public static XName Entry = XName.Get("entry", AtomNamespace);
        public static XName ID = XName.Get("id", AtomNamespace);
        public static XName Link = XName.Get("link", AtomNamespace);
        public static XName Published = XName.Get("published", AtomNamespace);
        public static XName Name = XName.Get("name", AtomNamespace);
        public static XName Title = XName.Get("title", AtomNamespace);
    }
}
