---
layout: documentation
title: PhoneCall
category: Plugins
---
The `PhoneCall` plugin provides implementations of:

```c#
public interface IMvxPhoneCallTask
{
    void MakePhoneCall(string name, string number);
}
```

The PhoneCall plugin is available on Android, iOS, and WindowsPhone with a Skype-based implementation on WindowsStore.

The PhoneCall plugin is very simple - e.g. it doesn't provide any detection of whether or not a phone call is currently possible - e.g. for flight mode or for iPod-type devices without cell connectivity.

Sample using the PhoneCall plugin include:

- CustomerManagement - https://github.com/slodge/MvvmCross-Tutorials/tree/master/Sample%20-%20CustomerManagement
- Conference - https://github.com/slodge/MvvmCross-Tutorials/tree/master/Sample%20-%20CirriousConference

