namespace Assignment_04.Model
{
    public class Video
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public bool? IsRented { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
