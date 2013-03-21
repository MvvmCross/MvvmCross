using System;

namespace Cirrious.Conference.Core.ViewModels
{
    public class SplashScreenViewModel
        : BaseConferenceViewModel
    {
        public SplashScreenViewModel()
        {
            SplashScreenComplete = false;
            this.PropertyChanged += (sender, args) =>
                                        {
                                            if (args.PropertyName == "IsSearching")
                                            {
                                                MoveForwardsIfPossible();
                                            }
                                        };
        }

        private bool _splashScreenComplete;
        public bool SplashScreenComplete
        {
            get { return _splashScreenComplete; }
            set 
            { 
                _splashScreenComplete = value;
                MoveForwardsIfPossible();
            }
        }

        private void MoveForwardsIfPossible()
        {
            if (IsLoading || !SplashScreenComplete)
                return;

            ShowViewModel<HomeViewModel>(true);           
        }
    }
}