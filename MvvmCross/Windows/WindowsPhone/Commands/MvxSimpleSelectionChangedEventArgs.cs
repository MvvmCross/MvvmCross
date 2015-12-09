// MvxSimpleSelectionChangedEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Commands
{
    using System;
    using System.Collections;

    public class MvxSimpleSelectionChangedEventArgs : EventArgs
    {
        public IList AddedItems { get; set; }
        public IList RemovedItems { get; set; }
    }
}