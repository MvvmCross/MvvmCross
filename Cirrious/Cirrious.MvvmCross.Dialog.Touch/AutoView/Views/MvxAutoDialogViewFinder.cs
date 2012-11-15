using System;
using Cirrious.MvvmCross.AutoView;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch.AutoView.Views.Dialog;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Views
{
    public class MvxAutoDialogViewFinder : IMvxViewFinder, IMvxServiceConsumer
    {
        public Type DialogViewType { get; set; }

        public MvxAutoDialogViewFinder()
        {
            DialogViewType = typeof(MvxAutoDialogTouchView);
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