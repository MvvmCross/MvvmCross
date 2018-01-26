namespace System.Diagnostics
{
    //
    // Summary:
    //     Identifies the type of event that has caused the trace.
    public enum TraceEventType
    {
        //
        // Summary:
        //     Fatal error or application crash.
        Critical = 1,
        //
        // Summary:
        //     Recoverable error.
        Error = 2,
        //
        // Summary:
        //     Noncritical problem.
        Warning = 4,
        //
        // Summary:
        //     Informational message.
        Information = 8,
        //
        // Summary:
        //     Debugging trace.
        Verbose = 16,
        //
        // Summary:
        //     Starting of a logical operation.
        Start = 256,
        //
        // Summary:
        //     Stopping of a logical operation.
        Stop = 512,
        //
        // Summary:
        //     Suspension of a logical operation.
        Suspend = 1024,
        //
        // Summary:
        //     Resumption of a logical operation.
        Resume = 2048,
        //
        // Summary:
        //     Changing of correlation identity.
        Transfer = 4096
    }
}
