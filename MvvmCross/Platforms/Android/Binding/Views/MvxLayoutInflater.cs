// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Java.Interop;
using Java.Lang;
using Java.Lang.Reflect;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.Binders;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Boolean = Java.Lang.Boolean;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;

namespace MvvmCross.Platforms.Android.Binding.Views
{
#nullable enable
    /// <summary>
    /// Custom LayoutInflater responsible for inflating views and hooking up bindings
    /// Typically this is attached to MvxActivity and co via our MvxContextWrapper.
    ///
    /// Potential order of view creation is the following (HC+):
    ///   1. IFactory2.OnCreateView
    ///   2. IFactory.OnCreateView
    ///   3. PrivateFactory.OnCreateView
    ///   4. OnCreateView(parent, name, attrs)
    ///   5. OnCreateView(name, attrs)
    ///   6. CreateView (sadly final)
    ///
    /// We intercept these calls and wrap any IFactory/IFactory2 with our own factory
    /// that binds when the view is returned.
    ///
    /// Heavily based on Calligraphy's CalligraphyLayoutInflater
    /// See: https://github.com/chrisjenx/Calligraphy/blob/master/calligraphy/src/main/java/uk/co/chrisjenx/calligraphy/CalligraphyLayoutInflater.java" />
    /// </summary>
    [Register("mvvmcross.platforms.android.binding.views.MvxLayoutInflater")]
    public class MvxLayoutInflater : LayoutInflater
    {
        public class MvxBindingVisitor
        {
            private static readonly Boolean TheTruth = Boolean.True!;

            public IMvxLayoutInflaterHolderFactory? Factory { get; set; }

            public View? OnViewCreated(View? view, Context? context, IAttributeSet? attrs)
            {
                if (Factory != null && view != null && view.GetTag(Resource.Id.MvvmCrossTagId) != TheTruth)
                {
                    // Bind here.
                    view = Factory.BindCreatedView(view, context, attrs);

                    view.SetTag(Resource.Id.MvvmCrossTagId, TheTruth);
                }

                return view;
            }
        }

        public static bool Debug { get; set; }

        private const string Tag = "MvxLayoutInflater";

        private static readonly string[] ClassPrefixList = {
            "android.widget.",
            "android.webkit.",
            "android.app."
        };

        private readonly MvxBindingVisitor _bindingVisitor;

        private IMvxAndroidViewFactory? _androidViewFactory;
        private IMvxLayoutInflaterHolderFactoryFactory? _layoutInflaterHolderFactoryFactory;
        private Field? _constructorArgs;
        private bool _setPrivateFactory;

        public MvxLayoutInflater(Context context)
            : base(context)
        {
            _bindingVisitor = new MvxBindingVisitor();
            SetupLayoutFactories(false);
        }

        public MvxLayoutInflater(LayoutInflater original, Context? newContext, MvxBindingVisitor? bindingVisitor, bool cloned)
            : base(original, newContext)
        {
            _bindingVisitor = bindingVisitor ?? new MvxBindingVisitor();

            SetupLayoutFactories(cloned);
        }

        [Preserve(Conditional = true)]
        public MvxLayoutInflater(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            _bindingVisitor = new MvxBindingVisitor();
            SetupLayoutFactories(false);
        }

        public override LayoutInflater CloneInContext(Context? newContext)
        {
            return new MvxLayoutInflater(this, newContext, _bindingVisitor, true);
        }

        // We can't call this.  See: https://bugzilla.xamarin.com/show_bug.cgi?id=30843
        //public override View Inflate(XmlReader parser, ViewGroup root, bool attachToRoot)
        //{
        //    SetPrivateFactoryInternal();
        //    return base.Inflate(parser, root, attachToRoot);
        //}

