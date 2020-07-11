// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Forms.Views.Base;

namespace MvvmCross.Forms.Views
{
    public class MvxPageAdapter : MvxBasePageAdapter
    {
        protected IMvxPage FormsView => View as IMvxPage;

        public MvxPageAdapter(IMvxEventSourcePage eventSource) : base(eventSource)
        {
            if (!(eventSource is IMvxPage))
                throw new ArgumentException("eventSource", "eventSource should be a IMvxPage");
        }

        public override async void HandleAppearingCalled(object sender, EventArgs e)
        {
            await FormsView.OnViewAppearing().ConfigureAwait(false);
            base.HandleAppearingCalled(sender, e);
        }
    }
}
