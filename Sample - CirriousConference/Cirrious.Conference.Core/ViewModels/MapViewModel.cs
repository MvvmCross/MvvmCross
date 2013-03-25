using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels
{
    public class MapViewModel
        : BaseViewModel
    {

        public string Name { get { return "Novotel London West"; } }
        public string Address { get { return "London W6 8DR"; }}
        public string LocationWebPage { get { return "http://www.novotellondonwest.co.uk/location/index.htm"; } }

        public string Phone { get { return "+44 (0)208 741 1555"; } }
        public string Email { get { return "H0737@accor.com"; } }
        public double Latitude { get { return 51.491351; } }
        public double Longitude { get { return -0.222044; } }

        public ICommand PhoneCommand
        {
            get
            {
                return new MvxCommand(() => MakePhoneCall(Name, Phone));
            }
        }

        public ICommand EmailCommand
        {
            get
            {
                return new MvxCommand(() => ComposeEmail(Email, "About SQLBits X", "About SQLBits X"));
            }
        }

        public ICommand WebPageCommand
        {
            get
            {
                return new MvxCommand(() => ShowWebPage(LocationWebPage));
            }
        }
    }
}