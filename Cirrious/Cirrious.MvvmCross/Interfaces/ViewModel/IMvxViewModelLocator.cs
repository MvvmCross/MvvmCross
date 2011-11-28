using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Interfaces.ViewModel
{
    public interface IMvxViewModelLocator
    {
        string DefaultAction { get; }
        Type ViewModelType { get; }
        bool TryLoad(string perspective, IDictionary<string, string> parameters, out IMvxViewModel model);
    }

    public interface IMvxViewModelLocator<TViewModel> : IMvxViewModelLocator where TViewModel : IMvxViewModel
    {
        bool TryLoad(string perspective, IDictionary<string, string> parameters, out TViewModel model);
    }
}