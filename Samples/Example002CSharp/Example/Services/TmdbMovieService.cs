using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Example.Models;
using ModernHttpClient;
using Newtonsoft.Json;

namespace Example.Services
{
    public class TmdbMovieService : IMovieService
    {
        private Configuration _tmdbConfiguration;
        private HttpClient _baseClient;

        private HttpClient BaseClient
        {
            get
            {
                return _baseClient ?? (_baseClient = new HttpClient(new NativeMessageHandler())
                {
                    BaseAddress = new Uri(Constants.TmdbBaseUrl)
                });
            }
        }

        private async Task GetConfigurationIfNeeded(bool force = false)
        {
            if (_tmdbConfiguration != null && !force) return;

            try
            {
                var res = await BaseClient.GetAsync(string.Format(Constants.TmdbConfigurationUrl, Constants.TmdbApiKey));
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) throw new Exception("Return content was empty :(");

                _tmdbConfiguration = JsonConvert.DeserializeObject<Configuration>(json);
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof (TmdbMovieService).Name,
                    "Ooops! Something went wrong fetching the configuration. Exception: {1}", ex);
            }
        }

        public async Task<List<Movie>> SearchMovie(string movieTitle)
        {
            try
            {
                var res = await BaseClient.GetAsync(string.Format(Constants.TmdbMovieSearchUrl, Constants.TmdbApiKey,
                    movieTitle));
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) return null;

                var movies = JsonConvert.DeserializeObject<Movies>(json);
                await GetConfigurationIfNeeded();

                var movieList = movies.results.Select(movie => new Movie
                {
                    Id = movie.id, 
                    Title = movie.title, 
                    PosterUrl = _tmdbConfiguration.images.base_url + 
                                _tmdbConfiguration.images.poster_sizes[3] + 
                                movie.poster_path,
                    Score = movie.vote_average
                }).ToList();

                return movieList;
            }
            catch(Exception ex)
            {
                Mvx.TaggedTrace(typeof(TmdbMovieService).Name, 
                    "Ooops! Something went wrong fetching information for: {0}. Exception: {1}", movieTitle, ex);
                return null;
            }
        }

        public async Task<DetailedMovie> DetailedMovieFromId(int id)
        {
            try
            {
                var res = await BaseClient.GetAsync(string.Format(Constants.TmdbMovieUrl, Constants.TmdbApiKey,
                    id));
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(json)) return null;

                var movie = JsonConvert.DeserializeObject<TmdbMovie>(json);
                await GetConfigurationIfNeeded();

                var detailed = new DetailedMovie
                {
                    Genres = movie.genres,
                    OriginalTitle = movie.original_title,
                    Overview = movie.overview,
                    Score = movie.vote_average,
                    VoteCount = movie.vote_count,
                    ImdbId = movie.imdb_id,
                    PosterUrl = _tmdbConfiguration.images.base_url +
                                _tmdbConfiguration.images.poster_sizes[3] +
                                movie.poster_path,
                    ReleaseDate = movie.release_date,
                    Runtime = movie.runtime,
                    Tagline = movie.tagline
                };

                return detailed;
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(TmdbMovieService).Name,
                    "Ooops! Something went wrong fetching information id: {0}. Exception: {1}", id, ex);
                return null;
            }
        }
    }
}
