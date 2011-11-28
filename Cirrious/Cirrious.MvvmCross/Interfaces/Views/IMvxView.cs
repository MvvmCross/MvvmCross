using System;

namespace Cirrious.MvvmCross.Interfaces.Views
{
    public interface IMvxView
    {
        Type ModelType { get; }
        void SetModel(object model);

        void Render();
    }
}