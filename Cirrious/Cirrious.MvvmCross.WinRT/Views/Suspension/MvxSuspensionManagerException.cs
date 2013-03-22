using System;
using Cirrious.CrossCore.Exceptions;

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