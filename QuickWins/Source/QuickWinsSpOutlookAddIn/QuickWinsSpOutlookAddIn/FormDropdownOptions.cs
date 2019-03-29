using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickWinsSpOutlookAddIn
{
    public class FormDropdownOptions
    {
        // List of String to hold the values of System name, Problems and Resolution
        //List<string> systemList = new List<string>();
        //List<string> problemList = new List<string>();
        //List<string> resolutionList = new List<string>();

        public static List<string> systemList { get; set; }
        public static List<string> problemList { get; set; }
        public static List<string> resolutionList { get; set; }

        public static List<SpRecords> records { get; set; }
    }
}
