using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoDomain.Entity
{
    public record Comment
    {
        public long Id { get; private set; }
        public Guid UserId { get; private set;}
        public User Sayer { get; private set; }
        public TheFile TheFile { get; private set; }
        public string FileHash { get; private set; }
        public bool IsDeleted { get; private set; } 
        public DateTime DateCreated { get; private set; }
        public string Content { get; private set; }
        private Comment() { }
        public static Comment InitComment(User sayer,TheFile theFile,string content) {
            Comment comment = new Comment();
            comment.Sayer = sayer;  
            comment.TheFile = theFile;
            comment.Content = content;
            comment.IsDeleted = false;
            return comment;
        }
        public void Delete()
        {
            this.IsDeleted = true;
        }
    }
}
