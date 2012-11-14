using System;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.AutoView.Droid.Views.Dialog;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
{
    public class MvxAutoDialogViewFinder : IMvxViewFinder, IMvxServiceConsumer
    {
        public Type DialogViewType { get; set; }

        public MvxAutoDialogViewFinder()
        {
            DialogViewType = typeof (MvxAutoDialogActivityView);
        }

        public Type GetViewType(Type viewModelType)
        {
            // best of a bad bunch - http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (viewModelType.GetInterface(typeof(IMvxAutoDialogViewModel).FullName) != null)
            {
                return DialogViewType;
            }

            var loader = this.GetService<IMvxAutoViewTextLoader>();
            if (loader.HasDefinition(viewModelType, MvxAutoViewConstants.Dialog))
            {
                return DialogViewType;
            }

            return null;
        }
    }    
}