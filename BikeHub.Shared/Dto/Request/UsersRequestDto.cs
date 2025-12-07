using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class UsersRequestDto
    {
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int PageSize { get; set; }
        
        public string? SearchName { get; set; }
        
        public string? SearchRole { get; set; }
    }
}
