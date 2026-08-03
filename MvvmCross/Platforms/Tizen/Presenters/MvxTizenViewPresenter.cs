
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Tizen.Presenters
{
    public class MvxTizenViewPresenter : MvxAttributeViewPresenter, IMvxTizenViewPresenter
    {
        [RequiresUnreferencedCode("Creates presentation attributes based on runtime view types; type hierarchy checks may not be preserved during trimming.")]
        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            return null;
        }

        [RequiresUnreferencedCode("Getting presentation attribute action uses type hierarchy checks and may call GetPresentationAttribute/CreatePresentationAttribute which require unreferenced code.")]
        public override void RegisterAttributeTypes()
        {
        }
    }
}
