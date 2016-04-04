// IMvxRecyclerAdapter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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

        IItemTemplateSelector ItemTemplateSelector { get; set; }
        ICommand ItemClick { get; set; }
        ICommand ItemLongClick { get; set; }

        object GetItem(int position);

        int ItemTemplateId { get; set; }
    }
}