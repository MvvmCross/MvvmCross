using Android.App;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.ViewModels;
using Foobar.Dialog.Core.Descriptions;
using FooBar.Dialog.Droid;

namespace CustomerManagement.Droid.Views
{
    [Activity]
    public class MvxDefaultDialogActivityView : MvxBindingDialogActivityView<MvxViewModel>
    {
        protected override void OnViewModelSet()
        {
            LoadDialogRoot();
            LoadMenu();
        }

        protected virtual void LoadMenu()
        {
            /*
            var jsonText = GetJsonText(MvxDefaultViewConstants.Dialog);
            var description = Newtonsoft.Json.JsonConvert.DeserializeObject<MenuDescription>(jsonText);
            var builder = new MvxDroidMenuBuilder(this, ViewModel);
            var root = builder.Build(description) as menu;
            return root;
             */
        }

        protected virtual void LoadDialogRoot()
        {
            var root = BuildDialogRoot();
            Root = root;
        }

        protected virtual RootElement BuildDialogRoot()
        {
            var jsonText = GetJsonText(MvxDefaultViewConstants.Dialog);
            var description = Newtonsoft.Json.JsonConvert.DeserializeObject<ElementDescription>(jsonText);
            var builder = new MvxDroidElementBuilder(this, ViewModel);
            var root = builder.Build(description) as RootElement;
            return root;
        }

        protected virtual string GetJsonText(string key)
        {
            var typeName = ViewModel.GetType().Name;
            var defaultViewTextLoader = this.GetService<IMvxDefaultViewTextLoader>();
            var json = defaultViewTextLoader.GetDefinition(typeName, key);
            return json;
        }
    }
}