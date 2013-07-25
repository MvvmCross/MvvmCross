// MvxAndroidBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Views;
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
        private readonly IMvxLayoutInflater _layoutInflater;

        public MvxAndroidBindingContext(Context droidContext, IMvxLayoutInflater layoutInflater, object source = null)
            : base(source)
        {
            _droidContext = droidContext;
            _layoutInflater = layoutInflater;
        }

        public IMvxLayoutInflater LayoutInflater
        {
            get { return _layoutInflater; }
        }

        public virtual View BindingInflate(int resourceId, ViewGroup viewGroup)
        {
            var view = CommonInflate(
                resourceId,
                viewGroup,
                new MvxBindingLayoutInflatorFactory(DataContext));
            return view;
        }

        protected virtual View CommonInflate(int resourceId, ViewGroup viewGroup,
                                             MvxBindingLayoutInflatorFactory factory)
        {
            using (new MvxBindingContextStackRegistration<IMvxAndroidBindingContext>(this))
            {
                var layoutInflator = _layoutInflater.LayoutInflater;
                using (var clone = layoutInflator.CloneInContext(_droidContext))
                {
                    using (factory)
                    {
                        if (factory != null)
                        {
                            clone.Factory = factory;
                        }
                        var toReturn = clone.Inflate(resourceId, viewGroup);
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
}