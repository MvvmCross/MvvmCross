// MvxTouchUserInterfaceFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Touch.Views;
using CrossUI.Core.Builder;
using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders
{
    public class MvxTouchUserInterfaceFactory
        : IMvxUserInterfaceFactory
    {
        public TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description)
        {
            var bindingViewController = view as IMvxTouchView;

            if (bindingViewController == null)
                throw new MvxException(
                    "View passed to MvxTouchUserInterfaceFactory must be an IMvxBindingViewController - type {0}",
                    view.GetType().Name);

            var registry = Mvx.Resolve<IBuilderRegistry>();
            var builder = new MvxTouchUserInterfaceBuilder(bindingViewController, view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }
    }
}