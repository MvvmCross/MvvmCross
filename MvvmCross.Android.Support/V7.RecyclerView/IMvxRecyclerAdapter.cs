// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Windows.Input;
using MvvmCross.Binding.Attributes;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace MvvmCross.Droid.Support.V7.RecyclerView
{
    public interface IMvxRecyclerAdapter
    {
        [MvxSetToNullAfterBinding]
        IEnumerable ItemsSource { get; set; }

        IMvxTemplateSelector ItemTemplateSelector { get; set; }
        ICommand ItemClick { get; set; }
        ICommand ItemLongClick { get; set; }

        object GetItem(int viewPosition);

        int ItemTemplateId { get; set; }
    }
}