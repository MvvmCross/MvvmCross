// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Views.Base;

namespace MvvmCross.Forms.Views
{
    public class MvxElementAdapter : MvxBaseElementAdapter
    {
        protected IMvxElement FormsView => View as IMvxElement;

        public MvxElementAdapter(IMvxEventSourceElement eventSource) : base(eventSource)
        {
            if (!(eventSource is IMvxElement))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxElement");
        }

        public override async void HandleBindingContextChangedCalled(object sender, EventArgs e)
        {
            await FormsView.OnBindingContextChanged().ConfigureAwait(false);
            base.HandleBindingContextChangedCalled(sender, e);
        }
    }
}
