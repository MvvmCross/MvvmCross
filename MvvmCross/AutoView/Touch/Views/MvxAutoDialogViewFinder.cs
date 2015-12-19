// MvxAutoDialogViewFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Views
{
    using System;

    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.AutoView.Touch.Views.Dialog;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;

    public class MvxAutoDialogViewFinder : IMvxViewFinder
    {
        public Type DialogViewType { get; set; }

        public MvxAutoDialogViewFinder()
        {
            this.DialogViewType = typeof(MvxAutoDialogTouchView);
        }

        public Type GetViewType(Type viewModelType)
        {
            // best of a bad bunch - http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (viewModelType.GetInterface(typeof(IMvxAutoDialogViewModel).FullName) != null)
            {
                return this.DialogViewType;
            }

            var loader = Mvx.Resolve<IMvxAutoViewTextLoader>();
            if (loader.HasDefinition(viewModelType, MvxAutoViewConstants.Dialog))
            {
                return this.DialogViewType;
            }

            return null;
        }
    }
}