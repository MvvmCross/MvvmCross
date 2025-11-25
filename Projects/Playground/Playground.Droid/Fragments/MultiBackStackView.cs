// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using Google.Android.Material.Navigation;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;
using Playground.Core.ViewModels.Navigation;

namespace Playground.Droid.Fragments;

[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
    AllowReordering = true,
    ViewModelType = typeof(MultiBackStackViewModel),
    SetAsPrimaryFragment = true)]
public class MultiBackStackView : MvxFragment<MultiBackStackViewModel>
{
    private NavigationBarView _navigationView;
    private bool _navigatedToTab2;
    
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
	    base.OnCreateView(inflater, container, savedInstanceState);

	    var view = this.BindingInflate(Resource.Layout.MultiBackStackView, null);
	    
	    _navigationView = view.FindViewById<NavigationBarView>(Resource.Id.navigationview);
	    _navigationView.ItemSelected += NavigationViewOnItemSelected;

	    return view;
    }

    public override void OnDestroy()
    {
	    base.OnDestroy();
	    
	    if (_navigationView != null)
	    {
		    _navigationView.ItemSelected -= NavigationViewOnItemSelected;
	    }
    }

	private void NavigationViewOnItemSelected(object sender, NavigationBarView.ItemSelectedEventArgs ev)
	{
	    switch (ev.Item.ItemId)
	    {
		    case Resource.Id.tab1:
			    ChildFragmentManager.SaveBackStack(typeof(MultiBackStackTab2View).FragmentJavaName());
			    ChildFragmentManager.RestoreBackStack(typeof(MultiBackStackTab1View).FragmentJavaName());
			    break;
		    case Resource.Id.tab2:
			    ChildFragmentManager.SaveBackStack(typeof(MultiBackStackTab1View).FragmentJavaName());
			    if (!_navigatedToTab2) {
				    _navigatedToTab2 = true;
				    Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate(typeof(MultiBackStackTab2ViewModel));
			    } else {
				    ChildFragmentManager.RestoreBackStack(typeof(MultiBackStackTab2View).FragmentJavaName());
			    }
			    break;
	    }
	}
}

[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
    FragmentHostViewType = typeof(MultiBackStackView),
    AllowReordering = true,
    ViewModelType = typeof(MultiBackStackTab1ViewModel))]
public class MultiBackStackTab1View : MvxFragment<MultiBackStackTab1ViewModel>
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
	    base.OnCreateView(inflater, container, savedInstanceState);

	    var view = this.BindingInflate(Resource.Layout.MultiBackStackTab1View, null);

	    return view;
    }

}

[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
    FragmentHostViewType = typeof(MultiBackStackView),
    AllowReordering = true,
    ViewModelType = typeof(MultiBackStackTab2ViewModel))]
public class MultiBackStackTab2View : MvxFragment<MultiBackStackTab2ViewModel>
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
	    base.OnCreateView(inflater, container, savedInstanceState);

	    var view = this.BindingInflate(Resource.Layout.MultiBackStackTab2View, null);

	    return view;
    }
}

[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
    FragmentHostViewType = typeof(MultiBackStackView),
    AllowReordering = true,
    ViewModelType = typeof(MultiBackStackInnerViewModel))]
public class MultiBackStackInnerView : MvxFragment<MultiBackStackInnerViewModel>, IMvxOverridePresentationAttribute
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
	    base.OnCreateView(inflater, container, savedInstanceState);

	    var view = this.BindingInflate(Resource.Layout.MultiBackStackInnerView, null);

	    var f = ParentFragmentManager.PrimaryNavigationFragment;
	    return view;
    }

    public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
    {
	    if (request is MvxViewModelInstanceRequest { ViewModelInstance: MultiBackStackInnerViewModel viewModel, ViewModelType: { } viewModelType })
	    {
		    return new MvxFragmentPresentationAttribute()
		    {
			    ViewModelType = typeof(MultiBackStackInnerViewModel),
			    ActivityHostViewModelType = typeof(RootViewModel),
			    FragmentHostViewType = typeof(MultiBackStackView),
			    FragmentContentId = Resource.Id.content_frame,
			    AddToBackStack = true,
			    AllowReordering = true,
			    Tag = viewModelType.Name + viewModel.Depth // unique tag so the restoration restores all of them
		    };
	    }
	    else
	    {
		    return null;
	    }
    }
}