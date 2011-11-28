using System;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxViewsContainer
    {
        void Add(MxvViewModelAction viewModelAction, Type viewType);
        void Add<TViewModel>(string actionName, Type viewType) where TViewModel : IMvxViewModel;
        void Add<TViewModel>(Type viewType) where TViewModel : IMvxViewModel;
        bool ContainsKey(MxvViewModelAction viewModelAction);
        Type GetViewType(MxvViewModelAction viewModelAction);
    }
}