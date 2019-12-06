using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace The_Wall.Models
{
    public class Messages
    {

        [Key]
        public int MessageId { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // Will not be mapped to your users table!
        public int UserId { get; set; }
        public User MsgCreator { get; set; }
        public List<Comments> MsgComments { get; set; }
        // public List<Association> Assoc_User { get; set; }
    }
}