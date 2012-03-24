using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace Cirrious.Conference.Core.ViewModels
{
    public class AboutViewModel
        : BaseConferenceViewModel
    {
        public IMvxCommand ContactSlodgeCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ComposeEmail("me@slodge.com", "About MvvmCross and the SQL Bits app", "I've got a question"));
            }
        }

        public IMvxCommand MvvmCrossOnGithubCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ShowWebPage("http://github.com/slodge/mvvmcross"));
            }
        }

        public IMvxCommand ShowSqlBitsCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ShowWebPage("http://sqlbits.com"));
            }
        }
        
        public IMvxCommand MonoTouchCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ShowWebPage("http://ios.xamarin.com"));
            }
        }

        public IMvxCommand MonoDroidCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ShowWebPage("http://android.xamarin.com"));
            }
        }
    }
}