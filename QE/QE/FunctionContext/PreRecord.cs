using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QE.FunctionContext
{
    public class PreRecord
    {
        [Column("out_s_day_week_id")]
        public long SDayWeekId { get; set; }
        [Column("out_day_name")]
        public string DayName { get; set; }
        [Column("out_date")]
        public DateTime Date { get; set; }
        [Column("out_start_time_prerecord")]
        public TimeSpan StartTimePrerecord { get; set; }
        [Column("out_stop_time_prerecord")]
        public TimeSpan StopTimePrerecord { get; set; }
    }
}
