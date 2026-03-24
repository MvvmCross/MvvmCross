// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Hosting;
using MvvmCross.Platforms.Wpf.Binding.MvxBinding;
using MvvmCross.Platforms.Wpf.Binding.MvxBinding.Target;
using MvvmCross.Platforms.Wpf.Binding.WindowsBinding;

namespace MvvmCross.Platforms.Wpf.Binding
{
    public class MvxWindowsBindingBuilder : MvxBindingBuilder
    {
        public enum BindingType
        {
            Windows,
            MvvmCross
        }

        private readonly BindingType _bindingType;
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillTargetFactories;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNames;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConverters;
        private readonly Action<IMvxValueCombinerRegistry> _fillValueCombiners;

        public MvxWindowsBindingBuilder(
            Action<IMvxTargetBindingFactoryRegistry> fillTargetFactories = null,
            Action<IMvxBindingNameRegistry> fillBindingNames = null,
            Action<IMvxValueConverterRegistry> fillValueConverters = null,
            Action<IMvxValueCombinerRegistry> fillValueCombiners = null,
            BindingType bindingType = BindingType.MvvmCross)
        {
            _fillTargetFactories = fillTargetFactories;
            _fillBindingNames = fillBindingNames;
            _fillValueConverters = fillValueConverters;
            _fillValueCombiners = fillValueCombiners;
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
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RegisterBindingCreator(IServiceCollection services)
        {
            var creator = CreateBindingCreator();
            services.TryAddSingleton<IMvxBindingCreator>(_ => creator);
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
                    throw new ArgumentOutOfRangeException();
            }
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
                    throw new ArgumentOutOfRangeException();
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

            _fillValueConverters?.Invoke(registry);
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

            _fillValueCombiners?.Invoke(registry);
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

            _fillTargetFactories?.Invoke(registry);
        }

        protected override void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);
            _fillBindingNames?.Invoke(registry);
        }
    }
}
