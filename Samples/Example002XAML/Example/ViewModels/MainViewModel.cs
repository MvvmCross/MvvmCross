using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Example.Models;
using Example.Services;

namespace Example.ViewModels
{
    public class MainViewModel 
        : MvxViewModel
    {
        private readonly IMovieService _service;

        public MainViewModel(IMovieService service)
        {
            _service = service;
        }

        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set 
            {
                _searchString = value;
                RaisePropertyChanged(() => SearchString);
            }
        }

        private List<Movie> _movies;

        public List<Movie> Movies
        {
            get { return _movies; }
            set
            {
                _movies = value;
                RaisePropertyChanged(() => Movies);
            }
        }

        private Movie _selectedMovie;

        public Movie SelectedMovie
        {
            get { return _selectedMovie; }
            set
            {
                _selectedMovie = value;
                RaisePropertyChanged(() => SelectedMovie);

                ShowSelectedMovieCommand.Execute(null);
            }
        }

        public ICommand GetMoviesCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    Movies = await _service.SearchMovie(SearchString);
                });
            }
        }

        public ICommand ShowSelectedMovieCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<DetailedMovieViewModel>(new {movieId = SelectedMovie.Id}),
                    () => SelectedMovie != null);
            }
        }

        public ICommand ShowAboutPageCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<AboutViewModel>());
            }
        }
    }
}
