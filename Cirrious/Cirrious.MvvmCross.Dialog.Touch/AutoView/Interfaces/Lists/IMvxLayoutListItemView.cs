namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Interfaces.Lists
{
    public interface IMvxLayoutListItemView
    {
        string UniqueName { get; }
        void BindTo(object source);
    }
}