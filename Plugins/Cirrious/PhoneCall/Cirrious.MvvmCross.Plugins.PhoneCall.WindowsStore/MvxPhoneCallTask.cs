// MvxPhoneCallTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.System;

namespace Cirrious.MvvmCross.Plugins.PhoneCall.WindowsStore
{
    public class MvxPhoneCallTask : IMvxPhoneCallTask
    {
        public void MakePhoneCall(string name, string number)
        {
            // TODO! This is far too skype specific 
            // TODO! name/number need looking at
            // this is the best I can do so far... 
            var uri = new Uri("skype:" + number + "?call", UriKind.Absolute);
            Launcher.LaunchUriAsync(uri);
        }
    }
}