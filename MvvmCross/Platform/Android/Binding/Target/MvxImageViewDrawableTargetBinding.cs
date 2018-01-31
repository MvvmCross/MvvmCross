﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.Target
{
	public class MvxImageViewDrawableTargetBinding
		: MvxAndroidTargetBinding
	{
		protected ImageView ImageView => (ImageView)Target;

		public MvxImageViewDrawableTargetBinding(ImageView imageView)
			: base(imageView)
		{
		}

		public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

		public override Type TargetType => typeof(int);

		protected override void SetValueImpl(object target, object value)
		{
			var imageView = (ImageView)target;

			if (!(value is int))
			{
				MvxBindingLog.Warning(
					"Value was not a valid Drawable");
				imageView.SetImageDrawable(null);
				return;
			}

			var intValue = (int)value;

			if (intValue == 0)
				imageView.SetImageDrawable(null);
			else
				SetImage(imageView, intValue);
		}

		protected virtual void SetImage(ImageView imageView, int id)
		{
			var context = imageView.Context;
			Drawable drawable;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
				drawable = context?.Resources?.GetDrawable(id, context.Theme);
			else
#pragma warning disable CS0618 // Type or member is obsolete
                drawable = context?.Resources?.GetDrawable(id);
#pragma warning restore CS0618 // Type or member is obsolete

            if (drawable != null)
				imageView.SetImageDrawable(drawable);
		}
	}
}