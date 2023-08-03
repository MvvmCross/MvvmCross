// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC
{
    public class MvxLazySingletonCreator
    {
        private object _instance;
        private readonly Lazy<object> _lazyType;

        public object Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = _lazyType.Value;

                return _instance;
            }
        }

        public MvxLazySingletonCreator(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]Type type)
        {
            _lazyType = new Lazy<object>(() => Mvx.IoCProvider?.IoCConstruct(type));
        }
    }
}
