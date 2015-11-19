// MvxPhoneLifetimeMonitor.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Platform;
using Microsoft.Phone.Shell;

namespace Cirrious.MvvmCross.WindowsPhone.Platform
{
    public class MvxPhoneLifetimeMonitor : MvxLifetimeMonitor
    {
        public MvxPhoneLifetimeMonitor()
        {
            PhoneApplicationService.Current.Activated += (s, e) =>
                {
                    FireLifetimeChange(
                        e.IsApplicationInstancePreserved
                            ? MvxLifetimeEvent.ActivatedFromMemory
                            : MvxLifetimeEvent.ActivatedFromDisk);
                };
            PhoneApplicationService.Current.Closing += (s, e) => FireLifetimeChange(MvxLifetimeEvent.Closing);
            PhoneApplicationService.Current.Deactivated += (s, e) => FireLifetimeChange(MvxLifetimeEvent.Deactivated);
            PhoneApplicationService.Current.Launching += (s, e) => FireLifetimeChange(MvxLifetimeEvent.Launching);
        }
    }
}