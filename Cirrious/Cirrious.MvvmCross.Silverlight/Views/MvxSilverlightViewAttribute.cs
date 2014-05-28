using System;

namespace Cirrious.MvvmCross.Silverlight.Views
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MvxSilverlightViewAttribute : Attribute
    {
        public MvxSilverlightViewAttribute(String url)
        {
            Url = url;
        }

        public string Url { get; private set; }
    }
}