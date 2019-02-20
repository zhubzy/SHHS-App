using System;
using System.Collections.Generic;

namespace SHHS.Model
{
    public class SHHSSchedule
    {


        public List<SHHSPeriod> Schedule { get; set; }
        public string ScheduleName { get; set; }

        public SHHSSchedule()
        {
            Schedule = new List<SHHSPeriod>();

        }


    }

    public class SHHSPeriod
    {

        public string PeriodName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Length { get; set; }

        public string DisplayTime
        {

            get
            {
                return string.Format("{0} - {1}", StartTime, EndTime);
            }
        }


    }


}
