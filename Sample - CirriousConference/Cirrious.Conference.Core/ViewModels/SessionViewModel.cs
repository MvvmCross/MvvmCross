using System.Windows.Input;
using Cirrious.Conference.Core.Models;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Plugins.Share;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels
{
    public class SessionViewModel
        : BaseConferenceViewModel
        , IMvxConsumer
    {
        public SessionViewModel(string key)
        {
            SessionWithFavoriteFlag session;
            if (!Service.Sessions.TryGetValue(key, out session))
            {
#warning TODO - report error!
                RequestClose(this);
                return;
            }

            Session = session;
        }

        public SessionWithFavoriteFlag Session { get; private set; }

        public ICommand ShareCommand
        {
            get { return new MvxRelayCommand(DoShare); }
        }

        public void DoShare()
        {
            var service = this.Resolve<IMvxShareTask>();
            var toShare = string.Format("#SQLBitsX: {0} - {1}", Session.Session.SpeakerKey, Session.Session.Title);
            if (toShare.Length > 140)
                toShare = toShare.Substring(0, 135).Trim() + "...";
            service.ShareShort(toShare);
        }
    }
}