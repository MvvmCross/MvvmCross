using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.Dialog.Droid.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CrossUI.Core.Builder;
using CrossUI.Core.Descriptions.Dialog;
using CrossUI.Core.Elements.Dialog;
using FooBar.Dialog.Droid;
using FooBar.Dialog.Droid.Elements;

namespace CustomerManagement.Droid.Views
{
    public class BaseDialogView<TViewModel> : MvxBindingDialogActivityView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected override void OnViewModelSet()
        {
            //SetContentView(Resource.Layout.Page_DetailsCustomerView);
            var description = Newtonsoft.Json.JsonConvert.DeserializeObject<ElementDescription>(JsonText);
            var registry = this.GetService<IBuilderRegistry>();
            var builder = new MvxDroidUserInterfaceBuilder(this, ViewModel, registry);
            var root = builder.Build(typeof(IElement), description) as RootElement;
            Root = root;
        }

        protected virtual string JsonText
        {
            get { return "Override this!"; }
        }
    }
}