using System;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using DataLibrary.Models;

namespace DataLibrary.DTO
{
    [DataContract]
    public class CommentDTO
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }

        [DataMember]
        [JsonProperty("description")]
        public string Description { get; set; }

        [DataMember]
        [JsonProperty("date_Posted")]
        public DateTime Date_Posted { get; set; }

        [DataMember]
        [JsonProperty("user_Id")]
        public string User_Id { get; set; }


        [DataMember]
        [JsonProperty("task_Id")]
        public string Task_Id { get; set; }

        [DataMember]
        [JsonProperty("username")]
        public string Username { get; set; }


        public CommentDTO()
        {
        }

        public static CommentDTO FromModel(Comment model)
        {
            if (model == null) return null;
            if (model.User == null) return null;
            return new CommentDTO()
            {
                Id = model.Id,
                Description = model.Description,
                Date_Posted = model.Date_Posted,
                User_Id = model.User_Id,
                Task_Id = model.Task_Id,
                Username = model.User.UserName
            }; 
        }

        public Comment ToModel()
        {
            return new Comment()
            {
                Id = Id, 
                Description = Description, 
                Date_Posted = Date_Posted, 
                User_Id = User_Id, 
                Task_Id = Task_Id, 
            }; 
        }
    }
}