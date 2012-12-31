#region Copyright

// <copyright file="MvxStopWatch.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

#if !NETFX_CORE

using System;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform.Diagnostics
{
    public class MvxStopWatch
        : IDisposable
          , IMvxServiceConsumer<IMvxEnvironment>
    {
        private readonly string _message;
        private readonly int _startTickCount;
        private readonly string _tag;

        private MvxStopWatch(string tag, string text, params object[] args)
        {
            _tag = tag;
            _startTickCount = Environment.TickCount;
            _message = string.Format(text, args);
        }

        private IMvxEnvironment environment;

        private IMvxEnvironment Environment
        {
            get
            {
                if (environment == null)
                {
                    environment = this.GetService();
                }
                return environment;
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            MvxTrace.TaggedTrace(_tag, "{0} - {1}", Environment.TickCount - _startTickCount, _message);
            GC.SuppressFinalize(this);
        }

        #endregion

        public static MvxStopWatch CreateWithTag(string tag, string text, params object[] args)
        {
            return new MvxStopWatch(tag, text, args);
        }

        public static MvxStopWatch Create(string text, params object[] args)
        {
            return CreateWithTag("mvxStopWatch", text, args);
        }
    }
}

#endif