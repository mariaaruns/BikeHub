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

        public string FullName { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
        public string Image { get; set; }


    }
}
