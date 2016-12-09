### Network

The original purpose of the `Network` plugin was to provide `IMvxReachability` **on iOS only**

    public interface IMvxReachability
    {
        bool IsHostReachable(string host);
    }

**Note:** this interface is currently implemented on iOS only, although some contributors are working on other platforms (e.g. for Android see https://github.com/slodge/MvvmCross/issues/362)

Since this original purpose, Network has now further been expanded to provide a simple Rest implementation - and this is available on Droid, Touch, WindowsPhone, WindowsStore and Wpf.

The Rest client is mainly implemented using the `IMvxRestClient` and `IMvxJsonRestClient` interfaces

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
        Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        void MakeRequestFor<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction,
                               Action<Exception> errorAction);
    }

These are supported by a small set of `Request` and `Response` classes:

- `MvxRestRequest`, `MvxStreamRestRequest`, `MvxJsonRestRequest`, `MvxStringRestRequest`, `MvxStringRestRequest`, `MvxMultiPartFormRestRequest` and `MvxWwwFormRequest`
- `MvxRestResponse`, `MvxStreamRestResponse`, `MvxJsonRestResponse`

To make a simple fixed url Rest request, you can use:

    var request = new MvxRestRequest("http://myService.org/things/list");
    var client = Mvx.Resolve<IMvxRestClient>();
    client.MakeRequest(request,
         (MvxStreamRestResponse response) => {
             // do something with the response.StatusCode and response.Stream
         },
         error => {
         	   // do something with the error
         });

To use the Json APIs, you must have an `IMvxJsonConverter` implementation available - one way to get this is to load the Json plugin. With this in place, a simple Json upload with Json response might look like:

    var request = new MvxJsonRestRequest<Person>("http://myService.org/things/add")
    {
        Body = person
    };
    
    var client = Mvx.Resolve<IMvxJsonRestClient>();
    client.MakeRequestFor<PersonAddResult>(request,
         (MvxDecodedRestResponse<PersonAddResult> response) => {
             // do something with the response.StatusCode and response.Result
         },
         error => {
         	   // do something with the error
         });

Note:

- This Rest module is a 'light-weight' implementation which works for many simple Rest web services.
- For more advanced web service requirements, consider extending the classes offered here or consider importing other more established Rest libraries such as RestSharp (http://restsharp.org/).