using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class Customer
    {
        public string customer_id { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string create_date { get; set; }
    }
}
