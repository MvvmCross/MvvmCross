// IMvxPhoneViewModelRequestTranslator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Views
{
    using System;

    using MvvmCross.Core.ViewModels;

    public interface IMvxPhoneViewModelRequestTranslator
    {
        MvxViewModelRequest GetRequestFromXamlUri(Uri viewUri);

        Uri GetXamlUriFor(MvxViewModelRequest request);
    }
}