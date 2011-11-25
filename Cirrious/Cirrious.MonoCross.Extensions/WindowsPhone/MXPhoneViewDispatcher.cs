using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;
using Cirrious.MonoCross.Extensions.Interfaces;
using MonoCross.Navigation;

namespace Cirrious.MonoCross.Extensions.WindowsPhone
{
    public class MXPhoneViewDispatcher : MxCrossThreadDispatcher, IMXViewDispatcher
    {
        public MXPhoneViewDispatcher(Dispatcher uiDispatcher)
            : base(uiDispatcher)
        {
        }

        public bool RequestNavigate(string url, Dictionary<string, string> parameters)
        {
            return InvokeOrBeginInvoke(() =>
                                           {                
                                               // note that we are passing a null "fromView" into this method
                                               // this is a shame - as it looks ugly - but seemed unavoidable...
                                               // is this fromView parameter really needed?
                                               MXContainer.Navigate(null, url, parameters);
                                           });
        }
    }
}