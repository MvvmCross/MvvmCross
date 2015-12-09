// IMvxPhoneViewModelRequestTranslator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;
using System;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    public interface IMvxPhoneViewModelRequestTranslator
    {
        MvxViewModelRequest GetRequestFromXamlUri(Uri viewUri);

        Uri GetXamlUriFor(MvxViewModelRequest request);
    }
}