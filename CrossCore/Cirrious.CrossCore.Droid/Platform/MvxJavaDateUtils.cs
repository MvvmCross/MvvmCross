// MvxJavaDateUtils.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Droid.Platform
{
    public static class MvxJavaDateUtils
    {
        public static DateTime DateTimeFromJava(int year, int month, int dayOfMonth)
        {
            // +1 needed as Java date months are zero-based
            return new DateTime(year, month + 1, dayOfMonth);
        }
    }
}