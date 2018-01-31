// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Views.EventSource;

namespace MvvmCross.Forms.Views
{
    public class MvxCellAdapter : MvxBaseCellAdapter
    {
        protected IMvxCell FormsView => View as IMvxCell;

        public MvxCellAdapter(IMvxEventSourceCell eventSource) : base(eventSource)
        {
            if (!(eventSource is IMvxCell))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxCell");
        }

        public override void HandleAppearingCalled(object sender, EventArgs e)
        {
            FormsView.OnViewAppearing();
            base.HandleAppearingCalled(sender, e);
        }
    }
}