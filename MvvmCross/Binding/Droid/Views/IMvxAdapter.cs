// IMvxAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Views
{
    using System.Collections;

    using Android.Widget;

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