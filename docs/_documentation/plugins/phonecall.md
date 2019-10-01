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

## Setup

Install the ```MvvmCross.Plugin.PhoneCall``` nuget in the Core and the platforms you want to support. After that you can inject the ```IMvxPhoneCallTask``` in your constructor and use it:

```c#
public class MyViewModel : MvxViewModel
{
    public MyViewModel(IMvxPhoneCallTask phoneCallTask)
    {
        phoneCallTask.MakePhoneCall("Contact name", "+310631798511");
    }
}
```


