// IMvxAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Widget;
using Cirrious.MvvmCross.Binding.Attributes;
using System.Collections;

namespace Cirrious.MvvmCross.Binding.Droid.Views
{
    public interface IMvxAdapter
        : ISpinnerAdapter
        , IListAdapter
    {
        int SimpleViewLayoutId { get; set; }

        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }
        int DropDownItemTemplateId { get; set; }

        object GetRawItem(int position);

        int GetPosition(object value);
    }
}