using System.ClientModel;
using System.Text;
using System.Text.Json.Nodes;
using Azure.AI.OpenAI;
using Microsoft.Extensions.ObjectPool;
using OpenAI.Chat;
using VaderSharp2;

namespace Fall2024_Assignment3_scwood4.Models
{
    public class ReviewModel
    {
        public string[] Reviews { get; set; }

        private string ApiKey;
        private string ApiEndpoint;
        private const string AiDeployment = "gpt-35-turbo";
        private readonly ApiKeyCredential ApiCredential;

        private string _movieTitle { get; set; }
        private int _movieYear { get; set; }

        public ReviewModel(string _apiKey, string _apiEndpoint, string movieTitle, int movieYear)
        {
            ApiKey = _apiKey;
            ApiEndpoint = _apiEndpoint;
            ApiCredential = new ApiKeyCredential(ApiKey);
            _movieTitle = movieTitle;
            _movieYear = movieYear;
            Reviews = new string[10]; // Assuming you want to collect 10 reviews
        }

        public async Task GetMovieReviews()
        {
            Console.WriteLine("Requesting reviews...");

            var client = new AzureOpenAIClient(new Uri(ApiEndpoint), ApiCredential).GetChatClient(AiDeployment);

            string[] personas = { "is harsh", "loves romance", "loves comedy", "loves thrillers", "loves fantasy", "smokes weed everyday", "optimistic", "doesn't pay attention", "seeks glory" };
            var messages = new ChatMessage[]
            {
        new SystemChatMessage("You are a movie reviewer. Write ten short reviews for the given movie. Each review should be separated by '###'. Do not include any extra text before or after the reviews."),
        new UserChatMessage($"Provide distinct reviews of {_movieTitle}, released in {_movieYear}, with each review being around 100 words long.")
            };

            var result = await client.CompleteChatAsync(messages);

            // Check if the result has content
            if (result.Value.Content == null || !result.Value.Content.Any())
            {
                Console.WriteLine("No reviews were returned.");
                return;
            }

            string reviewsString = result.Value.Content.FirstOrDefault()?.Text ?? "";
            Console.WriteLine("Raw Reviews Response:\n" + reviewsString);

            // Attempt to split the reviews using the separator '###'
            string[] splitReviews = reviewsString.Split(new[] { "###" }, StringSplitOptions.RemoveEmptyEntries);

            // Trim each review and filter out any blank entries
            Reviews = splitReviews.Select(r => r.Trim()).Where(r => !string.IsNullOrEmpty(r)).ToArray();

            // Debug: Output the parsed reviews
            if (Reviews.Length > 0)
            {
                Console.WriteLine("Parsed Reviews:");
                for (int i = 0; i < Reviews.Length; i++)
                {
                    Console.WriteLine($"Review {i + 1}: {Reviews[i]}\n");
                }
            }
            else
            {
                Console.WriteLine("No valid reviews were parsed.");
            }
        }

        public double[] CalculateSentiment()
        {
            var analyzer = new SentimentIntensityAnalyzer();
            double[] sentimentScores = new double[Reviews.Length];

            for (int i = 0; i < Reviews.Length; i++)
            {
                string review = Reviews[i];
                if (!string.IsNullOrEmpty(review))
                {
                    var sentiment = analyzer.PolarityScores(review);
                    sentimentScores[i] = sentiment.Compound;
                }
            }

            return sentimentScores;

            
        }
    }
}
