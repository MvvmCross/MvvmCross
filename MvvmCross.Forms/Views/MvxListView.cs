// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;

namespace MvvmCross.Forms.Views
{
    public class MvxListView : Xamarin.Forms.ListView
    {
        public MvxListView(Xamarin.Forms.ListViewCachingStrategy strategy) : base(strategy)
        {
        }

        public MvxListView() : base()
        {
        }

        public ICommand ItemClick
        {
            get;
            set;
        }
    }
}
