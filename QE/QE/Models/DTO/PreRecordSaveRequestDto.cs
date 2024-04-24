using System;

namespace QE.Models.DTO
{
    public class PreRecordSaveRequestDto
    {
        public string Fio { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DatePreRecord { get; set; }
        public TimeSpan StartTimePrerecord { get; set; }
        public TimeSpan StopTimePrerecord { get; set; }
    }
}
