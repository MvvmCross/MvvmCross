using System;
using Android.Content;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Droid.Interfaces
{
    public interface IMvxAndroidViewModelLoader
    {
        IMvxViewModel Load(Intent intent);
        IMvxViewModel Load(Intent intent, Type viewModelTypeHint);
    }
}