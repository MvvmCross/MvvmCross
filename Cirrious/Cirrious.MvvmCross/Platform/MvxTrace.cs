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
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services;

namespace Cirrious.MvvmCross.Platform
{
    public class MvxTrace
        : MvxSingleton<IMvxTrace>
          , IMvxTrace
          , IMvxServiceConsumer<IMvxTrace>
    {
        #region public static Interface

        public static string DefaultTag { get; set; }

        public static void Trace(string tag, string message)
        {
            Instance.Trace(tag, message);
        }

        public static void Trace(string tag, string message, params object[] args)
        {
            Instance.Trace(tag, message, args);
        }

        public static void Trace(string message)
        {
            Instance.Trace(DefaultTag, message);
        }

        public static void Trace(string message, params object[] args)
        {
            Instance.Trace(DefaultTag, message, args);
        }

        #endregion Static Interface

        private readonly IMvxTrace _realTrace;

        static MvxTrace()
        {
            DefaultTag = "mvx";
        }

        public MvxTrace()
        {
            _realTrace = this.GetService();
        }

        #region IMvxTrace Members

        void IMvxTrace.Trace(string tag, string message)
        {
            _realTrace.Trace(tag, message);
        }

        void IMvxTrace.Trace(string tag, string message, params object[] args)
        {
            _realTrace.Trace(tag, message, args);
        }

        #endregion
    }
}