using System;
using SQLite;
namespace SHHS.Model
{
    public class SHHSEvent
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Title { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string DaysLeft { get; set; }
        
    }
}
