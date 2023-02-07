using System;

namespace DataLibrary.Models
{
    public class Comment: IEntity
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime Date_Posted { get; set; }

        public string User_Id { get; set; }
        public virtual User User { get; set; }

        public string Task_Id { get; set; }
        public virtual AppTask Task { get; set; }
    }
}
