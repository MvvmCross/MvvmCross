// MvxSimpleSelectionChangedEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;

namespace Cirrious.MvvmCross.Wpf.Commands
{
    public class MvxSimpleSelectionChangedEventArgs : EventArgs
    {
        public IList AddedItems { get; set; }
        public IList RemovedItems { get; set; }
    }
}