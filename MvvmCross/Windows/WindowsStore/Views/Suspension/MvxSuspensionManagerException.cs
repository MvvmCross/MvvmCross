namespace MvvmCross.WindowsStore.Views.Suspension
{
    using System;

    using MvvmCross.Platform.Exceptions;

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