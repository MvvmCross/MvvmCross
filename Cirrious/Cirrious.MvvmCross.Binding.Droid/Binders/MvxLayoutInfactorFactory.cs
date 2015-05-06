using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Cirrious.CrossCore;
using Java.Lang;
using Java.Lang.Reflect;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public static class MvxLayoutInfactorFactory
    {
        private static readonly int SdkInt = (int)Build.VERSION.SdkInt;
        private static Field _layoutInflaterFactory2Field;
        private static bool _checkedField;

        private class FactoryWrapper : Java.Lang.Object, LayoutInflater.IFactory
        {
            protected readonly IMvxLayoutInfactorFactory DelegateFactory;

            public FactoryWrapper(IMvxLayoutInfactorFactory delegateFactory)
            {
                this.DelegateFactory = delegateFactory;
            }

            public View OnCreateView(string name, Context context, IAttributeSet attrs)
            {
                return this.DelegateFactory.OnCreateView(null, name, context, attrs);
            }
        }

        private class FactoryWrapper2 : FactoryWrapper, LayoutInflater.IFactory2
        {
            public FactoryWrapper2(IMvxLayoutInfactorFactory delegateFactory)
                : base(delegateFactory) {}

            public View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
            {
                return DelegateFactory.OnCreateView(parent, name, context, attrs);
            }
        }

        public static void SetFactory(LayoutInflater layoutInflater, IMvxLayoutInfactorFactory factory)
        {
            if (SdkInt >= 21)
            {
                layoutInflater.Factory2 = (factory != null ? new FactoryWrapper2(factory) : null);
            }
            else if (SdkInt >= 11)
            {
                var factory2 = factory != null ? new FactoryWrapper2(factory) : null;
                layoutInflater.Factory2 = factory2;

                LayoutInflater.IFactory f = layoutInflater.Factory;
                var f2 = f as LayoutInflater.IFactory2;
                if (f2 != null)
                {
                    // The merged factory is now set to Factory, but not Factory2 (pre-v21).
                    // We will now try and force set the merged factory to mFactory2
                    ForceSetFactory2(layoutInflater, f2);
                }
                else
                {
                    // Else, we will force set the original wrapped Factory2
                    ForceSetFactory2(layoutInflater, factory2);
                }

            }
            else
            {
                layoutInflater.Factory = (factory != null ? new FactoryWrapper(factory) : null);
            }
        }


        // Workaround from Support.v4 v22.1.1 library:
        //
        // For APIs >= 11 && < 21, there was a framework bug that prevented a LayoutInflater's
        // Factory2 from being merged properly if set after a cloneInContext from a LayoutInflater
        // that already had a Factory2 registered. We work around that bug here. If we can't we
        // log an error.
        private static void ForceSetFactory2(LayoutInflater inflater, LayoutInflater.IFactory2 factory)
        {
            if (!_checkedField)
            {
                try
                {
                    Class layoutInflaterClass = Class.FromType(typeof(LayoutInflater));
                    _layoutInflaterFactory2Field = layoutInflaterClass.GetDeclaredField("mFactory2");
                    _layoutInflaterFactory2Field.Accessible = true;
                }
                catch (NoSuchFieldException e)
                {
                    Mvx.Error(
                        "ForceSetFactory2 Could not find field 'mFactory2' on class {0}; inflation may have unexpected results.",
                        Class.FromType(typeof(LayoutInflater)).Name);
                }
                _checkedField = true;
            }

            if (_layoutInflaterFactory2Field != null)
            {
                try
                {
                    _layoutInflaterFactory2Field.Set(inflater, (Java.Lang.Object)factory);
                }
                catch (IllegalAccessException e)
                {
                    Mvx.Error("ForceSetFactory2 could not set the Factory2 on LayoutInflater {0} ; inflation may have unexpected results.", inflater);
                }
            }
        }
    }
}