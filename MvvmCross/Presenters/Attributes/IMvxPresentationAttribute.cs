// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Presenters.Attributes
{
#nullable enable
    public interface IMvxPresentationAttribute
    {
        /// <summary>
        /// That shall be used only if you are using non generic views.
        /// </summary>
        Type? ViewModelType { get; set; }

        /// <summary>
        /// Type of the view
        /// </summary>
        Type? ViewType { get; set; }
    }
#nullable restore
}
