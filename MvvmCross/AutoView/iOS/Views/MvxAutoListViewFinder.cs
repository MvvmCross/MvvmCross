// MvxAutoListViewFinder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.AutoView.iOS.Views.Lists;

namespace MvvmCross.AutoView.iOS.Views
{
    using System;

    using MvvmCross.AutoView.Interfaces;
    using iOS.Views.Lists;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;

    public class MvxAutoListViewFinder : IMvxViewFinder
    {
        public Type ListViewType { get; set; }

        public MvxAutoListViewFinder()
        {
            this.ListViewType = typeof(MvxAutoListActivityView);
        }

        public Type GetViewType(Type viewModelType)
        {
            // best of a bad bunch - http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (viewModelType.GetInterface(typeof(IMvxAutoListViewModel).FullName) != null)
            {
                return this.ListViewType;
            }

            var loader = Mvx.Resolve<IMvxAutoViewTextLoader>();
            if (loader.HasDefinition(viewModelType, MvxAutoViewConstants.List))
            {
                return this.ListViewType;
            }

            return null;
        }
    }
}