using System;

namespace QE.Models.DTO
{
    public class SchedulesDto
    {
        public long Id { get; set; }
        public long SDayWeekId { get; set; }
        public string SDayWeekName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan StopTime { get; set; }
    }
}
