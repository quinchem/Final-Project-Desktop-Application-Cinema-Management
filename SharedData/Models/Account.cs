using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
        public class Account
        {
            public string account_id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string role_account { get; set; }
            public string? staff_id { get; set; }
            public string? customer_id { get; set; }
        }
    
}
