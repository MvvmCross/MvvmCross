using Cirrious.CrossCore.Exceptions;
using System;

namespace Cirrious.MvvmCross.WindowsStore.Views.Suspension
{
    public class MvxSuspensionManagerException : MvxException
    {
        public MvxSuspensionManagerException()
        {
        }

        public MvxSuspensionManagerException(Exception e)
            : base(e, "MvxSuspensionManager failed")
        {
        }
    }
}