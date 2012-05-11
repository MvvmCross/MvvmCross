namespace Cirrious.MvvmCross.Binding.Android.Views
{
    public interface IMvxBindableAutoCompleteTextView
    {
        int TemplateId { get; }
        void BindTo(object source);
    }
}