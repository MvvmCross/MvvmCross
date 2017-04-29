// MvxFormsIosPagePresenter.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Martin Nygren, @zzcgumn, zzcgumn@me.com

using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.Forms.Views.iOS
{
    public class MvxPageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer, IMvxBindingContextOwner
    {            
        public IMvxBindingContext BindingContext { get; set;}

        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            MvxPresenterHelpers.AdaptForBinding(Element, this);           
        }
    }

    public class MvxPageRenderer<TViewModel>
        : MvxPageRenderer where TViewModel : class, IMvxViewModel {

        public TViewModel ViewModel {
            get { return BindingContext.DataContext as TViewModel; }
            set { BindingContext.DataContext = value;}
        }
    }
}

