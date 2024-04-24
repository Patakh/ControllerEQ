using System;

namespace QE.Models.DTO
{
    public class PreRecordSaveResponseDto
    {
        public long Number { get; set; }
        public DateTime DatePreRecord { get; set; }
        public TimeSpan StartTimePrerecord { get; set; }
        public TimeSpan StopTimePrerecord { get; set; }
    }
}
