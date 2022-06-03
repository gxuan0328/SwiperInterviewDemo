using System.ComponentModel.DataAnnotations;

namespace CommunityCenter.Models
{
    public class PostCommentDto
    {
        [Required]
        public int Parent_Comment_Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int User_Id { get; set; }
    }
    
    public class PutCommentDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int User_Id { get; set; }
    }

    public class DeleteCommentDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int User_Id { get; set; }
    }
}