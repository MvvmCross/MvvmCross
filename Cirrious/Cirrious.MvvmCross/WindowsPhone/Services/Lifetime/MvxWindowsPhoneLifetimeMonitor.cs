using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.MvvmCross.Interfaces.Services.Lifetime;
using Cirrious.MvvmCross.Platform.Lifetime;
using Microsoft.Phone.Shell;

namespace Cirrious.MvvmCross.WindowsPhone.Services.Lifetime
{
    public class MvxWindowsPhoneLifetimeMonitor : MvxBaseLifetimeMonitor
    {
        public MvxWindowsPhoneLifetimeMonitor()
        {
            PhoneApplicationService.Current.Activated += (s, e) =>
                                                             {
                                                                 if (e.IsApplicationInstancePreserved)
                                                                     FireLifetimeChange(
                                                                         MvxLifetimeEvent.ActivatedFromMemory);
                                                                 else
                                                                     FireLifetimeChange(
                                                                         MvxLifetimeEvent.ActivatedFromDisk);
                                                             };
            PhoneApplicationService.Current.Closing += (s, e) => FireLifetimeChange(MvxLifetimeEvent.Closing);
            PhoneApplicationService.Current.Deactivated += (s, e) => FireLifetimeChange(MvxLifetimeEvent.Deactivated);
            PhoneApplicationService.Current.Launching += (s, e) => FireLifetimeChange(MvxLifetimeEvent.Launching);
        }
    }
}
