namespace Fall2024_Assignment3_scwood4.Models
{
    public class ActorMovie
    {
        public int Id { get; set; }

        public int actorId { get; set; }
        public Actor actor { get; set; }

        public int movieId { get; set; }
        public Movie movie { get; set; }

    }


}
