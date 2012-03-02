#region Copyright
// <copyright file="MvxTouchOptions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using MonoTouch.UIKit;

#warning Currently unused
namespace Cirrious.MvvmCross.Touch.Options
{	
	
	public class MvxTouchTabletOptions : MvxTouchOptions
	{
	    private bool _allowDividerResize = false;
	    private bool _masterBeforeDetail = true;
	    private String _masterButtonText = "Master";
	    private bool _masterShowsInLandscape = true;
	    private bool _masterShowsInPortrait = false;

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

	    public bool MasterShowsinLandscape
		{
			get { return _masterShowsInLandscape; }
			set { _masterShowsInLandscape = value; }
		}

	    public bool MasterBeforeDetail 
		{
			get { return _masterBeforeDetail; }
			set { _masterBeforeDetail = value; }
		}

	    public bool AllowDividerResize
		{
			get { return _allowDividerResize; }
			set { _allowDividerResize = value; }
		}

	    public String MasterButtonText
		{
			get { return _masterButtonText; }
			set { _masterButtonText = value; }
		}
	}
	
	public class MvxTouchOptions
	{
	    public UIColor NavigationBarTintColor = UIColor.Clear;

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

	    //public String Icon { get; set; }
	}	
}

