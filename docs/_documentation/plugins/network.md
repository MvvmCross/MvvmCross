---
layout: documentation
title: Network
category: Plugins
---

> Note: This plugin was removed in MvvmCross 7.1.0


The purpose of the `Network` plugin is to provide an implementation `IMvxReachability`. It is currently implemented for iOS, Android, UWP and WPF.

```c#
public interface IMvxReachability
{
    bool IsHostReachable(string host);
}
```

Since this original purpose, Network has now further been expanded to provide a simple Rest implementation - and this is available on Android, iOS, Windows UWP and WPF.

The Rest client is mainly implemented using the `IMvxRestClient` and `IMvxJsonRestClient` interfaces

```c#
public interface IMvxRestClient
{
    void ClearSetting(string key);
    void SetSetting(string key, object value);

    void MakeRequest(MvxRestRequest restRequest, Action<MvxRestResponse> successAction,
                     Action<Exception> errorAction);

    void MakeRequest(MvxRestRequest restRequest, Action<MvxStreamRestResponse> successAction,
                     Action<Exception> errorAction);
}

public interface IMvxJsonRestClient
{
    Func<IMvxJsonConverter> JsonConverterProvider {
        get;
        set;
    }

    void MakeRequestFor<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction,
                           Action<Exception> errorAction);
}
```

These are supported by a small set of `Request` and `Response` classes:

- `MvxRestRequest`, `MvxStreamRestRequest`, `MvxJsonRestRequest`, `MvxStringRestRequest`, `MvxStringRestRequest`, `MvxMultiPartFormRestRequest` and `MvxWwwFormRequest`
- `MvxRestResponse`, `MvxStreamRestResponse`, `MvxJsonRestResponse`

To make a simple fixed url Rest request, you can use:

```c#
var request = new MvxRestRequest("http://myService.org/things/list");
var client = Mvx.IoCProvider.Resolve<IMvxRestClient>();
client.MakeRequest(request,
(MvxStreamRestResponse response) => {
    // do something with the response.StatusCode and response.Stream
},
error => {
    // do something with the error
});
```

To use the Json APIs, you must have an `IMvxJsonConverter` implementation available - one way to get this is to load the Json plugin. With this in place, a simple Json upload with Json response might look like:

```c#
var request = new MvxJsonRestRequest<Person>("http://myService.org/things/add")
{
    Body = person
};

var client = Mvx.IoCProvider.Resolve<IMvxJsonRestClient>();
client.MakeRequestFor<PersonAddResult>(request,
(MvxDecodedRestResponse<PersonAddResult> response) => {
    // do something with the response.StatusCode and response.Result
},
error => {
    // do something with the error
});
```

Note:

- This Rest module is a 'light-weight' implementation which works for many simple Rest web services.
- For more advanced web service requirements, consider extending the classes offered here or consider importing other more established Rest libraries such as RestSharp (http://restsharp.org/).

