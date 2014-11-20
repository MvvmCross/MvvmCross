using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cirrious.MvvmCross.WindowsCommon.Views
{
    [DataContract]
    internal class MvxWindowsFrameDescription
    {
        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public List<MvxWindowsPageDescription> PageStack { get; set; }
    }
}