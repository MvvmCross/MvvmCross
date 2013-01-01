// MvxAutoDialogViewFinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.AutoView.Touch.Views.Dialog;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.AutoView.Touch.Views
{
    public class MvxAutoDialogViewFinder : IMvxViewFinder, IMvxServiceConsumer
    {
        public Type DialogViewType { get; set; }

        public MvxAutoDialogViewFinder()
        {
            DialogViewType = typeof (MvxAutoDialogTouchView);
        }

        public Type GetViewType(Type viewModelType)
        {
            // best of a bad bunch - http://www.hanselman.com/blog/DoesATypeImplementAnInterface.aspx
            if (viewModelType.GetInterface(typeof (IMvxAutoDialogViewModel).FullName) != null)
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