        // Calligraphy doesn't override this one...
        public override View? Inflate(int resource, ViewGroup? root, bool attachToRoot)
        {
            // Make sure our private factory is set since LayoutInflater > Honeycomb
            // uses a private factory.
            SetPrivateFactoryInternal();

            // Save the old factory in case we are recursing because of an MvxAdapter etc.
            IMvxLayoutInflaterHolderFactory? originalFactory = _bindingVisitor.Factory;

            try
            {
                IMvxLayoutInflaterHolderFactory? factory = null;

                // Get the current binding context
                var currentBindingContext = MvxAndroidBindingContextHelpers.Current();
                if (currentBindingContext != null)
                {
                    factory = FactoryFactory?.Create(currentBindingContext.DataContext);

                    // Set the current factory used to generate bindings
                    if (factory != null)
                        _bindingVisitor.Factory = factory;
                }

                // Inflate the resource
                var view = base.Inflate(resource, root, attachToRoot);

                // Register bindings with clear key
                if (currentBindingContext != null)
                {
                    if (factory != null)
                        currentBindingContext.RegisterBindingsWithClearKey(view, factory.CreatedBindings);
                }

                return view;
            }
            finally
            {
                _bindingVisitor.Factory = originalFactory;
            }
        }

        protected override View? OnCreateView(View? parent, string? name, IAttributeSet? attrs)
        {
            if (Debug)
                MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... OnCreateView 3 ... {name}", Tag, name);

            return _bindingVisitor.OnViewCreated(
                base.OnCreateView(parent, name, attrs),
                Context,
                attrs);
        }

        protected override View? OnCreateView(string? name, IAttributeSet? attrs)
        {
            if (Debug)
                MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... OnCreateView 2 ... {name}", Tag, name);

            View? view = null;
            if (name != null && Context != null && attrs != null)
                view = AndroidViewFactory?.CreateView(null, name, Context, attrs);

            view ??= PhoneLayoutInflaterOnCreateView(name, attrs) ?? base.OnCreateView(name, attrs);

            return _bindingVisitor.OnViewCreated(view, Context, attrs);
        }

#if __ANDROID_29__
        public override View? OnCreateView(Context viewContext, View? parent, string name, IAttributeSet? attrs)
        {
            if (Debug)
                MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... OnCreateView 4 ... {name}", Tag, name);

            return _bindingVisitor.OnViewCreated(
                base.OnCreateView(viewContext, parent, name, attrs),
                viewContext,
                attrs);
        }
#endif

        // Mimic PhoneLayoutInflater's OnCreateView.
        private View? PhoneLayoutInflaterOnCreateView(string? name, IAttributeSet? attrs)
        {
            if (Debug)
                MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... PhoneLayoutInflaterOnCreateView ... {name}", Tag, name);

            foreach (var prefix in ClassPrefixList)
            {
                try
                {
                    return CreateView(name, prefix, attrs);
                }
                catch (ClassNotFoundException)
                {
                }
            }

            return null;
        }

        // Note: setFactory/setFactory2 are implemented with export
        // because there's a bug in the generator that doesn't
        // mark the Factory/Factory2 setters as virtual.
        // See: https://bugzilla.xamarin.com/show_bug.cgi?id=30764
        [Export]
        public void setFactory(IFactory factory)
        {
            // Wrap the incoming factory if we need to.
            if (!(factory is MvxLayoutInflaterCompat.FactoryWrapper))
            {
                Factory = new MvxLayoutInflaterCompat.FactoryWrapper(new DelegateFactory1(factory, _bindingVisitor));
                return;
            }

            Factory = factory;
        }

        [Export]
        public void setFactory2(IFactory2 factory2)
        {
            // Wrap the incoming factory if we need to.
            if (!(factory2 is MvxLayoutInflaterCompat.FactoryWrapper2))
            {
                Factory2 = new MvxLayoutInflaterCompat.FactoryWrapper2(new DelegateFactory2(factory2, _bindingVisitor));
                return;
            }

            Factory2 = factory2;
        }

        private void SetupLayoutFactories(bool cloned)
        {
            if (cloned)
                return;

            // Check for FactoryWrapper2 may be too loose
            if (Factory2 != null && !(Factory2 is MvxLayoutInflaterCompat.FactoryWrapper2))
            {
                MvxLayoutInflaterCompat.SetFactory(this, new DelegateFactory2(Factory2, _bindingVisitor));
            }
        }

