using System.Windows.Input;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using Example.Models;
using Example.Services;
using MvvmCross.Plugins.WebBrowser;

namespace Example.ViewModels
{
    public class DetailedMovieViewModel 
        : MvxViewModel
    {
        private int _movieId;
        private DetailedMovie _model;

        public void Init(int movieId)
        {
            _movieId = movieId;
        }

        public override async void Start()
        {
            base.Start();

            if (_model != null) return;

            _model = await _service.DetailedMovieFromId(_movieId);
            Title = _model.OriginalTitle;
            Score = _model.Score;
            PosterUrl = _model.PosterUrl;
            ReleaseDate = _model.ReleaseDate;
            VoteCount = _model.VoteCount;
            ImdbUrl = _model.ImdbId;
            Synopsis = _model.Overview;
            TagLine = _model.Tagline;
            Runtime = _model.Runtime;
        }

        private readonly IMovieService _service;

        public DetailedMovieViewModel(IMovieService service)
        {
            _service = service;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private double _score;
        public double Score
        {
            get { return _score; }
            set
            {
                _score = value;
                RaisePropertyChanged(() => Score);
            }
        }

        private string _posterUrl;
        public string PosterUrl
        {
            get { return _posterUrl; }
            set
            {
                _posterUrl = value;
                RaisePropertyChanged(() => PosterUrl);
            }
        }

        private string _releaseDate;
        public string ReleaseDate
        {
            get { return _releaseDate; }
            set
            {
                _releaseDate = value;
                RaisePropertyChanged(() => ReleaseDate);
            }
        }

        private int _voteCount;
        public int VoteCount
        {
            get { return _voteCount; }
            set
            {
                _voteCount = value;
                RaisePropertyChanged(() => VoteCount);
            }
        }

        private string _imdbUrl;
        public string ImdbUrl
        {
            get { return "http://www.imdb.com/title/" + _imdbUrl; }
            set
            {
                _imdbUrl = value;
                RaisePropertyChanged(() => ImdbUrl);
            }
        }

        private string _synopsis;
        public string Synopsis
        {
            get { return _synopsis; }
            set
            {
                _synopsis = value;
                RaisePropertyChanged(() => Synopsis);
            }
        }

        private string _tagline;
        public string TagLine
        {
            get { return _tagline; }
            set
            {
                _tagline = value;
                RaisePropertyChanged(() => TagLine);
            }
        }

        private int _runtime;
        public int Runtime
        {
            get { return _runtime; }
            set
            {
                _runtime = value;
                RaisePropertyChanged(() => Runtime);
            }
        }

        public ICommand ShowOnImdbCommand
        {
            get
            {
                return new MvxCommand(() => 
                    Mvx.Resolve<IMvxWebBrowserTask>().ShowWebPage(ImdbUrl));
            }
        }
    }
}
