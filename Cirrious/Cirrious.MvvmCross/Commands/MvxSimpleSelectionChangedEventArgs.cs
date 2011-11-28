#region Copyright
// <copyright file="MvxSimpleSelectionChangedEventArgs.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections;

namespace Cirrious.MvvmCross.Commands
{
    public class MvxSimpleSelectionChangedEventArgs
    {
        public IList AddedItems { get; set;  }
        public IList RemovedItems { get; set; }
    }
}
