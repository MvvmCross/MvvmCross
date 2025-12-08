// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Presenters.Attributes
{
    public interface IMvxPresentationAttribute
    {
        /// <summary>
        /// That shall be used only if you are using non generic views.
        /// </summary>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type? ViewModelType { get; set; }

        /// <summary>
        /// Type of the view
        /// </summary>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type? ViewType { get; set; }
    }
}
