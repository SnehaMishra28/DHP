using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCallRailData
{
    public class Call
    {
        public string customer_city { get; set; }
        public string customer_country { get; set; }
        public string customer_name { get; set; }
        public string customer_phone_number { get; set; }
        public string customer_state { get; set; }
        public int? duration { get; set; }
        public string id { get; set; }
        //public int companyId { get; set; }
        //public string callDate { get; set; }
        //public int orderNumber { get; set; }
        //public string orderAmount { get; set; }
        //public string matchDate { get; set; }
        //public int matchStatus { get; set; }
       // public string uploadDate { get; set; }
       // public string orderDate { get; set; }
        public string start_time { get; set; }
        public string source_name { get; set; }
        public string keywords { get; set; }
    }

    public class RootObject
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total_pages { get; set; }
        public int total_records { get; set; }
        public List<Call> calls { get; set; }
    }


}
