// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.IoC
{
    public class MvxLazySingletonCreator
    {
        private readonly object _lockObject = new object();

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        private readonly Type _type;
        private object _instance;

        public object Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lockObject)
                {
                    _instance ??= Mvx.IoCProvider.IoCConstruct(_type);
                    return _instance;
                }
            }
        }

        public MvxLazySingletonCreator(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type)
        {
            _type = type;
        }
    }
}