        private void SetPrivateFactoryInternal()
        {
            if (_setPrivateFactory)
                return;

            if (!(Context is IFactory2))
            {
                _setPrivateFactory = true;
                return;
            }

            try
            {
                Class layoutInflaterClass = Class.FromType(typeof(LayoutInflater));
                Method setPrivateFactoryMethod = layoutInflaterClass.GetMethod("setPrivateFactory", Class.FromType(typeof(IFactory2)));
                setPrivateFactoryMethod.Accessible = true;
                setPrivateFactoryMethod.Invoke(this,
                    new PrivateFactoryWrapper2((IFactory2)Context, this, _bindingVisitor));
            }
            catch (Exception ex)
            {
                MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Warning, ex, "Cannot invoke LayoutInflater.setPrivateFactory :\n{0}", ex.StackTrace);
            }

            _setPrivateFactory = true;
        }

        internal View? CreateCustomViewInternal(View? parent, View? view, string name, Context viewContext,
            IAttributeSet attrs)
        {
            if (Debug)
                MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... CreateCustomViewInternal ... {name}", Tag, name);

            if (view == null &&
                !string.IsNullOrWhiteSpace(name) &&
                name.IndexOf('.', StringComparison.InvariantCulture) > -1)
            {
                // Attempt to inflate with MvvmCross unless we're trying to inflate an internal views
                // since we don't resolve those.
                if (!name.StartsWith("com.android.internal.", StringComparison.InvariantCulture))
                {
                    view = AndroidViewFactory?.CreateView(parent, name, viewContext, attrs);
                }

                if (view == null)
                {
                    var (constructorArgsArr, lastContext) = GetConstructorArgs(viewContext);

                    try
                    {
                        view = CreateViewCompat(viewContext, name, attrs);
                    }
                    catch (ClassNotFoundException)
                    {
                    }
                    finally
                    {
                        RestoreConstructorArgs(constructorArgsArr, lastContext);
                    }
                }
            }

            return view;
        }

