// MvxIosUserInterfaceFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders
{
    using CrossUI.Core.Builder;
    using CrossUI.Core.Descriptions;

    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.iOS.Views;

    public class MvxIosUserInterfaceFactory
        : IMvxUserInterfaceFactory
    {
        public TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description)
        {
            var bindingViewController = view as IMvxIosView;

            if (bindingViewController == null)
                throw new MvxException(
                    "View passed to MvxIosUserInterfaceFactory must be an IMvxBindingViewController - type {0}",
                    view.GetType().Name);

            var registry = Mvx.Resolve<IBuilderRegistry>();
            var builder = new MvxIosUserInterfaceBuilder(bindingViewController, view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }
    }
}