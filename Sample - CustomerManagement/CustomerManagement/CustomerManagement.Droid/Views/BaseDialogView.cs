using Cirrious.MvvmCross.Interfaces.ViewModels;
using FooBar.Dialog.Droid;
using Foobar.Dialog.Core.Descriptions;

namespace CustomerManagement.Droid.Views
{
    public class BaseDialogView<TViewModel> : MvxBindingDialogActivityView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected override void OnViewModelSet()
        {
            //SetContentView(Resource.Layout.Page_DetailsCustomerView);
            var description = Newtonsoft.Json.JsonConvert.DeserializeObject<ElementDescription>(JsonText);
            var builder = new MvxDroidElementBuilder(this, ViewModel);
            Root = builder.Build(description) as RootElement;
        }

        protected virtual string JsonText
        {
            get { return "Override this!"; }
        }
    }
}