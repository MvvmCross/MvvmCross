// MvxAutoListViewFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.AutoView.Touch.Views.Lists;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    public class MvxAutoListViewFinder : IMvxViewFinder, IMvxServiceConsumer
    {
        public Type ListViewType { get; set; }

        public MvxAutoListViewFinder()
        {
            ListViewType = typeof (MvxAutoListActivityView);
        }

        public Type GetViewType(Type viewModelType)
        {
            // best of a bad bunch - http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (viewModelType.GetInterface(typeof (IMvxAutoListViewModel).FullName) != null)
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