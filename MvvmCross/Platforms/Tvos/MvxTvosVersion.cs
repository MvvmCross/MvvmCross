// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Exceptions;

namespace MvvmCross.Platforms.Tvos
{
    public class MvxTvosVersion
    {
        public MvxTvosVersion(int[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new MvxException("Invalid parts in constructor for MvxTvosVersion");

            Parts = parts;
            Major = parts[0];

            if (parts.Length > 1)
            {
                Minor = parts[1];
            }
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int[] Parts { get; private set; }
    }
}
