using System;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Silverlight.Views
{
    public interface IMvxSilverlightViewModelRequestTranslator
    {
        MvxViewModelRequest GetRequestFromXamlUri(Uri viewUri);
        Uri GetXamlUriFor(MvxViewModelRequest request);
    }
}