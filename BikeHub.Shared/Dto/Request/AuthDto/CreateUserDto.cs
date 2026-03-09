using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request.AuthDto
{
    public  class CreateUserDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }


        public string Password { get; set; }

        public long RoleId { get; set; }
        public string PhoneNumber { get; set; }

        public byte[]? Imagebyte { get; set; }
        public string? Image { get; set; }

    }


    public class UpdateUserDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public byte[]? Imagebyte { get; set; }
        public string? Image { get; set; }

    }

}
