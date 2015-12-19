// MvxAutoDialogViewFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Views
{
    using System;

    using MvvmCross.AutoView.Droid.Views.Dialog;
    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;

    public class MvxAutoDialogViewFinder : IMvxViewFinder
    {
        public static Type DialogViewType { get; set; }

        public MvxAutoDialogViewFinder()
        {
            if (DialogViewType == null)
                DialogViewType = typeof(MvxAutoDialogActivity);
        }

        public Type GetViewType(Type viewModelType)
        {
            // best of a bad bunch - http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (viewModelType.GetInterface(typeof(IMvxAutoDialogViewModel).FullName) != null)
            {
                return DialogViewType;
            }

            var loader = Mvx.Resolve<IMvxAutoViewTextLoader>();
            if (loader.HasDefinition(viewModelType, MvxAutoViewConstants.Dialog))
            {
                return DialogViewType;
            }

            return null;
        }
    }
}