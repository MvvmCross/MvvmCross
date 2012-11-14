using System;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoViewTextLoader
    {
        bool HasDefinition(Type viewModelType, string key);
        string GetDefinition(Type viewModelType, string key);
    }
}