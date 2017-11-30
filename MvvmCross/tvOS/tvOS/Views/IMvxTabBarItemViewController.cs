using System;
namespace MvvmCross.tvOS.Views
{
    public interface IMvxTabBarItemViewController
    {
        string TabName { get; }
        string TabIconName { get; }

        string TabSelectedIconName { get; }
    }
}
