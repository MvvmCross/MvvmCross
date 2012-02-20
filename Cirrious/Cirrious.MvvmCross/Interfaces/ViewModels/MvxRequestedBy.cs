using Cirrious.MvvmCross.Json;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Interfaces.ViewModels
{
    public class MvxRequestedBy
    {
        public static MvxRequestedBy Unknown = new MvxRequestedBy(MvxRequestedByType.Unknown);
        public static MvxRequestedBy Bookmark = new MvxRequestedBy(MvxRequestedByType.Bookmark);
        public static MvxRequestedBy UserAction = new MvxRequestedBy(MvxRequestedByType.UserAction);

        [JsonConverter(typeof(MvxGeneralJsonEnumConverter))]
        public MvxRequestedByType Type { get; set; }
        public string AdditionalInfo { get; set; }

        public MvxRequestedBy()
            : this(MvxRequestedByType.Unknown)
        {           
        }

        public MvxRequestedBy(MvxRequestedByType requestedByType)
            : this(requestedByType, null)
        {            
        }

        public MvxRequestedBy(MvxRequestedByType requestedByType, string additionalInfo)
        {
            Type = requestedByType;
            AdditionalInfo = additionalInfo;
        }
    }
}