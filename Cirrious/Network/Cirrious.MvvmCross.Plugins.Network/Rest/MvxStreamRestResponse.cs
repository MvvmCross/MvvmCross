using System.IO;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public class MvxStreamRestResponse
        : MvxRestResponse
    {
        public Stream Stream { get; set; }
    }
}