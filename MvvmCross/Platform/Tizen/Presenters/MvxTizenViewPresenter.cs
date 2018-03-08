
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Presenters;

namespace MvvmCross.Platform.Tizen.Presenters
{
    public class MvxTizenViewPresenter : MvxAttributeViewPresenter, IMvxTizenViewPresenter
    {
        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            throw new NotImplementedException();
        }

        public override void RegisterAttributeTypes()
        {
            throw new NotImplementedException();
        }
    }
}
