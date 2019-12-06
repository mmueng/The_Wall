using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace The_Wall.Models
{
    public class Comments
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // Will not be mapped to your users table!

        public int UserId { get; set; }
        public User CommentCreator { get; set; }


        public int MessageId { get; set; }
        public Messages CommentToMesg { get; set; }

        // public List<Association> Assoc_User { get; set; }
    }
}