using System.Collections.Generic;

namespace MonoCross.Navigation
{
    public class MXNavigation
    {
        public IMXController Controller { get; private set; }
        public string Pattern { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }

        public MXNavigation(string pattern, IMXController controller, Dictionary<string, string> parameters)
        {
            Controller = controller;
            Pattern = pattern;
            Parameters = parameters;
        }

        public string RegexPattern()
        {
            return Pattern.Replace("{", "(?<").Replace("}", @">[-&\w\. ]+)");
        }
	}
}
