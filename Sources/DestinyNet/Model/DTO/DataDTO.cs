using System.Collections.Generic;

namespace DestinyNet
{
    public class DataDTO
    {
        public List<CalendarDTO> Calendars { get; set; }
        public CalendarDTO CalendarsDefault { get; set; }
        public List<EventDTO> Events { get; set; }
        public List<TaskDTO> Tasks { get; set; }
        public List<PersonDTO> People { get; set; }

        public DataDTO()
        {
            Calendars = new List<CalendarDTO>();
            Events = new List<EventDTO>();
            Tasks = new List<TaskDTO>();
            People = new List<PersonDTO>();
        }
    }
}
