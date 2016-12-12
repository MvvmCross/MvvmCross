using System.Collections.Generic;

namespace Example.Models
{
    public class DetailedMovie
    {
        public string OriginalTitle { get; set; }
        public string ReleaseDate { get; set; }
        public double Score { get; set; }
        public int VoteCount { get; set; }
        public string ImdbId { get; set; }
        public List<Genre> Genres { get; set; }
        public string Overview { get; set; }
        public int Runtime { get; set; }
        public string Tagline { get; set; }
        public string PosterUrl { get; set; }
    }
}
