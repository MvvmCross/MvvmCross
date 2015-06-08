// MvxAndroidBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid.BindingContext
{
    public class MvxAndroidBindingContext
        : MvxBindingContext
          , IMvxAndroidBindingContext
    {
        private readonly Context _droidContext;
        private IMvxLayoutInflater _layoutInflater;
        private IMvxLayoutInflaterFactoryFactory _layoutInflaterFactoryFactory;

        public MvxAndroidBindingContext(Context droidContext, IMvxLayoutInflater layoutInflater, object source = null)
            : base(source)
        {
            _droidContext = droidContext;
            _layoutInflater = layoutInflater;
        }

        public IMvxLayoutInflater LayoutInflater
        {
            get { return _layoutInflater; }
            set { _layoutInflater = value; }
        }

        protected IMvxLayoutInflaterFactoryFactory FactoryFactory
        {
            get
            {
                if (this._layoutInflaterFactoryFactory == null)
                    this._layoutInflaterFactoryFactory = Mvx.Resolve<IMvxLayoutInflaterFactoryFactory>();
                return this._layoutInflaterFactoryFactory;
            }
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup)
        {
            return BindingInflate(resourceId, viewGroup, true);
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToRoot)
        {
            var view = CommonInflate(
                resourceId,
                viewGroup,
                FactoryFactory.Create(DataContext),
                attachToRoot);
            return view;
        }

        [Obsolete("Switch to new CommonInflate method - with additional attachToRoot parameter")]
        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup,
                                             IMvxLayoutInflaterFactory factory)
        {
            return CommonInflate(resourceId, viewGroup, factory, viewGroup != null);
        }

        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup,
                                             IMvxLayoutInflaterFactory factory, bool attachToRoot)
        {
            using (new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>(this))
            {
                var layoutInflater = _layoutInflater.LayoutInflater;
                using (var clone = layoutInflater.CloneInContext(_droidContext))
                {
                    if (factory != null)
                    {
                        MvxLayoutInflaterCompat.SetFactory(clone, factory);
                    }
                    
                    var toReturn = clone.Inflate(resourceId, viewGroup, attachToRoot);
                    if (factory != null)
                    {
                        RegisterBindingsWithClearKey(toReturn, factory.CreatedBindings);
                    }
                    return toReturn;
                }
            }
        }
    }
}