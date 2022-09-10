// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Tvos.Presenters.Attributes
{
    public class MvxMasterDetailPresentationAttribute : MvxBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = true;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
        public MasterDetailPosition Position { get; set; } = MasterDetailPosition.Detail;

        public MvxMasterDetailPresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
        {
            Position = position;
        }
    }

    public enum MasterDetailPosition
    {
        Root,
        Master,
        Detail
    }
}
