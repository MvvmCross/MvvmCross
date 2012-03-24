using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.Conference.Core.ViewModels;

namespace Cirrious.Conference.UI.Droid.Views
{
    [Activity(Label = "SqlBits", NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenView : BaseView<SplashScreenViewModel>
    {
        public class SplashCountDownTimer : CountDownTimer
        {
            private readonly Action _finished;
            public SplashCountDownTimer(Action finished) 
                : base(2000, 2000)
            {
                _finished = finished;
            }

            public override void OnFinish()
            {
                _finished();
            }

            public override void OnTick(long millisUntilFinished)
            {
                // ignored
            }
        }

        private CountDownTimer _timer;

        protected override void OnViewModelSet()
        {
            this.SetContentView(Resource.Layout.Page_SplashScreen);
            _timer = new SplashCountDownTimer(SplashAnimationFinished);
            _timer.Start();
        }

        private void SplashAnimationFinished()
        {
            ViewModel.SplashScreenComplete = true;
        }
    }
}