using Android.App;
using Android.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.ViewModels;
using FooBar.Dialog.Droid.Menus;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Menus;
using FooBar.Dialog.Droid;

namespace CustomerManagement.Droid.Views
{
    [Activity]
    public class MvxDefaultDialogActivityView : MvxBindingDialogActivityView<MvxViewModel>
    {
        private IParentMenu _parentMenu;

        protected override void OnViewModelSet()
        {
            LoadDialogRoot();
            _parentMenu = LoadMenu();
        }

        protected virtual IParentMenu LoadMenu()
        {
            var jsonText = GetJsonText(MvxDefaultViewConstants.Menu);
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }

            var description = Newtonsoft.Json.JsonConvert.DeserializeObject<ParentMenuDescription>(jsonText);
            var builder = new MvxDroidMenuBuilder(this, ViewModel);
            var root = builder.Build(description) as IParentMenu;
            return root;
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

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            if (_parentMenu == null)
            {
                return false;
            }

#warning TODO - make this OO - let the _parentMenu render itself...
            foreach (var child in _parentMenu.Children)
            {
                var childCast = child as CaptionAndIconMenu;

                var resourceId = (int) typeof (Resource.Drawable).GetField(childCast.Icon).GetValue(null);
                menu.Add(1, childCast.UniqueId, 0, childCast.Caption).SetIcon(resourceId);
            }
            //MenuInflater.Inflate(Resource.Menu.customer_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
#warning TODO - make this OO - let the _parentMenu respond to commands itself...
            foreach (var child in _parentMenu.Children)
            {
                var childCast = child as CaptionAndIconMenu;
                if (childCast.UniqueId == item.ItemId)
                {
                    childCast.Command.Execute(null);
                    return true;
                }
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}