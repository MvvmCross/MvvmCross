using Cirrious.Conference.Core;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Microsoft.Phone.Controls;

namespace Cirrious.Conference.UI.WP7
{
    public class Setup
        : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) 
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new ConferenceApp();
        }
    }
}
