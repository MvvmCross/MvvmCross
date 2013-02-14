// MvxViewBindingExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.App;
using Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Binding.Droid.Binders;

namespace Cirrious.MvvmCross.Binding.Droid.ExtensionMethods
{
    public static class MvxViewBindingExtensions
    {
        /// <summary>
        /// Sets the binding tag for a <see cref="View"/>. 
        /// </summary>
        /// 
        /// <seealso cref="M:GetBindingTag"/>
        public static void SetBindingTag(this View view, MvxViewBindingTag tag)
        {
            view.SetTag (MvxAndroidBindingResource.Instance.BindingTagUnique, tag);
        }

        /// <summary>
        /// Gets the binding tag for a <see cref="View"/>. 
        /// </summary>
        /// <returns>The binding tag, or <c>null</c> if not set.</returns>
        /// 
        /// <seealso cref="M:SetBindingTag"/>
        public static MvxViewBindingTag GetBindingTag(this View view)
        {
            return view.GetTag (MvxAndroidBindingResource.Instance.BindingTagUnique) as MvxViewBindingTag;
        }

        /// <summary>
        /// Updates the <see cref="T:MvxViewBindingTag"/> data source.
        /// You need to call <see cref="M:IMvxViewBindingManager.BindView"/> to rebind items.
        /// </summary>
        public static void UpdateDataSource(this View view, object dataSource) {
            var tag = view.GetBindingTag();
            if (tag == null) {
                tag = new MvxViewBindingTag() {
                    DataSource = dataSource,
                };
                view.SetBindingTag(tag);
            } else {
                tag.DataSource = dataSource;
            }
        }

        /// <summary>
        /// Removes the <see cref="T:MvxViewBindingTag"/> data source, by disabling data source overriding.
        /// </summary>
        public static void RemoveDataSource(this View view) {
            var tag = view.GetBindingTag();
            if (tag != null) {
                tag.OverrideDataSource = false;
            }
        }
    }
}