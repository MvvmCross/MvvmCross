using System.Collections.Generic;
using Android.App;
using Android.Views;
using Cirrious.MvvmCross.AutoView.Droid.Builders;
using Cirrious.MvvmCross.AutoView.Droid.Builders.Lists;
using Cirrious.MvvmCross.AutoView.Droid.ExtensionMethods;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views.Attributes;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Lists;
using Foobar.Dialog.Core.Menus;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
{
    [Activity]
    [MvxUnconventionalView]
    public class MvxDefaultListActivityView
        : MvxBindingActivityView<MvxViewModel>
        , IMvxDefaultAndroidView<MvxViewModel>
    {
        private IParentMenu _parentMenu;
        private GeneralListLayout _list;

        protected override void OnViewModelSet()
        {
            _parentMenu = this.LoadMenu();

            _list = LoadList(this);
            var listView = _list.InitialiseListView(this);
            this.SetContentView(listView);
            RegisterBindingsFor(listView);
        }

#warning consider making static - and moving to extension method?
        private GeneralListLayout LoadList<TViewModel>(IMvxDefaultAndroidView<TViewModel> view)
            where TViewModel : class, IMvxViewModel
        {
            var jsonText = view.GetJsonText(MvxDefaultViewConstants.List);
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }

            var jsonService = MvxServiceProviderExtensions.GetService<IMvxJsonConverter>(view);
            var description = jsonService.DeserializeObject<ListLayoutDescription>(jsonText);
            HackFlattenJObjectsToStringDictionary(description);
            var builder = new MvxNewUserInterfaceBuilder(view, view.ViewModel);
            var root = builder.Build(typeof(IListLayout), description) as GeneralListLayout;
            return root;
        }

        private void HackFlattenJObjectsToStringDictionary(ListLayoutDescription description)
        {
            if (description.ItemLayouts != null)
            {
                foreach (var layout in description.ItemLayouts)
                {
                    HackFlattenJObjectsToStringDictionary(layout.Value);
                }
            }
            HackFlattenJObjectsToStringDictionary((KeyedDescription) description);
        }

        private void HackFlattenJObjectsToStringDictionary(KeyedDescription description)
        {
            foreach (var propertyInfo in description.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(description, null);
                var keyedDescription = value as KeyedDescription;
                if (keyedDescription != null)
                {
                    HackFlattenJObjectsToStringDictionary(keyedDescription);
                }
            }

            var flattener = this.GetService<IMvxJsonFlattener>();
            var listToUpdate = new List<KeyValuePair<string, object>>();
            foreach (var prop in description.Properties)
            {
                if (flattener.IsJsonObject(prop.Value))
                {
                    listToUpdate.Add(prop);
                }
            }

            foreach (var keyValuePair in listToUpdate)
            {
                var value = keyValuePair.Value;
                var dict = flattener.FlattenJsonObjectToStringDictionary(value);
                description.Properties[keyValuePair.Key] = dict;
            }
        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            return this.CreateOptionsMenu(_parentMenu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_parentMenu.ProcessMenuItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}