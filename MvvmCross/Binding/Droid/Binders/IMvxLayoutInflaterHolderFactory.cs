// IMvxLayoutInflaterHolderFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders
{
    using System.Collections.Generic;

    using Android.Content;
    using Android.Util;
    using Android.Views;

    public interface IMvxLayoutInflaterHolderFactory : IMvxLayoutInflaterFactory
    {
        IList<KeyValuePair<object, IMvxUpdateableBinding>> CreatedBindings { get; }

        View BindCreatedView(View view, Context context, IAttributeSet attrs);
    }
}