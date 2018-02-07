// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core.Views;

namespace MvvmCross.Platform.Uap.Attributes
{
    public class MvxSplitViewPresentationAttribute : MvxBasePresentationAttribute
    {

        public MvxSplitViewPresentationAttribute() : this(SplitPanePosition.Content)
        {
            
        }

        public MvxSplitViewPresentationAttribute(SplitPanePosition position)
        {
            Position = position;
        }

        public SplitPanePosition Position { get; set; }
    }

    public enum SplitPanePosition
    {
        Pane,
        Content
    }

}
