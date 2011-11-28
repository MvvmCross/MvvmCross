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