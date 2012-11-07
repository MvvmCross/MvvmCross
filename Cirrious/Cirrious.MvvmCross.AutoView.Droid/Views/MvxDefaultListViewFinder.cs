using System;
using Cirrious.MvvmCross.AutoView.Droid.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Droid.Views
{
    public class MvxDefaultListViewFinder : IMvxViewFinder, IMvxServiceConsumer
    {
        public Type ListViewType { get; set; }

        public MvxDefaultListViewFinder()
        {
            ListViewType = typeof(MvxDefaultListActivityView);
        }

        public Type GetViewType(Type viewModelType)
        {
            var loader = this.GetService<IMvxDefaultViewTextLoader>();
            if (loader.HasDefinition(viewModelType.Name, MvxDefaultViewConstants.List))
            {
                return ListViewType;
            }

            return null;
        }
    }
}