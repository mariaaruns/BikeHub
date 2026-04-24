using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Common
{
    public class OutBoxMessage
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
    }

    public class OutBoxMessagePayload { 
    
        public string Email { get; set; }

        public string CustomerName { get; set; }    

        public string PhoneNumber { get; set; }

        public string Subject { get; set; }
        public string TemplateContent { get; set; }

    }

}
