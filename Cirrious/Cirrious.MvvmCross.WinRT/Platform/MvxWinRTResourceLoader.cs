// MvxWinRTResourceLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#warning Do we need a resource loader in WinRT? If we do, then it should move to the plugin (of course)
#if false
namespace Cirrious.MvvmCross.WinRT.Platform
{
    public class MvxWindowsPhoneResourceLoader : MvxBaseResourceLoader
    {
        #region Implementation of IMvxResourceLoader

        public override void GetResourceStream(string resourcePath, Action<Stream> streamAction)
        {
#warning ? need to check and clarify what exceptions can be thrown here!
            throw new NotImplementedException("?");
        }

        #endregion
    }
}
#endif