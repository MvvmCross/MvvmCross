// MvxAndroidSetupSingleton.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Android.Content;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Droid.Platform
{
#warning WHy does this not inherit from MvxSingleton?
    public class MvxAndroidSetupSingleton
    {
        private static readonly object LockObject = new object();
        private static MvxAndroidSetup _instance;

        public static MvxAndroidSetup GetOrCreateSetup(Context applicationContext)
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
                    _instance = (MvxAndroidSetup) Activator.CreateInstance(setupType, applicationContext);
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
                        where typeof (MvxAndroidSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }
    }
}