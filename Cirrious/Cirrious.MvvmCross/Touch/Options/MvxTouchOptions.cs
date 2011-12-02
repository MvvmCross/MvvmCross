#region Copyright

// <copyright file="MvxTouchOptions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

using Cirrious.MvvmCross.Touch.ExtensionMethods;

#warning Currently unused
namespace Cirrious.MvvmCross.Touch.Options
{	
	
	public class MvxTouchTabletOptions : MvxTouchOptions
	{
		public MvxTouchTabletOptions(MvxTabletLayout tabletLayout)
		{
			TabletLayout = tabletLayout;
		}

		public MvxTabletLayout TabletLayout { get; set; }
		
		public bool MasterShowsinPotrait
		{
			get { return _masterShowsInPortrait; }
			set { _masterShowsInPortrait = value; }
		}
		private bool _masterShowsInPortrait = false;

		public bool MasterShowsinLandscape
		{
			get { return _masterShowsInLandscape; }
			set { _masterShowsInLandscape = value; }
		}
		private bool _masterShowsInLandscape = true;
		
		public bool MasterBeforeDetail 
		{
			get { return _masterBeforeDetail; }
			set { _masterBeforeDetail = value; }
		}
		private bool _masterBeforeDetail = true;

		public bool AllowDividerResize
		{
			get { return _allowDividerResize; }
			set { _allowDividerResize = value; }
		}
		private bool _allowDividerResize = false;
		
		public String MasterButtonText
		{
			get { return _masterButtonText; }
			set { _masterButtonText = value; }
		}
		private String _masterButtonText = "Master";
	}
	
	public class MvxTouchOptions
	{
		public MvxTouchOptions() 
		{
			NavigationBarTintColor = UIColor.Clear; // navigation bar has to have a color, hence this means leave as default
		}

		public String SplashBitmap { get; set; } 

		public uint NavigationBarTint
		{
			get
			{
				return NavigationBarTintColor.IntFromColor();
			}
			set
			{
				NavigationBarTintColor = value.ColorWithAlphaFromInt();
			}
		}

		public UIColor NavigationBarTintColor = UIColor.Clear;

		//public String Icon { get; set; }
	}	
}

