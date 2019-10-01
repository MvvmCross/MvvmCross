// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Plugin.Sidebar.Views
{
    public interface IMvxSidebarMenu
    {
        bool AnimateMenu { get; }
        bool DisablePanGesture { get; }
        float DarkOverlayAlpha { get; }
        bool HasDarkOverlay { get; }
        bool HasShadowing { get; }
        float ShadowOpacity { get; }
        float ShadowRadius { get; }
        UIColor ShadowColor { get; }
        UIImage MenuButtonImage { get; }
        int MenuWidth { get; }
        bool ReopenOnRotate { get; }
        void MenuWillOpen();
        void MenuDidOpen();
        void MenuWillClose();
        void MenuDidClose();
    }
}
