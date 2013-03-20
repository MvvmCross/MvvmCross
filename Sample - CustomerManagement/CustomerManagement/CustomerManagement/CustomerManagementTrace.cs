using Cirrious.CrossCore.Platform;

namespace CustomerManagement.Core
{
    public static class CustomerManagementTrace
    {
        public const string Tag = "CustomerManagement";

        public static void Trace(string message, params object[] args)
        {
            MvxTrace.TaggedTrace(Tag, message, args);
        }
    }
}
