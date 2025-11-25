using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.Models
{
    public class Product
    {
        public string product_id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int purchase_price { get; set; }
        public int price { get; set; }
        public string product_type_id { get; set; }
    }
}
