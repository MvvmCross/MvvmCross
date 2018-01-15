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
