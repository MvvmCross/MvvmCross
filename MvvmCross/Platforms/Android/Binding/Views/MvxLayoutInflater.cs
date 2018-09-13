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
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.Binders;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Boolean = Java.Lang.Boolean;
using Exception = Java.Lang.Exception;
using Object = Java.Lang.Object;

namespace MvvmCross.Platforms.Android.Binding.Views
{
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
            private static readonly Boolean TheTruth = Boolean.True;

            public IMvxLayoutInflaterHolderFactory Factory { get; set; }

            public View OnViewCreated(View view, Context context, IAttributeSet attrs)
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

        public static bool Debug = false;

        private static readonly string Tag = "MvxLayoutInflater";

        internal static BuildVersionCodes Sdk = Build.VERSION.SdkInt;

        private static readonly string[] ClassPrefixList = {
            "android.widget.",
            "android.webkit.",
            "android.app."
        };

        private readonly MvxBindingVisitor _bindingVisitor;

        private IMvxAndroidViewFactory _androidViewFactory;
        private IMvxLayoutInflaterHolderFactoryFactory _layoutInflaterHolderFactoryFactory;
        private Field _constructorArgs;
        private bool _setPrivateFactory;

        public MvxLayoutInflater(Context context)
            : base(context)
        {
            _bindingVisitor = new MvxBindingVisitor();
            SetupLayoutFactories(false);
        }

        public MvxLayoutInflater(LayoutInflater original, Context newContext, MvxBindingVisitor bindingVisitor, bool cloned)
            : base(original, newContext)
        {
            _bindingVisitor = bindingVisitor ?? new MvxBindingVisitor();

            SetupLayoutFactories(cloned);
        }

