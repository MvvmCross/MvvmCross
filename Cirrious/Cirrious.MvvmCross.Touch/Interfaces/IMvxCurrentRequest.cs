using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
    public interface IMvxCurrentRequest
    {
        MvxShowViewModelRequest CurrentRequest { get;  }
    }
}