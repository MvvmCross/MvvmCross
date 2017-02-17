---
layout: documentation
title: Network
category: Plugins
---
The original purpose of the `Network` plugin was to provide `IMvxReachability` **on iOS only**
```c# 
public interface IMvxReachability\n{\n  bool IsHostReachable(string host);\n}",
```
**Note:** this interface is currently implemented on iOS only, although some contributors are working on other platforms (e.g. for Android see https://github.com/slodge/MvvmCross/issues/362)

Since this original purpose, Network has now further been expanded to provide a simple Rest implementation - and this is available on Droid, Touch, WindowsPhone, WindowsStore and Wpf.

The Rest client is mainly implemented using the `IMvxRestClient` and `IMvxJsonRestClient` interfaces
```c# 
public interface IMvxRestClient\n{\n  void ClearSetting(string key);\n  void SetSetting(string key, object value);\n\n  void MakeRequest(MvxRestRequest restRequest, Action<MvxRestResponse> successAction,\n                   Action<Exception> errorAction);\n\n  void MakeRequest(MvxRestRequest restRequest, Action<MvxStreamRestResponse> successAction,\n                   Action<Exception> errorAction);\n}\n\npublic interface IMvxJsonRestClient\n{\n  Func<IMvxJsonConverter> JsonConverterProvider { get; set; }\n\n  void MakeRequestFor<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction,\n                         Action<Exception> errorAction);\n}",
```
These are supported by a small set of `Request` and `Response` classes:

- `MvxRestRequest`, `MvxStreamRestRequest`, `MvxJsonRestRequest`, `MvxStringRestRequest`, `MvxStringRestRequest`, `MvxMultiPartFormRestRequest` and `MvxWwwFormRequest`
- `MvxRestResponse`, `MvxStreamRestResponse`, `MvxJsonRestResponse`

To make a simple fixed url Rest request, you can use:
```c# 
var request = new MvxRestRequest(\"http://myService.org/things/list\");\nvar client = Mvx.Resolve<IMvxRestClient>();\nclient.MakeRequest(request,\n                   (MvxStreamRestResponse response) => {\n                     // do something with the response.StatusCode and response.Stream\n                   },\n                   error => {\n                     // do something with the error\n                   });",
```
To use the Json APIs, you must have an `IMvxJsonConverter` implementation available - one way to get this is to load the Json plugin. With this in place, a simple Json upload with Json response might look like:
```c# 
var request = new MvxJsonRestRequest<Person>(\"http://myService.org/things/add\")\n{\n  Body = person\n};\n\nvar client = Mvx.Resolve<IMvxJsonRestClient>();\nclient.MakeRequestFor<PersonAddResult>(request,\n                                       (MvxDecodedRestResponse<PersonAddResult> response) => {\n                                         // do something with the response.StatusCode and response.Result\n                                       },\n                                       error => {\n                                         // do something with the error\n                                       });",
```
Note:

- This Rest module is a 'light-weight' implementation which works for many simple Rest web services.
- For more advanced web service requirements, consider extending the classes offered here or consider importing other more established Rest libraries such as RestSharp (http://restsharp.org/).