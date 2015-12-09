// MvxAndroidUserInterfaceFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Builders
{
    using CrossUI.Core.Builder;
    using CrossUI.Core.Descriptions;

    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;

    public class MvxAndroidUserInterfaceFactory
        : IMvxUserInterfaceFactory

    {
        public TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description)
        {
            var bindingActivity = view as IMvxBindingContextOwner;
            if (bindingActivity == null)
                throw new MvxException(
                    "Activity passed to MvxAndroidUserInterfaceFactory must be an IMvxAndroidBindingContext - type {0}",
                    view.GetType().Name);

            var registry = Mvx.Resolve<IBuilderRegistry>();
            var builder = new MvxAndroidUserInterfaceBuilder((IMvxAndroidBindingContext)bindingActivity.BindingContext,
                                                           view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }
    }
}