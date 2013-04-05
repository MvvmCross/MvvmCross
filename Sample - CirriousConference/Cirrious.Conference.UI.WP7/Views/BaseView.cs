using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.Conference.Core.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;

namespace Cirrious.Conference.UI.WP7.Views
{
    public abstract class BaseView<TViewModel>
        : MvxPhonePage
        where TViewModel : BaseViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected BaseView()
        {            
            var navInTransition = new NavigationInTransition()
                                  {
                                      Backward = new TurnstileTransition {Mode = TurnstileTransitionMode.BackwardIn},
                                      Forward = new TurnstileTransition {Mode = TurnstileTransitionMode.ForwardIn}
                                  };
            var navOutTransition = new NavigationOutTransition()
            {
                Backward = new TurnstileTransition { Mode = TurnstileTransitionMode.BackwardOut },
                Forward = new TurnstileTransition { Mode = TurnstileTransitionMode.ForwardOut }
            };

            TransitionService.SetNavigationInTransition(this, navInTransition);
            TransitionService.SetNavigationOutTransition(this, navOutTransition);
        }

    }
}
