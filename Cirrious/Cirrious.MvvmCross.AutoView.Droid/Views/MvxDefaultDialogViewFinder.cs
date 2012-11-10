using System;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.Droid.Views.Dialog;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
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