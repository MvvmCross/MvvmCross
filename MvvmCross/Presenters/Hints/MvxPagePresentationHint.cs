// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using MvvmCross.ViewModels;

namespace MvvmCross.Presenters.Hints
{
    public class MvxPagePresentationHint
        : MvxPresentationHint
    {
        public MvxPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel)
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel, MvxBundle body) : base(body)
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModel, IDictionary<string, string> hints) : this(viewModel, new MvxBundle(hints))
        {
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type ViewModel { get; }
    }
}
