namespace CommunityCenter.Models
{
    public class PostCommentDto
    {
        public int Parent_Comment_Id { get; set; }
        public string Content { get; set; }
        public int User_Id { get; set; }
    }
    
    public class PutCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int User_Id { get; set; }
    }

    public class DeleteCommentDto
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
    }
}