        public MvxLayoutInflater(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override LayoutInflater CloneInContext(Context newContext)
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
        public override View Inflate(int resource, ViewGroup root, bool attachToRoot)
        {
            // Make sure our private factory is set since LayoutInflater > Honeycomb
            // uses a private factory.
            SetPrivateFactoryInternal();

            // Save the old factory in case we are recursing because of an MvxAdapter etc.
            IMvxLayoutInflaterHolderFactory originalFactory = _bindingVisitor.Factory;

            try
            {
                IMvxLayoutInflaterHolderFactory factory = null;

                // Get the current binding context
                var currentBindingContext = MvxAndroidBindingContextHelpers.Current();
                if (currentBindingContext != null)
                {
                    factory = FactoryFactory.Create(currentBindingContext.DataContext);

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

        protected override View OnCreateView(View parent, string name, IAttributeSet attrs)
        {
            if (Debug)
                MvxLog.Instance.Trace(Tag, "... OnCreateView 3 ... {0}", name);

            return _bindingVisitor.OnViewCreated(
                base.OnCreateView(parent, name, attrs),
                Context,
                attrs);
        }

        protected override View OnCreateView(string name, IAttributeSet attrs)
        {
            if (Debug)
                MvxLog.Instance.Trace(Tag, "... OnCreateView 2 ... {0}", name);

            View view = AndroidViewFactory.CreateView(null, name, Context, attrs) ??
                        PhoneLayoutInflaterOnCreateView(name, attrs) ??
                        base.OnCreateView(name, attrs);

            return _bindingVisitor.OnViewCreated(view, Context, attrs);
        }

        // Mimic PhoneLayoutInflater's OnCreateView.
        private View PhoneLayoutInflaterOnCreateView(string name, IAttributeSet attrs)
        {
            if (Debug)
                MvxLog.Instance.Trace(Tag, "... PhoneLayoutInflaterOnCreateView ... {0}", name);

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
                Factory =
                    new MvxLayoutInflaterCompat.FactoryWrapper(new DelegateFactory1(factory, _bindingVisitor));
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
                Factory2 =
                    new MvxLayoutInflaterCompat.FactoryWrapper2(new DelegateFactory2(factory2, _bindingVisitor));
                return;
            }
            Factory2 = factory2;
        }

        private void SetupLayoutFactories(bool cloned)
        {
            if (cloned)
                return;

            // If factories are already set we need to wrap them in our
            // own secret sauce.
            if (Sdk > BuildVersionCodes.Honeycomb)
            {
                // Check for FactoryWrapper2 may be too loose
                if (Factory2 != null && !(Factory2 is MvxLayoutInflaterCompat.FactoryWrapper2))
                {
                    MvxLayoutInflaterCompat.SetFactory(this, new DelegateFactory2(Factory2, _bindingVisitor));
                }
            }

            // Check for FactoryWrapper may be too loose
            if (Factory != null && !(Factory is MvxLayoutInflaterCompat.FactoryWrapper))
            {
                MvxLayoutInflaterCompat.SetFactory(this, new DelegateFactory1(Factory, _bindingVisitor));
            }
        }

        private void SetPrivateFactoryInternal()
        {
            if (_setPrivateFactory)
                return;

            if (Build.VERSION.SdkInt < BuildVersionCodes.Honeycomb)
                return;

            if (!(Context is IFactory2))
            {
                _setPrivateFactory = true;
                return;
            }

            Class layoutInflaterClass = Class.FromType(typeof(LayoutInflater));
            Method setPrivateFactoryMethod = layoutInflaterClass.GetMethod("setPrivateFactory", Class.FromType(typeof(IFactory2)));
            if (setPrivateFactoryMethod != null)
            {
                try
                {
                    setPrivateFactoryMethod.Accessible = true;
                    setPrivateFactoryMethod.Invoke(this,
                        new PrivateFactoryWrapper2((IFactory2)Context, this, _bindingVisitor));
                }
                catch (Exception ex)
                {
                    MvxLog.Instance.Warn("Cannot invoke LayoutInflater.setPrivateFactory :\n{0}", ex.StackTrace);
                }
            }

            _setPrivateFactory = true;
        }

        protected View CreateCustomViewInternal(View parent, View view, string name, Context viewContext,
            IAttributeSet attrs)
        {
            if (Debug)
                MvxLog.Instance.Trace(Tag, "... CreateCustomViewInternal ... {0}", name);

            if (view == null && name.IndexOf('.') > -1)
            {
                // Attempt to inflate with MvvmCross unless we're trying to inflate an internal views
                // since we don't resolve those.
                if (!name.StartsWith("com.android.internal."))
                {
                    view = AndroidViewFactory.CreateView(parent, name, viewContext, attrs);
                }

                if (view == null)
                {
                    if (_constructorArgs == null)
                    {
                        Class layoutInflaterClass = Class.FromType(typeof(LayoutInflater));
                        _constructorArgs = layoutInflaterClass.GetDeclaredField("mConstructorArgs");
                        _constructorArgs.Accessible = true;
                    }

                    Object[] constructorArgsArr = (Object[])_constructorArgs.Get(this);
                    Object lastContext = constructorArgsArr[0];

                    // The LayoutInflater actually finds out the correct context to use. We just need to set
                    // it on the mConstructor for the internal method.
                    // Set the constructor args up for the createView, not sure why we can't pass these in.
                    constructorArgsArr[0] = viewContext;
                    _constructorArgs.Set(this, constructorArgsArr);
                    try
                    {
                        view = CreateView(name, null, attrs);
                    }
                    catch (ClassNotFoundException) 
                    {
                    }
                    finally
                    {
                        constructorArgsArr[0] = lastContext;
                        _constructorArgs.Set(this, constructorArgsArr);
                    }
                }
            }
            return view;
        }

        protected IMvxAndroidViewFactory AndroidViewFactory => _androidViewFactory ?? (_androidViewFactory = Mvx.IoCProvider.Resolve<IMvxAndroidViewFactory>());

        protected IMvxLayoutInflaterHolderFactoryFactory FactoryFactory => _layoutInflaterHolderFactoryFactory ??
                                                                           (_layoutInflaterHolderFactoryFactory = Mvx.IoCProvider.Resolve<IMvxLayoutInflaterHolderFactoryFactory>());

        private class DelegateFactory2 : IMvxLayoutInflaterFactory
        {
            private static readonly string Tag = "DelegateFactory2";

            private readonly IFactory2 _factory;
            private readonly MvxBindingVisitor _factoryPlaceholder;

            public DelegateFactory2(IFactory2 factoryToWrap, MvxBindingVisitor binder)
            {
                _factory = factoryToWrap;
                _factoryPlaceholder = binder;
            }

            public View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLog.Instance.Trace(Tag, "... OnCreateView ... {0}", name);

                return _factoryPlaceholder.OnViewCreated(
                    _factory.OnCreateView(parent, name, context, attrs),
                    context, attrs);
            }
        }

        private class DelegateFactory1 : IMvxLayoutInflaterFactory
        {
            private static readonly string Tag = "DelegateFactory1";

            private readonly IFactory _factory;
            private readonly MvxBindingVisitor _factoryPlaceholder;

            public DelegateFactory1(IFactory factoryToWrap, MvxBindingVisitor bindingVisitor)
            {
                _factory = factoryToWrap;
                _factoryPlaceholder = bindingVisitor;
            }

            public View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLog.Instance.Trace(Tag, "... OnCreateView ... {0}", name);

                return _factoryPlaceholder.OnViewCreated(
                    _factory.OnCreateView(name, context, attrs),
                    context, attrs);
            }
        }

        private class PrivateFactoryWrapper2 : Object, IFactory2
        {
            private static readonly string Tag = "PrivateFactoryWrapper2";

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

            public PrivateFactoryWrapper2(IntPtr handle, JniHandleOwnership transfer)
                : base(handle, transfer)
            {
            }

            public View OnCreateView(string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLog.Instance.Trace(Tag, "... OnCreateView 2 ... {0}", name);

                return _bindingVisitor.OnViewCreated(
                    // The activity's OnCreateView
                    _factory2.OnCreateView(name, context, attrs),
                    context, attrs);
            }

            public View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
            {
                if (Debug)
                    MvxLog.Instance.Trace(Tag, "... OnCreateView 3 ... {0}", name);

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
}
