using System;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content.Res;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using CustomerManagement.Droid.Views;

namespace CustomerManagement.Droid
{
    public class MvxDefaultDialogViewFinder : IMvxViewFinder, IMvxServiceConsumer
    {
        public Type DialogViewType { get; set; }

        public MvxDefaultDialogViewFinder()
        {
            DialogViewType = typeof (MvxDefaultDialogActivityView);
        }

        public Type GetViewType(Type viewModelType)
        {
            var loader = this.GetService<IMvxDefaultViewTextLoader>();
            if (loader.HasDefinition(viewModelType.Name, MvxDefaultViewConstants.Dialog))
            {
                return DialogViewType;
            }

            return null;
        }
    }
}