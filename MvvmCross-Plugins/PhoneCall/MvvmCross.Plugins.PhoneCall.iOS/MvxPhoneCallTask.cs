// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Foundation;
using MvvmCross.Platform.iOS.Platform;

namespace MvvmCross.Plugins.PhoneCall.iOS
{
    [Preserve(AllMembers = true)]
	public class MvxPhoneCallTask : MvxIosTask, IMvxPhoneCallTask
    {
        public void MakePhoneCall(string name, string number)
        {            
            var url = new NSUrl("tel:" + Uri.EscapeUriString(number));
            DoUrlOpen(url);
        }
    }
}