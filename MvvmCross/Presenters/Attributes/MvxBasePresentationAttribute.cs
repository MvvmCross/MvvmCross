// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Presenters.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxBasePresentationAttribute : Attribute, IMvxPresentationAttribute
    {
        /// <inheritdoc />
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? ViewModelType { get; set; }

        /// <inheritdoc />
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? ViewType { get; set; }
    }
}
