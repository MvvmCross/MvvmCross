// MvxLazySingletonCreator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.IoC
{
    public class MvxLazySingletonCreator
    {
        private readonly object _lockObject = new object();
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
                    _instance = _instance ?? Mvx.IocConstruct(_type);
                    return _instance;
                }
            }
        }

        public MvxLazySingletonCreator(Type type)
        {
            _type = type;
        }
    }
}