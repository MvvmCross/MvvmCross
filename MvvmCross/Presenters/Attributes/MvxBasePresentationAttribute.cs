// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Presenters.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxBasePresentationAttribute : Attribute
    {
        /// <summary>
        /// That shall be used only if you are using non generic views.
        /// </summary>
        public Type ViewModelType { get; set; }

        /// <summary>
        /// Type of the view
        /// </summary>
        public Type ViewType { get; set; }
    }
}
