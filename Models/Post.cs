using System.ComponentModel.DataAnnotations;

namespace CommunityCenter.Models
{
    public class PostPostDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int User_Id { get; set; }
        [Required]
        public int PrivacyType_Id { get; set; }
    }
    public class PutPostDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int User_Id { get; set; }
        [Required]
        public int PrivacyType_Id { get; set; }
    }
    public class DeletePostDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int User_Id { get; set; }
    }
}