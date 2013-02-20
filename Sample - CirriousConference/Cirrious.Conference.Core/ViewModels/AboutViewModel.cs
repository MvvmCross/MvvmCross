using System.Windows.Input;
using Cirrious.CrossCore.Commands;

namespace Cirrious.Conference.Core.ViewModels
{
    public class AboutViewModel
        : BaseConferenceViewModel
    {
        public ICommand ContactSlodgeCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ComposeEmail("me@slodge.com", "About MvvmCross and the SQL Bits app", "I've got a question"));
            }
        }

        public ICommand MvvmCrossOnGithubCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ShowWebPage("http://github.com/slodge/mvvmcross"));
            }
        }

        public ICommand ShowSqlBitsCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ShowWebPage("http://sqlbits.com"));
            }
        }
        
        public ICommand MonoTouchCommand
        {
            get
            {
                return
                    new MvxRelayCommand(
                        () =>
                        ShowWebPage("http://ios.xamarin.com"));
            }
        }

        public ICommand MonoDroidCommand
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