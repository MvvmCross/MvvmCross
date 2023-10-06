// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Presenters.Attributes
{
#nullable enable
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxBasePresentationAttribute : Attribute, IMvxPresentationAttribute
    {
        /// <inheritdoc />
        public Type? ViewModelType { get; set; }

        /// <inheritdoc />
        public Type? ViewType { get; set; }
    }
#nullable restore
}
