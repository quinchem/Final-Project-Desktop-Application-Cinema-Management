using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.Models
{
    public class Staff
    {
        public string staff_id { get; set; }        // staff_id
        public string full_name { get; set; }       // full_name
        public string date_of_birth { get; set; }    // date_of_birth (TEXT)
        public string gender { get; set; }         // gender
        public string email { get; set; }          // email
        public string phone_number { get; set; }    // phone_number
        public string role { get; set; }            // role
    }
}

