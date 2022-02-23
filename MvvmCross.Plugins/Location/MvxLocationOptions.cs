// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugin.Location
{
    [Preserve(AllMembers = true)]
    public class MvxLocationOptions
    {
        public MvxLocationAccuracy Accuracy { get; set; }

        /// <summary>
        /// Use TimeSpan.Zero for most frequent updates
        /// </summary>
        public TimeSpan TimeBetweenUpdates { get; set; }

        /// <summary>
        /// Use 0 threshold for most frequent updates
        /// </summary>
        public int MovementThresholdInM { get; set; }

        /// <summary>
        /// Use MvxLocationTrackingMode.Background to enable location tracking in background.
        /// </summary>
        public MvxLocationTrackingMode TrackingMode { get; set; }
    }
}
