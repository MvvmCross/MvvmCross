// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MvvmCross.ViewModels
{
#nullable enable
    public interface IMvxAppStart
    {
        [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
        void Start(object? hint = null);

        [RequiresUnreferencedCode("Navigation uses presentation attributes and view type lookups that may not be preserved during trimming.")]
        Task StartAsync(object? hint = null);

        bool IsStarted { get; }

        void ResetStart();
    }
#nullable restore
}
