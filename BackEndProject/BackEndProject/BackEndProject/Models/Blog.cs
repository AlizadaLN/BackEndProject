namespace BackEndProject.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Desc { get; set; }

        public DateTime CreteDate { get; set; }
    }
}
