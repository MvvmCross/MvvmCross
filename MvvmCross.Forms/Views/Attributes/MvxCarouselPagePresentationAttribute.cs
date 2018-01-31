// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxCarouselPagePresentationAttribute : MvxPagePresentationAttribute
    {
        public MvxCarouselPagePresentationAttribute(CarouselPosition position = CarouselPosition.Carousel)
        {
            Position = position;
        }

        public CarouselPosition Position { get; set; }
    }

    public enum CarouselPosition
    {
        Root,
        Carousel
    }
}
