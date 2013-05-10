namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public class MvxDecodedRestResponse<T>
        : MvxRestResponse
    {
        public T Result { get; set; }
    }
}