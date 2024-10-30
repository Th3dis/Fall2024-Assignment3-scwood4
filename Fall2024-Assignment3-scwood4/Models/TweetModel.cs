using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json.Nodes;
using VaderSharp2;

namespace Fall2024_Assignment3_scwood4.Models
{
	public class TweetModel
	{
		public string[] Tweets { get; set; }
		private string _actorName { get; set; }

		private string ApiKey;
		private string ApiEndpoint;
		private const string AiDeployment = "gpt-35-turbo";
		private readonly ApiKeyCredential ApiCredential;

		public TweetModel(string _apiKey, string _apiEndpoint, string name)
		{
            ApiKey = _apiKey;
            ApiEndpoint = _apiEndpoint;
            ApiCredential = new ApiKeyCredential(ApiKey);
            _actorName = name;
			Tweets = new string[20];
		}

        public async Task TwitterApiSim()
        {
            Console.WriteLine("Polling Twitter...");

            var client = new AzureOpenAIClient(new Uri(ApiEndpoint), ApiCredential).GetChatClient(AiDeployment);

            var messages = new ChatMessage[]
{
        new SystemChatMessage($"You represent the Twitter social media platform. Generate an answer that only contains a valid JSON array of objects with the format: [{{\"username\": \"example\", \"tweet\": \"example tweet\"}}, ...]. Do not include any text before or after the array."),
        new UserChatMessage($"Generate 20 tweets from a variety of users about the actor {_actorName}.")
};
            var result = await client.CompleteChatAsync(messages);

            string tweetsJsonString = result.Value.Content.FirstOrDefault()?.Text ?? "[]";
            Console.WriteLine(tweetsJsonString);

            // Attempt to find the JSON array within the response
            int startIndex = tweetsJsonString.IndexOf('[');
            if (startIndex > -1)
            {
                tweetsJsonString = tweetsJsonString.Substring(startIndex);
            }

            try
            {
                JsonArray json = JsonNode.Parse(tweetsJsonString)!.AsArray();
                var tweets = json.Select(t => t!["tweet"]?.ToString() ?? "").ToArray();

                // Store the tweets in the Tweets array
                for (int i = 0; i < tweets.Length && i < Tweets.Length; i++)
                {
                    Tweets[i] = tweets[i];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing tweets JSON: {ex.Message}");
            }
        }

        public double[] CalculateSentiment()
        {
            var analyzer = new SentimentIntensityAnalyzer();
            double[] sentimentScores = new double[Tweets.Length];

            for (int i = 0; i < Tweets.Length; i++)
            {
                string review = Tweets[i];
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
