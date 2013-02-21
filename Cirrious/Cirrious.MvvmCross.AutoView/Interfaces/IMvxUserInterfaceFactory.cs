using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Interfaces
{
    public interface IMvxUserInterfaceFactory
    {
        TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description);
    }
}