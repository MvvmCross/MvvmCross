// IMvxWindowsPhoneViewModelRequestTranslator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.WindowsPhone.Interfaces
{
    public interface IMvxWindowsPhoneViewModelRequestTranslator
    {
        MvxShowViewModelRequest GetRequestFromXamlUri(Uri viewUri);
        Uri GetXamlUriFor(MvxShowViewModelRequest request);
    }
}