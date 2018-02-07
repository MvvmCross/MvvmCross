// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;

namespace MvvmCross.Plugin.Accelerometer
{
    [Preserve(AllMembers = true)]
    public class MvxAccelerometerReading
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public MvxAccelerometerReading Clone()
        {
            return new MvxAccelerometerReading { X = X, Y = Y, Z = Z };
        }
    }
}
