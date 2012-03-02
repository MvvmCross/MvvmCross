#region Copyright
// <copyright file="MvxGeoLocation.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;

namespace Cirrious.MvvmCross.Interfaces.Platform.Location
{
    public class MvxGeoLocation
    {
        public MvxGeoLocation()
        {
            Coordinates = new MvxCoordinates();
        }

        public MvxCoordinates Coordinates { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}