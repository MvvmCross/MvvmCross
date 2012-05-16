#region Copyright
// <copyright file="MvxAndroidSetupSingleton.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Linq;
using Android.Content;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Android.Platform
{
    public class MvxAndroidSetupSingleton
    {
        private static readonly object LockObject = new object();
        private static MvxBaseAndroidSetup _instance;

        public static MvxBaseAndroidSetup GetOrCreateSetup(Context applicationContext)
        {
            if (_instance != null)
            {
                return _instance;
            }

            lock (LockObject)
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var setupType = FindSetupType();
                if (setupType == null)
                {
                    throw new MvxException("Could not find a Setup class for application");
                }

                try
                {
                    _instance = (MvxBaseAndroidSetup)Activator.CreateInstance(setupType, applicationContext);
                }
                catch (Exception exception)
                {                    
                    throw exception.MvxWrap("Failed to create instance of {0}", setupType.FullName);
                }

                return _instance;
            }
        }

        private static Type FindSetupType()
        {
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.GetTypes()
                        where type.Name == "Setup"
                        where typeof (MvxBaseAndroidSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }
    }
}