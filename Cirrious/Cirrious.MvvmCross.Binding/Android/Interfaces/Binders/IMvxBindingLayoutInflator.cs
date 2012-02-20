using System;
using Android.Views;

namespace Cirrious.MvvmCross.Binding.Android.Interfaces.Binders
{
    public interface IMvxBindingLayoutInflator : IDisposable
    {
        View Inflate(int resource, ViewGroup id);
    }
}