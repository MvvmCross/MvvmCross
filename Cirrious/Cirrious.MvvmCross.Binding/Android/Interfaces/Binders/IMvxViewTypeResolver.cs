using System;

namespace Cirrious.MvvmCross.Binding.Android.Interfaces.Binders
{
    public interface IMvxViewTypeResolver
    {
        Type Resolve(string tagName);
    }
}