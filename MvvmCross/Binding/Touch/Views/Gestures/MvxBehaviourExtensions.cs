﻿// MvxBehaviourExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views.Gestures
{
    public static class MvxBehaviourExtensions
    {
        public static MvxTapGestureRecognizerBehaviour Tap(this UIView view, uint numberOfTapsRequired = 1,
                                                           uint numberOfTouchesRequired = 1)
        {
            var toReturn = new MvxTapGestureRecognizerBehaviour(view, numberOfTapsRequired, numberOfTouchesRequired);
            return toReturn;
        }
    }
}