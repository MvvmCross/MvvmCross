// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace MvvmCross.Platforms.Android;

public static class MvxJavaDateUtils
{
    public static DateTime DateTimeFromJava(int year, int month, int dayOfMonth)
    {
        // +1 needed as Java date months are zero-based
        return new DateTime(year, month + 1, dayOfMonth, 0, 0, 0, DateTimeKind.Unspecified);
    }
}
