// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core;
using Playground.Droid.Bindings;
using Playground.Droid.Controls;
using Serilog;

namespace Playground.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override IEnumerable<Assembly> AndroidViewAssemblies =>
            new List<Assembly>(base.AndroidViewAssemblies)
            {
                typeof(MvxRecyclerView).Assembly
            };

        public override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Serilog;

        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();
            return base.CreateLogProvider();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new CustomPresenter(AndroidViewAssemblies);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<BinaryEdit>(
                "MyCount",
                (arg) => new BinaryEditTargetBinding(arg));

            base.FillTargetFactories(registry);
        }

        public class CustomPresenter : MvxAppCompatViewPresenter
        {
            public CustomPresenter(IEnumerable<Assembly> androidViewAssemblies)
                : base(androidViewAssemblies)
            {
            }

            protected override void ShowNestedFragment(Type view, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
            {
                var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                if (fragmentHost == null)
                    throw new NullReferenceException($"Fragment host not found when trying to show View {view.Name} as Nested Fragment");

                //if (!fragmentHost.IsVisible)
                //    throw new InvalidOperationException($"Fragment host is not visible when trying to show View {view.Name} as Nested Fragment");

                PerformShowFragmentTransaction(fragmentHost.ChildFragmentManager, attribute, request);
            }
        }
    }
}
