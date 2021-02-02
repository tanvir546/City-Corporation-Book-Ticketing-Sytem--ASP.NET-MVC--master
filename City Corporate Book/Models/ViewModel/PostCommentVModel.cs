using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace City_Corporate_Book.Models.ViewModel
{
    public class PostCommentVModel
    {
        public int PostId { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public int StatusPost { get; set; }
        public DateTime Date { get; set; }
        public string Post { get; set; }
        public int Role { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
        public int Action { get; set; }
        public List<MemberUserLocationWiseComplainCommants> Comments { get; set; }
    }
}