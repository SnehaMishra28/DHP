using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateCallRailData
{
    class RecordOrderDetails
    {
        public string recid { get; set; }
        public string orno { get; set; }
        public string oramnt { get; set; }

        public override string ToString()
        {
            return "Record ID: " + recid + ", Order Number: " + orno + ", Order Amount: " + oramnt;
        }

        public bool Equals(string passRecid)
        {
            if (passRecid == null) return false;
            return (this.recid.Equals(passRecid));
        }

    }

}
