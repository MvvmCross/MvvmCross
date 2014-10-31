using System;
using Cirrious.MvvmCross.ViewModels;
using Java.Net;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IMvxMultipleViewModelCache
    {
        void Cache(IMvxViewModel toCache);
        IMvxViewModel GetAndClear(Type viewModelType);
        T GetAndClear<T>() where T : IMvxViewModel;
    }
}