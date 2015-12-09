// MvxPhoneViewAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Views
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MvxPhoneViewAttribute : Attribute
    {
        public MvxPhoneViewAttribute(String url)
        {
            this.Url = url;
        }

        public string Url { get; private set; }
    }
}