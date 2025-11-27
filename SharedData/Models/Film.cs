using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class Film
    {
        public string movie_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string genre { get; set; }
        public string director { get; set; }
        public string actor { get; set; }
        public string release_date { get; set; }
        public string language { get; set; }
        public string age_restriction { get; set; }
        public int duration { get; set; }
        public int? film_purchase_price { get; set; }
        public string status { get; set; }
    }
}
