namespace CommunityCenter.Models
{
    public class PostPostDto
    {
        public string Content { get; set; }
        public int User_Id { get; set; }
        public int PrivacyType_Id { get; set; }
    }
    public class PutPostDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int User_Id { get; set; }
        public int PrivacyType_Id { get; set; }
    }
    public class DeletePostDto
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
    }
}