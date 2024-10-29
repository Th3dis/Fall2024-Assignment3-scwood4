using Fall2024_Assignment3_scwood4.Data.Migrations;

namespace Fall2024_Assignment3_scwood4.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string IMDBLink { get; set; }
        public string ImageUrl { get; set; }

    }
}
