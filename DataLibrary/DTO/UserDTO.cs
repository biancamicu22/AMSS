using System;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using DataLibrary.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace DataLibrary.DTO
{
    [DataContract]
    public class UserDTO
    {
        [DataMember]
        [JsonProperty("id")]
        public string Id { get; set; }

        [DataMember]
        [JsonProperty("active")]
        public bool Active { get; set; }

        [DataMember]
        [JsonProperty("birthdate")]
        public DateTime? Birthdate { get; set; }

        [DataMember]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [DataMember]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [DataMember]
        [JsonProperty("roles")]
        public string Roles { get; set; }

        [DataMember]
        [JsonProperty("description")]
        public string Description { get; set; }

        [DataMember]
        [JsonProperty("password")]
        public string Password { get; set; }

        [DataMember]
        [JsonProperty("email")]
        public string Email { get; set; }

        [DataMember]
        [JsonProperty("username")]
        public string UserName { get; set; }

        [DataMember]
        [JsonProperty("comments")]
        public ICollection<CommentDTO> Comments { get; set; }

        [DataMember]
        [JsonProperty("tasks")]
        public ICollection<TaskDTO> Tasks { get; set; }

        public UserDTO()
        {
            Comments = new HashSet<CommentDTO>();
            Tasks = new HashSet<TaskDTO>();
        }

        public static UserDTO FromModel(User model)
        {
            if (model == null) return null;
            return new UserDTO()
            {
                Id = model.Id,
                UserName = model.UserName,
                Email = model.Email,
                Active = model.Active, 
                Birthdate = model.Birthdate, 
                FirstName = model.FirstName, 
                LastName = model.LastName, 
                Roles = model.Roles, 
                Description = model.Description, 
                Comments = model.Comments.Select(c => { c.User = model; return CommentDTO.FromModel(c); }).ToList(), 
                Tasks = model.Tasks.Select(t => { t.User = model; return TaskDTO.FromModel(t); }).ToList(), 
            }; 
        }

        public User ToModel()
        {
            return new User()
            {
                Id = Id,
                UserName = UserName,
                Email = Email,
                Active = Active, 
                Birthdate = Birthdate, 
                FirstName = FirstName, 
                LastName = LastName, 
                Roles = Roles, 
                Description = Description, 
                Comments = Comments.Select(c => c.ToModel()).ToList(), 
                Tasks = Tasks.Select(t => t.ToModel()).ToList(), 
            }; 
        }

        public static implicit operator ClaimsPrincipal(UserDTO v)
        {
            throw new NotImplementedException();
        }
    }
}