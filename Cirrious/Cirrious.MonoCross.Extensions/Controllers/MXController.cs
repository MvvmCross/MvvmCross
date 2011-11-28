using System;
using System.Collections.Generic;
using MonoCross.Navigation.ActionResults;

namespace MonoCross.Navigation
{
#warning What are Parameters and Uri for here?!
    public interface IMXController
    {
        IMXActionResult Load(Dictionary<string, string> parameters);
    }

    public abstract class MXController
    {
        public abstract IMXActionResult Load(Dictionary<string, string> parameters);
    }
}
