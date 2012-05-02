#region Copyright
// <copyright file="MvxWindowsPhoneLifetimeMonitor.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

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
