namespace MvvmCross.Droid.Views
{
    using System;

    using MvvmCross.Core.ViewModels;

    public interface IMvxMultipleViewModelCache
    {
        void Cache(IMvxViewModel toCache, string viewModelTag = "singleInstanceCache");

        IMvxViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache");

        T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : IMvxViewModel;
    }
}