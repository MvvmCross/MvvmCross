using System.Collections.Generic;

namespace Cirrious.MonoCross.Extensions.Interfaces
{
    public interface IMXViewDispatcher : IMXCrossThreadDispatcher
    {
        bool RequestNavigate(string url, Dictionary<string, string> parameters);
        // TODO - maybe other things (timers? services for GPS, etc?)
    }
}