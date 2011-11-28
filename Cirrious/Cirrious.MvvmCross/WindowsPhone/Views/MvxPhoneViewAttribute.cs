using System;

namespace Cirrious.MvvmCross.WindowsPhone.Views
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MvxPhoneViewAttribute : System.Attribute
    {
        public string Url { get; private set; }

        public MvxPhoneViewAttribute(String url)
        {
            Url = url;
        }
    }
}
