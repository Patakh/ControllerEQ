using System.Collections.Generic;

namespace QE.Models.DTO
{
    public class ButtonDto
    {
        public ButtonDto()
        {
            ChildButtons = new List<ButtonDto>();
            Destcriptions = new List<string>();
        }
        public long Id { get; set; }
        public long SortId { get; set; }
        public long? ParetId { get; set; }
        public string Name { get; set; }
        public long? Type { get; set; }
        public long? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public List<string> Destcriptions { get; set; }
        public List<ButtonDto> ChildButtons { get; set; }
    }
}
