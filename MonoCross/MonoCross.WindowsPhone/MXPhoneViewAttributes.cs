using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MonoCross.WindowsPhone
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MXPhoneViewAttribute : System.Attribute
    {
        public String Uri { get; set; }

        public MXPhoneViewAttribute(String uri)
        {
            Uri = uri;
        }
    }
}
