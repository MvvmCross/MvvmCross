using Cirrious.Conference.Core.Models.Raw;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.Conference.Core.Models
{
    public class SessionWithFavoriteFlag
        : MvxNotifyPropertyChanged
    {
        public Session Session { get; set; }
        private bool _isFavorite;
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                if (_isFavorite == value)
                    return;
                _isFavorite = value;
                RaisePropertyChanged("IsFavorite");
            }
        }
    }
}