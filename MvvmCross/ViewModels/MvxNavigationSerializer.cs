// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;

namespace MvvmCross.ViewModels
{

    public class MvxNavigationSerializer
        : IMvxNavigationSerializer
    {
        public MvxNavigationSerializer(IMvxTextSerializer serializer)
        {
            Serializer = serializer;
        }

        public IMvxTextSerializer Serializer { get; }
    }

    public class MvxNavigationSerializer<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>
            : MvxNavigationSerializer
                where T : class, IMvxTextSerializer
    {
        public MvxNavigationSerializer()
            : base(Mvx.IoCProvider.Resolve<T>())
        {
        }
    }
}
