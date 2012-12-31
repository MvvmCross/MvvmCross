﻿#region Copyright

// <copyright file="MvxWinRTResourceLoader.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion


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