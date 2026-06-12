// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Platforms.Uap.Binding.MvxBinding;
using MvvmCross.Platforms.Uap.Binding.MvxBinding.Target;
using Windows.UI.Xaml;

namespace MvvmCross.Platforms.Uap.Binding
{
    public class MvxWindowsBindingBuilder : MvxBindingBuilder
    {
        public enum BindingType
        {
            Windows,
            MvvmCross
        }

        private readonly BindingType _bindingType;

        public MvxWindowsBindingBuilder(
            BindingType bindingType = BindingType.MvvmCross)
        {
            _bindingType = bindingType;
        }

        public override void DoRegistration(IServiceCollection services)
        {
            base.DoRegistration(services);
            RegisterBindingCreator(services);
        }

        protected override void RegisterBindingFactories(IServiceCollection services)
        {
            switch (_bindingType)
            {
                case BindingType.Windows:
                    // no need for MvvmCross binding factories - so don't create them
                    break;

                case BindingType.MvvmCross:
                    base.RegisterBindingFactories(services);
                    break;

                default:
                    throw new InvalidOperationException($"Unable to register binding factories for BindingType: {_bindingType}");
            }
        }

        protected override IMvxTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            switch (_bindingType)
            {
                case BindingType.Windows:
                    return base.CreateTargetBindingRegistry();

                case BindingType.MvvmCross:
                    return new MvxWindowsTargetBindingFactoryRegistry();

                default:
                    throw new InvalidOperationException($"Unable to create target binding registry for BindingType: {_bindingType}");
            }
        }

        private void RegisterBindingCreator(IServiceCollection services)
        {
            var creator = CreateBindingCreator();
            services.TryAddSingleton<IMvxBindingCreator>(_ => creator);
        }

        protected virtual IMvxBindingCreator CreateBindingCreator()
        {
            switch (_bindingType)
            {
                case BindingType.Windows:
                    return new MvxWindowsBindingCreator();

                case BindingType.MvvmCross:
                    return new MvxMvvmCrossBindingCreator();

                default:
                    throw new InvalidOperationException($"Unable to create binding creator for BindingType: {_bindingType}");
            }
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            if (MvxWindowsAssemblyCache.Instance != null)
            {
                foreach (var assembly in MvxWindowsAssemblyCache.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }
        }

        protected override void FillValueCombiners(IMvxValueCombinerRegistry registry)
        {
            base.FillValueCombiners(registry);

            if (MvxWindowsAssemblyCache.Instance != null)
            {
                foreach (var assembly in MvxWindowsAssemblyCache.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<FrameworkElement>(
                MvxWindowsPropertyBinding.FrameworkElement_Visible,
                view => new MvxVisibleTargetBinding(view));

            registry.RegisterCustomBindingFactory<FrameworkElement>(
                MvxWindowsPropertyBinding.FrameworkElement_Collapsed,
                view => new MvxCollapsedTargetBinding(view));

            registry.RegisterCustomBindingFactory<FrameworkElement>(
                MvxWindowsPropertyBinding.FrameworkElement_Hidden,
                view => new MvxCollapsedTargetBinding(view));

            base.FillTargetFactories(registry);
        }
    }
}
