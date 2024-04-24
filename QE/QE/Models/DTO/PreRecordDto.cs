using System;

namespace QE.Models.DTO
{
    public class PreRecordDto
    {
        public long SDayWeekId { get; set; }
        public string DayName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTimePrerecord { get; set; }
        public TimeSpan StopTimePrerecord { get; set; }
    }
}
