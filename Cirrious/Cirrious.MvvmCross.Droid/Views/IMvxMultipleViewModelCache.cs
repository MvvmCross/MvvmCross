using System;
using Cirrious.MvvmCross.ViewModels;
using Java.Net;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IMvxMultipleViewModelCache
    {
        void Cache(IMvxViewModel toCache, string viewModelTag = "singleInstanceCache");
        IMvxViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache");
        T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : IMvxViewModel;
    }
}