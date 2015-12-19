﻿// MvxWindowsBindingBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if WINDOWS_PHONE || WINDOWS_WPF
#endif

#if NETFX_CORE

using Windows.UI.Xaml;

#endif


namespace MvvmCross.BindingEx.WindowsPhone

{
    using System;
    using System.Windows;

    using global::MvvmCross.Binding;
    using global::MvvmCross.Binding.Binders;
    using global::MvvmCross.Binding.Bindings.Target.Construction;
    using global::MvvmCross.Platform;
    using global::MvvmCross.Platform.Converters;
    using global::MvvmCross.Platform.Core;

    using MvvmCross.BindingEx.WindowsPhone.MvxBinding;
    using MvvmCross.BindingEx.WindowsPhone.MvxBinding.Target;
    using MvvmCross.BindingEx.WindowsPhone.WindowsBinding;

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
            this._bindingType = bindingType;
        }

        public override void DoRegistration()
        {
            base.DoRegistration();
            this.InitializeBindingCreator();
        }

        protected override void RegisterBindingFactories()
        {
            switch (this._bindingType)
            {
                case BindingType.Windows:
                    // no need for MvvmCross binding factories - so don't create them
                    break;

                case BindingType.MvvmCross:
                    base.RegisterBindingFactories();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override IMvxTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            switch (this._bindingType)
            {
                case BindingType.Windows:
                    return base.CreateTargetBindingRegistry();

                case BindingType.MvvmCross:
                    return new MvxWindowsTargetBindingFactoryRegistry();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializeBindingCreator()
        {
            var creator = this.CreateBindingCreator();
            Mvx.RegisterSingleton(creator);
        }

        protected virtual IMvxBindingCreator CreateBindingCreator()
        {
            switch (this._bindingType)
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

            if (MvxSingleton<IMvxWindowsAssemblyCache>.Instance != null)
            {
                foreach (var assembly in MvxSingleton<IMvxWindowsAssemblyCache>.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }
        }

        protected override void FillValueCombiners(Binding.Combiners.IMvxValueCombinerRegistry registry)
        {
            base.FillValueCombiners(registry);

            if (MvxSingleton<IMvxWindowsAssemblyCache>.Instance != null)
            {
                foreach (var assembly in MvxSingleton<IMvxWindowsAssemblyCache>.Instance.Assemblies)
                {
                    registry.Fill(assembly);
                }
            }
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<FrameworkElement>("Visible",
                                                                    view => new MvxVisibleTargetBinding(view));
            registry.RegisterCustomBindingFactory<FrameworkElement>("Collapsed",
                                                                    view => new MvxCollapsedTargetBinding(view));
            registry.RegisterCustomBindingFactory<FrameworkElement>("Hidden",
                                                                    view => new MvxCollapsedTargetBinding(view));

            base.FillTargetFactories(registry);
        }
    }
}