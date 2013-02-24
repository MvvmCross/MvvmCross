// MvxDroidUserInterfaceFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.Binding.Droid.Views;
using CrossUI.Core.Builder;
using CrossUI.Core.Descriptions;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxDroidUserInterfaceFactory
        : IMvxUserInterfaceFactory
          , IMvxConsumer
    {
        public TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description)
        {
            var bindingActivity = view as IMvxBindingContextOwner;
            if (bindingActivity == null)
                throw new MvxException(
                    "Activity passed to MvxDroidUserInterfaceFactory must be an IMvxBindingContext - type {0}",
                    view.GetType().Name);

            var registry = this.Resolve<IBuilderRegistry>();
            var builder = new MvxDroidUserInterfaceBuilder(bindingActivity.BindingContext, view.ViewModel, registry);
            var root = (TResult) builder.Build(typeof (TBuildable), description);
            return root;
        }
    }
}