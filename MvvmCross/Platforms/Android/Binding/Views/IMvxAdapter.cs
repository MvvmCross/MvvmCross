// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using Android.Widget;
using MvvmCross.Binding.Attributes;

namespace MvvmCross.Platforms.Android.Binding.Views
{
    public interface IMvxAdapter
        : ISpinnerAdapter
        , IListAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        int ItemTemplateId { get; set; }
        int DropDownItemTemplateId { get; set; }

        object GetRawItem(int position);

        int GetPosition(object value);
    }
}
