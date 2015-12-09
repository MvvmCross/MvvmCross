// MvxLazySingletonCreator.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.IoC
{
    using System;

    public class MvxLazySingletonCreator
    {
        private readonly object _lockObject = new object();
        private readonly Type _type;

        private object _instance;

        public object Instance
        {
            get
            {
                if (this._instance != null)
                    return this._instance;

                lock (this._lockObject)
                {
                    this._instance = this._instance ?? Mvx.IocConstruct(this._type);
                    return this._instance;
                }
            }
        }

        public MvxLazySingletonCreator(Type type)
        {
            this._type = type;
        }
    }
}