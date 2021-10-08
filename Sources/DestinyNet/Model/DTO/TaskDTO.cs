using System;
using System.Collections.Generic;
using System.Text;

namespace DestinyNet
{
    public class TaskDTO
    {
        public TaskDTO()
        {
            Tasks = new List<TaskDTO>();
        }
        public string Name { get; set; }
        public string GUID { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public string CalendarGUID { get; set; }
        public List<TaskDTO> Tasks { get; }
    }
}
