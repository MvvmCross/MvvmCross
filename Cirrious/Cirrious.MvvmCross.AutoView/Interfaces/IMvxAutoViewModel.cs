using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxAutoViewModel
    {
        bool SupportsAutoView(string type);
        KeyedDescription GetAutoView(string type);
    }

    public interface IMvxAutoDialogViewModel : IMvxAutoViewModel
    {
    }

    public interface IMvxAutoListViewModel : IMvxAutoViewModel
    {
    }
}