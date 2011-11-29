#region Copyright
// <copyright file="MvxTrace.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services;

namespace Cirrious.MvvmCross.Platform
{
    public class MvxTrace 
            : MvxSingleton<IMvxTrace>
            , IMvxTrace
            , IMvxServiceConsumer<IMvxTrace>
    {
        private readonly IMvxTrace _realTrace;

        public MvxTrace()
        {
            _realTrace = this.GetService();
        }

        public static void Trace(string message)
        {
            Instance.Trace(message);
        }

        public static void Trace(string message, params object[] args)
        {
            Instance.Trace(message, args);
        }

        void IMvxTrace.Trace(string message)
        {
            _realTrace.Trace(message);
        }

        void IMvxTrace.Trace(string message, params object[] args)
        {
            _realTrace.Trace(message, args);
        }
    }
}