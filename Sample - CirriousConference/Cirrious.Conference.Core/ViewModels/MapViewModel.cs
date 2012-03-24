using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.Platform.Tasks;

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

        public IMvxCommand PhoneCommand
        {
            get
            {
                return new MvxRelayCommand(() => MakePhoneCall(Name, Phone));
            }
        }

        public IMvxCommand EmailCommand
        {
            get
            {
                return new MvxRelayCommand(() => ComposeEmail(Email, "About SQLBits X", "About SQLBits X"));
            }
        }

        public IMvxCommand WebPageCommand
        {
            get
            {
                return new MvxRelayCommand(() => ShowWebPage(LocationWebPage));
            }
        }
    }
}