#region Copyright
// <copyright file="MvxRequestedBy.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

#if !NETFX_CORE
using Cirrious.MvvmCross.Platform.Json;
using Newtonsoft.Json;
#endif

namespace Cirrious.MvvmCross.Interfaces.ViewModels
{
    public class MvxRequestedBy
    {
        public static MvxRequestedBy Unknown = new MvxRequestedBy(MvxRequestedByType.Unknown);
        public static MvxRequestedBy Bookmark = new MvxRequestedBy(MvxRequestedByType.Bookmark);
        public static MvxRequestedBy UserAction = new MvxRequestedBy(MvxRequestedByType.UserAction);

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

#if !NETFX_CORE
        [JsonConverter(typeof(MvxGeneralJsonEnumConverter))]
#endif
        public MvxRequestedByType Type { get; set; }
        public string AdditionalInfo { get; set; }
    }
}