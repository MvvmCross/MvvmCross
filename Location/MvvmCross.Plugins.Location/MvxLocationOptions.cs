// MvxLocationOptions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Location
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