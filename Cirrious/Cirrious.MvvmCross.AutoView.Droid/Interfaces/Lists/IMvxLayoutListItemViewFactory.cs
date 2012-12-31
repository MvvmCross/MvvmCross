#region Copyright

// <copyright file="IMvxLayoutListItemViewFactory.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Content;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using CrossUI.Core.Elements.Lists;

namespace Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists
{
    public interface IMvxLayoutListItemViewFactory
        : IListItemLayout
    {
        string UniqueName { get; }
        View BuildView(Context context, IMvxBindingActivity bindingActivity, object source);
    }
}