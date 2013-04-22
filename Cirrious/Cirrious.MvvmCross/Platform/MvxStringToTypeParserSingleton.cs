// MvxStringToTypeParserSingleton.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Platform
{
    public class MvxStringToTypeParserSingleton
        : MvxSingleton<IMvxStringToTypeParser>
        , IMvxStringToTypeParser
    {
        static MvxStringToTypeParserSingleton()
        {
            // create new singleton - base class will store this as instance
            var instance = new MvxStringToTypeParserSingleton();
        }

        private MvxStringToTypeParserSingleton()
        {
            // private constructor
        }

        public static new IMvxStringToTypeParser Instance
        {
            get { return MvxSingleton<IMvxStringToTypeParser>.Instance; }
        }

        private IMvxStringToTypeParser _parser;

        public IMvxStringToTypeParser Parser
        {
            get
            {
                _parser = _parser ?? Mvx.Resolve<IMvxStringToTypeParser>();
                return _parser;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            _parser = null;
        }

        public bool TypeSupported(Type targetType)
        {
            return Parser.TypeSupported(targetType);
        }

        public object ReadValue(string rawValue, Type targetType, string fieldOrParameterName)
        {
            return Parser.ReadValue(rawValue, targetType, fieldOrParameterName);
        }
    }
}