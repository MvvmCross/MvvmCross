using System.Windows.Input;
using Cirrious.Conference.Core.Models;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Share;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.ViewModels
{
    public class SessionViewModel
        : BaseConferenceViewModel
    {
        private string _key;

        public void Init(string key)
        {
            _key = key;
        }

        public override void Start()
        {
            ShowSession();
            base.Start();
        }

        protected override void RepositoryOnLoadingChanged()
        {
            ShowSession();
            base.RepositoryOnLoadingChanged();
        }

        private void ShowSession()
        {
            SessionWithFavoriteFlag session;
            if (Service.Sessions == null 
                || !Service.Sessions.TryGetValue(_key, out session))
            {
                // TODO - some kind of error notification would be nice
                return;
            }

            Session = session;
        }

        public SessionWithFavoriteFlag Session { get; private set; }

        public ICommand ShareCommand
        {
            get { return new MvxCommand(DoShare); }
        }

        public void DoShare()
        {
            if (Session == null)
                return;

            var service = Mvx.Resolve<IMvxShareTask>();
            var toShare = string.Format("#SQLBitsX: {0} - {1}", Session.Session.SpeakerKey, Session.Session.Title);
            if (toShare.Length > 140)
                toShare = toShare.Substring(0, 135).Trim() + "...";
            service.ShareShort(toShare);
        }
    }
}