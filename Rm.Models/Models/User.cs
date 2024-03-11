
using System.Text.Json.Serialization;


namespace Rm.Model.Models
{
    
    public partial class User : BaseModel
    {
       
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public int Age { get; set; }

        public string Email { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public string? ImagePath { get; set; }
        [JsonIgnore]
        public byte[]? ImageByte {  get; set; }

        private short roleId;

        public short RoleId
        {
            get { return roleId; }
            set
            {
                if (value == 1 || value == 0)
                {
                    roleId = value;
                }
            }
        }

        [JsonIgnore]
        public virtual Role? Role { get; set; }
    }
   
    public partial class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public int Age { get; set; }    
        public short RoleId { get; set; }
        public string? ImagePath { get; set; }
        //public UserResponse(User user)
        //{
        //    this.Id = user.Id;
        //    this.Name = user.Name;
        //    this.Email = user.Email;
        //    this.Surname = user.Surname;
        //}


    }
}
