using System.Collections.Generic;

namespace DestinyNet
{
    public class DataDTO
    {
        public List<DateRangeDTO> DateRanges { get; set; }
        public List<CalendarDTO> Calendars { get; set; }
        public CalendarDTO CalendarsDefault { get; set; }
        public List<EventDTO> Events { get; set; }

        public DataDTO()
        {
            DateRanges = new List<DateRangeDTO>();
            Calendars = new List<CalendarDTO>();
            Events = new List<EventDTO>();
        }
    }
}
