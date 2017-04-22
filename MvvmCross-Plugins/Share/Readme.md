### Share

The `Share` plugin provides implementations of:

    public interface IMvxShareTask
    {
        void ShareShort(string message);
        void ShareLink(string title, string message, string link);
    }
    
This plugin is available on Android, iOS, and WindowsPhone. On WindowsStore, sharing is done more by UI-based sharing (swipe in from the right).

On Android, sharing is done using general Share/Send Intents. This could be improved in future implementations.
On WindowsPhone, sharing is done via the OS level share task. 
On iOS, currently only sharing by linked Twitter account is supported. There is code available to extend this to Facebook - see https://github.com/slodge/MvvmCross/issues/188.

A sample using the Share plugin is:

- Conference - see https://github.com/slodge/MvvmCross-Tutorials/tree/master/Sample%20-%20CirriousConference