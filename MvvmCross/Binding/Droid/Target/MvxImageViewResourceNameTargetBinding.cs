// MvxImageViewResourceNameTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Widget;

namespace MvvmCross.Binding.Droid.Target
{
	public class MvxImageViewResourceNameTargetBinding : MvxImageViewDrawableNameTargetBinding
	{
		public MvxImageViewResourceNameTargetBinding(ImageView imageView)
			: base(imageView)
		{
		}

		protected override void SetImage(ImageView imageView, int id)
		{
			imageView.SetImageResource(id);
		}
	}
}