namespace MvvmCross.iOS.Views
{
    public interface IMvxTabBarItemViewController
    {
        string TabName { get; }
        string TabIconName { get; }
        
        string TabSelectedIconName { get;  }
    }
}
