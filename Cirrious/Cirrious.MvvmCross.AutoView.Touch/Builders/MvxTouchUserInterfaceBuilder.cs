// MvxTouchUserInterfaceBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.AutoView.Interfaces;
using Cirrious.MvvmCross.AutoView.Touch.Interfaces;
using CrossUI.Core.Builder;
using CrossUI.Core.Descriptions;
using CrossUI.Touch.Builder;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders
{
    public class MvxTouchUserInterfaceFactory
        : IMvxUserInterfaceFactory
    {
        public TResult Build<TBuildable, TResult>(IMvxAutoView view, KeyedDescription description)
        {
            var bindingViewController = view as IMvxBindingViewController;

            if (bindingViewController == null)
                throw new MvxException("View passed to MvxTouchUserInterfaceFactory must be an IMvxBindingViewController - type {0}", view.GetType().Name);
            
            var registry = view.GetService<IBuilderRegistry>();
            var builder = new MvxTouchUserInterfaceBuilder(bindingViewController, view.ViewModel, registry);
            var root = (TResult)builder.Build(typeof(TBuildable), description);
            return root;
        }
    }
    public class MvxTouchUserInterfaceBuilder
        : TouchUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxTouchUserInterfaceBuilder(IMvxBindingViewController activity, object dataSource,
                                            IBuilderRegistry builderRegistry,
                                            string bindTag = MvxAutoViewConstants.MvxBindTag,
                                            string platformName = TouchConstants.PlatformName)
            : base(builderRegistry, platformName)
        {
            _propertyBuilder = new PropertyBuilder();
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            _propertyBuilder.CustomPropertySetters[bindTag] = setter;
        }

        protected override IPropertyBuilder PropertyBuilder
        {
            get { return _propertyBuilder; }
        }
    }
}