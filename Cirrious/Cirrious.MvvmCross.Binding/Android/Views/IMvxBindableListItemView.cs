namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public interface IMvxBindableListItemView
    {
        int TemplateId { get; }
        void BindTo(object source);
    }
}