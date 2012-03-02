#region Copyright
// <copyright file="MvxViewClassNameResolver.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Text;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Binders;

namespace Cirrious.MvvmCross.Binding.Android.Binders
{
#warning Kill this class
    public class MvxViewClassNameResolver : IMvxViewClassNameResolver
    {
        #region IMvxViewClassNameResolver Members

        public virtual string Resolve(string tagName)
        {
            var nameBuilder = new StringBuilder();

            /*
             * looking at:
             * http://grepcode.com/file/repository.grepcode.com/java/ext/com.google.android/android/2.3.4_r1/android/view/LayoutInflater.java#LayoutInflater.onCreateView%28java.lang.String%2Candroid.util.AttributeSet%29
            if (!IsFullyQualified(tagName))
                nameBuilder.Append("android.view.");
            */
            switch (tagName)
            {
                case "View":
                case "ViewGroup":
                    nameBuilder.Append("Android.View.");
                    break;

                default:
                    if (!IsFullyQualified(tagName))
                        nameBuilder.Append("Android.Widget.");
                    break;
            }

            nameBuilder.Append(tagName);
            return nameBuilder.ToString();
        }

        #endregion

        private static bool IsFullyQualified(string tagName)
        {
            return tagName.Contains(".");
        }
    }
}