namespace SecretSanta.Domain.Models
{
    //hold the title, order of importance, url, description, and User
    public class Gift
    {
        public string Title { get; set; }
        public int OrderOfImportance { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public User User { get; set; }  //Potentially multiple users could list the same item, this could be a list to reduce duplication
    }
}