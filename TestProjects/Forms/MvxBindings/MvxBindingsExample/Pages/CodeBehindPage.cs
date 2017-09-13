using MvvmCross.Forms.Views;
using Xamarin.Forms;
using MvxBindingsExample.ViewModels;
using MvvmCross.Binding.BindingContext;

namespace MvxBindingsExample.Pages
{
    public class CodeBehindPage : MvxContentPage<CodeBehindViewModel>
    {
        private Label _label = new Label();
        private Entry _entry = new Entry();
        public CodeBehindPage()
        {
            Content = new StackLayout
            {
                Children = {
                    _label,
                    _entry
                }
            };

            var set = this.CreateBindingSet<CodeBehindPage, CodeBehindViewModel>();
            set.Bind(_label).For(l => l.Text).To(vm => vm.BindableText);
            set.Bind(_entry).For(e => e.Text).To(vm => vm.BindableText);
            set.Apply();
        }
    }
}