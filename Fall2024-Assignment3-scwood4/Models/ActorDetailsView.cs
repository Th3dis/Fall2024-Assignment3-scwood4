﻿namespace Fall2024_Assignment3_scwood4.Models
{
    public class ActorDetailsView
    {
        public Actor Actor { get; set; }
        public string[] Tweets { get; set; }
        public double[] SentimentScores { get; set; }
        public double AvgSentiment { get; set; }
        public IEnumerable<Movie> MoviesActed { get; set; } = new List<Movie>();

        public ActorDetailsView(Actor actor, string[] tweets, double[] sentimentScores, double avgSentiment, IEnumerable<Movie> movies)
        {
            Actor = actor;
            Tweets = tweets;
            SentimentScores = sentimentScores;
            AvgSentiment = avgSentiment;
            MoviesActed = movies;
        }
    }
}