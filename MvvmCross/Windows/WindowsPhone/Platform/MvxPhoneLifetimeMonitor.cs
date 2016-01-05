// MvxPhoneLifetimeMonitor.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Platform
{
    using Microsoft.Phone.Shell;

    using MvvmCross.Core.Platform;

    public class MvxPhoneLifetimeMonitor : MvxLifetimeMonitor
    {
        public MvxPhoneLifetimeMonitor()
        {
            PhoneApplicationService.Current.Activated += (s, e) =>
                {
                    this.FireLifetimeChange(
                        e.IsApplicationInstancePreserved
                            ? MvxLifetimeEvent.ActivatedFromMemory
                            : MvxLifetimeEvent.ActivatedFromDisk);
                };
            PhoneApplicationService.Current.Closing += (s, e) => this.FireLifetimeChange(MvxLifetimeEvent.Closing);
            PhoneApplicationService.Current.Deactivated += (s, e) => this.FireLifetimeChange(MvxLifetimeEvent.Deactivated);
            PhoneApplicationService.Current.Launching += (s, e) => this.FireLifetimeChange(MvxLifetimeEvent.Launching);
        }
    }
}