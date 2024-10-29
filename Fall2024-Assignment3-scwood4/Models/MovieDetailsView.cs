namespace Fall2024_Assignment3_scwood4.Models
{
    public class MovieDetailsView
    {
        public Movie Movie { get; set; }
        public string[] Reviews { get; set; }
        public double[] SentimentScores { get; set; }
        public double AvgSentiment { get; set; }
        public List<Actor> Actors { get; set; } = new List<Actor>();

        public MovieDetailsView(Movie movie, string[] reviews, double[] sentimentScores, double avgSentiment)
        {
            Movie = movie;
            Reviews = reviews;
            SentimentScores = sentimentScores;
            AvgSentiment = avgSentiment;
        }
    }
}