        private (Object[]? constructorArgs, Object? lastContext) GetConstructorArgs(Context viewContext)
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.P)
            {
                return (null, null);
            }

            if (_constructorArgs == null)
            {
                Class layoutInflaterClass = Class.FromType(typeof(LayoutInflater));
                _constructorArgs = layoutInflaterClass.GetDeclaredField("mConstructorArgs");
                _constructorArgs.Accessible = true;
            }

            var constructorArgsArr = (Object[]?)_constructorArgs!.Get(this);
            var lastContext = constructorArgsArr?[0];

            // The LayoutInflater actually finds out the correct context to use. We just need to set
            // it on the mConstructor for the internal method.
            // Set the constructor args up for the createView, not sure why we can't pass these in.
            if (constructorArgsArr != null)
            {
                constructorArgsArr[0] = viewContext;
                _constructorArgs.Set(this, constructorArgsArr);
            }

            return (constructorArgsArr, lastContext);
        }

        private void RestoreConstructorArgs(Object[]? constructorArgsArr, Object? lastContext)
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.P || constructorArgsArr == null || lastContext == null)
                return;

            constructorArgsArr[0] = lastContext;
            _constructorArgs?.Set(this, constructorArgsArr);
        }

        private View? CreateViewCompat(Context viewContext, string name, IAttributeSet attrs)
        {
            View? view;
#if __ANDROID_29__
            if (Build.VERSION.SdkInt > BuildVersionCodes.P)
                view = CreateView(viewContext, name, null, attrs);
            else
#endif
                view = CreateView(name, null, attrs);

            return view;
        }

        protected IMvxAndroidViewFactory? AndroidViewFactory
        {
            get
            {
                if (_androidViewFactory != null)
                    return _androidViewFactory;

                if (Mvx.IoCProvider == null)
                {
                    // if IoCProvider is null, Log instance will probably be null too
                    MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... AndroidViewFactory IoCProvider is null!", Tag);
                    return null;
                }

                if (Mvx.IoCProvider.TryResolve(out IMvxAndroidViewFactory viewFactory))
                {
                    _androidViewFactory = viewFactory;
                }

                return _androidViewFactory;
            }
        }

        protected IMvxLayoutInflaterHolderFactoryFactory? FactoryFactory
        {
            get
            {
                if (_layoutInflaterHolderFactoryFactory != null)
                    return _layoutInflaterHolderFactoryFactory;

                if (Mvx.IoCProvider == null)
                {
                    // if IoCProvider is null, Log instance will probably be null too
                    MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Error, "{Tag} - ... FactoryFactory IoCProvider is null!", Tag);
                    return null;
                }

                if (Mvx.IoCProvider.TryResolve(out IMvxLayoutInflaterHolderFactoryFactory factoryFactory))
                {
                    _layoutInflaterHolderFactoryFactory = factoryFactory;
                }

                return _layoutInflaterHolderFactoryFactory;
            }
        }

        private class DelegateFactory2 : IMvxLayoutInflaterFactory
        {
            private const string DelegateFactory2Tag = "DelegateFactory2";

            private readonly IFactory2 _factory;
            private readonly MvxBindingVisitor _factoryPlaceholder;

            public DelegateFactory2(IFactory2 factoryToWrap, MvxBindingVisitor binder)
            {
                _factory = factoryToWrap;
                _factoryPlaceholder = binder;
            }

            public View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... OnCreateView ... {name}", DelegateFactory2Tag, name);

                return _factoryPlaceholder.OnViewCreated(
                    _factory.OnCreateView(parent, name, context, attrs),
                    context, attrs);
            }
        }

        private class DelegateFactory1 : IMvxLayoutInflaterFactory
        {
            private const string DelegateFactory1Tag = "DelegateFactory1";

            private readonly IFactory _factory;
            private readonly MvxBindingVisitor _factoryPlaceholder;

            public DelegateFactory1(IFactory factoryToWrap, MvxBindingVisitor bindingVisitor)
            {
                _factory = factoryToWrap;
                _factoryPlaceholder = bindingVisitor;
            }

            public View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... OnCreateView ... {name}", DelegateFactory1Tag, name);

                return _factoryPlaceholder.OnViewCreated(
                    _factory.OnCreateView(name, context, attrs),
                    context, attrs);
            }
        }

        private class PrivateFactoryWrapper2 : Object, IFactory2
        {
            private const string PrivateFactoryWrapper2Tag = "PrivateFactoryWrapper2";

            private readonly IFactory2 _factory2;
            private readonly MvxBindingVisitor _bindingVisitor;
            private readonly MvxLayoutInflater _inflater;

            internal PrivateFactoryWrapper2(IFactory2 factory2, MvxLayoutInflater inflater,
                MvxBindingVisitor bindingVisitor)
            {
                _factory2 = factory2;
                _inflater = inflater;
                _bindingVisitor = bindingVisitor;
            }

            [Preserve(Conditional = true)]
#pragma warning disable 8618
            public PrivateFactoryWrapper2(IntPtr handle, JniHandleOwnership transfer)
#pragma warning restore 8618
                : base(handle, transfer)
            {
            }

            public View? OnCreateView(string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... OnCreateView 2 ... {name}", PrivateFactoryWrapper2Tag, name);

                return _bindingVisitor.OnViewCreated(
                    // The activity's OnCreateView
                    _factory2.OnCreateView(name, context, attrs),
                    context, attrs);
            }

            public View? OnCreateView(View? parent, string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLogHost.GetLog<MvxLayoutInflater>()?.Log(LogLevel.Trace, "{Tag} - ... OnCreateView 3 ... {name}", PrivateFactoryWrapper2Tag, name);

                return _bindingVisitor.OnViewCreated(
                    _inflater.CreateCustomViewInternal(
                        parent,
                        // The activity's OnCreateView
                        _factory2.OnCreateView(parent, name, context, attrs),
                        name, context, attrs),
                    context, attrs);
            }
        }
    }
#nullable restore
}
