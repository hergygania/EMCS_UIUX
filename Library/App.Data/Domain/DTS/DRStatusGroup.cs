using System;
using System.Collections.Generic;

namespace App.Data.Domain
{

    public class DRStatusGroup
    {
        public string Status { get; set; }
        public int Count_Id { get; set; }
    }
    public class DRChart
    {
        public int month { get; set; }
        public string status { get; set; }
    }
    public struct DateStruct
    {
        public int Count { get; set; }
        public string MonthName { get; set; }
    }

   
    public struct ListDateStruct
    {
        public string status { get; set; }
        public List<DateStruct> ListDate { get; set; }
    }    
}