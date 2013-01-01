// MvxWindowsPhoneLifetimeMonitor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Platform.Lifetime;
using Microsoft.Phone.Shell;

namespace Cirrious.MvvmCross.WindowsPhone.Platform.Lifetime
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