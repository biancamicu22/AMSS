using DataLibrary.StatePattern;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    public class AppTask: IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public State State { get; set; }

        public virtual Sprint Sprint { get; set; }
        public string Sprint_Id { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public string User_Id { get; set; }
        public virtual User User { get; set; }

        public AppTask()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
