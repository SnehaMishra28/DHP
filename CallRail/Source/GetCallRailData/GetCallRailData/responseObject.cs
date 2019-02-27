using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCallRailData
{
    class responseObject
    {
        public int recordId { get; set; }
        public int trackingId { get; set; }
        public int companyId { get; set; }
        public double callDuration { get; set; }
        public string customerName { get; set; }
        public string customerContact { get; set; }
        public string customerCountry { get; set; }
        public string customerState { get; set; }
        public string customerCity { get; set; }
        public double customerZip { get; set; }
        public string callDate { get; set; }
        public string callTime { get; set; }
        public string invoiceNumber { get; set; }
        public double invoiceAmount { get; set; }
        public string matchDate { get; set; }
        public int uploadStatus { get; set; }
        public string uploadDate { get; set; }

        //public responseObject()
        //{
        //    recordId = 0;
        //    trackingId = 0;
        //    companyId = 0;
        //    callDuration = 0;
        //    customerZip = 0;
        //    invoiceAmount = 0;
        //    matchDate = "";
        //    uploadStatus = -1;
        //    uploadDate = "";
        //    customerName = "";
        //    callDate = "";
        //    invoiceNumber = "";
        //    callTime = "";
        //    customerContact = "";
        //    customerCountry = "";
        //    customerState = "";
        //    customerCity = "";
        //}
    }
}
