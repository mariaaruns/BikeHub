using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class UsersDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string FullName { get; set; }
        public string PhoneNumber{ get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Image { get; set; }
    }
    public class UserByIdDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string Image { get; set; }
    }


}
