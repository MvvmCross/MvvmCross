using System;
using Cirrious.MvvmCross.AutoView;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch.AutoView.Views.Lists;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Views
{
    public class MvxAutoListViewFinder : IMvxViewFinder, IMvxServiceConsumer
    {
        public Type ListViewType { get; set; }

        public MvxAutoListViewFinder()
        {
            ListViewType = typeof(MvxAutoListActivityView);
        }

        public Type GetViewType(Type viewModelType)
        {
            // best of a bad bunch - http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (viewModelType.GetInterface(typeof(IMvxAutoListViewModel).FullName) != null)
            {
                return ListViewType;
            }

            var loader = this.GetService<IMvxAutoViewTextLoader>();
            if (loader.HasDefinition(viewModelType, MvxAutoViewConstants.List))
            {
                return ListViewType;
            }

            return null;
        }
    }
}