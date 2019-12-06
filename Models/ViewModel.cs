using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace The_Wall.Models
{
    public class ViewModel
    {
        // User Model
        public User NewUser { get; set; }
        public List<User> AllUsers { get; set; }

        public LoginUser LoginUser { get; set; }
        public List<LoginUser> AllLoginUser { get; set; }

        // Message Model
        public Messages NewMsg { get; set; }
        public List<Messages> AllMsg { get; set; }

        // Comment Model
        public Comments NewComment { get; set; }
        public List<Comments> AllComments { get; set; }

        // public Wedding NewWedding { get; set; }
        // public List<Wedding> AllWeddings { get; set; }

        // public Association newAssoc { get; set; }
        // public List<Association> AllAssociations { get; set; }
    }
}