// MvxContentType.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public static class MvxContentType
    {
        public const string Json = "application/json";
        public const string WwwForm = "application/x-www-form-urlencoded";
        public const string MultipartFormWithBoundary = "multipart/form-data; boundary=";
        //public const string Xml = "application/xml";
    }
}