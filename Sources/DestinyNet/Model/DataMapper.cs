using System;
using System.Collections.Generic;
using AutoMapper;

namespace DestinyNet
{
    public class DestinyNetMapper
    {
        public static IMapper GetMapper()
        {
            IMapper mapper = null;
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CalendarDTO, Calendar>().ReverseMap();
                cfg.CreateMap<Calendar, CalendarDTO>().ReverseMap();

                cfg.CreateMap<EventDTO, Event>().ConvertUsing(x => MappingEventDTO(x));
                cfg.CreateMap<Event, EventDTO>().ConvertUsing(x => MappingEvent(x));

                cfg.CreateMap<DataDTO, Data>().ConvertUsing((x, y, z) => MappingDataDTO(x, y, z));
                cfg.CreateMap<Data, DataDTO>().ConvertUsing((x, y, z) => MappingData(x, y, z));
            }
            );
            mapperConfiguration.AssertConfigurationIsValid();
            mapper = mapperConfiguration.CreateMapper();
            return mapper;
        }
        private static Event MappingEventDTO(EventDTO source)
        {
            var d = new Event();
            d.Start = source.Start;
            d.Finish = source.Finish;
            d.Caption = source.Caption;
            d.Calendar = null;
            return d;
        }
        private static EventDTO MappingEvent(Event source)
        {
            var d = new EventDTO();
            d.Start = source.Start;
            d.Finish = source.Finish;
            d.Caption = source.Caption;
            d.CalendarGUID = "";
            if (source.Calendar != null)
                d.CalendarGUID = source.Calendar.GUID;
            return d;
        }
        private static Data MappingDataDTO(DataDTO source, Data destination, ResolutionContext resolutionContext)
        {
            var d = new Data();
            var calendarsDictionary = new Dictionary<string, Calendar>();
            foreach (var calendarDTO in source.Calendars)
            {
                var c = resolutionContext.Mapper.Map<Calendar>(calendarDTO);
                calendarsDictionary.Add(calendarDTO.GUID, c);
                d.Calendars.Add(c);
            }
            foreach (var eventDTO in source.Events)
            {
                var e = resolutionContext.Mapper.Map<Event>(eventDTO);
                if ((e.Calendar != null)&&(calendarsDictionary.ContainsKey(e.Calendar.GUID)))
                    e.Calendar = calendarsDictionary[e.Calendar.GUID];
                else
                    continue;
                d.Events.Add(e);
            }
            return d;
        }
        private static DataDTO MappingData(Data source, DataDTO destination, ResolutionContext resolutionContext)
        {
            var d = new DataDTO();
            foreach (var calendar in source.Calendars)
            {
                d.Calendars.Add(resolutionContext.Mapper.Map<CalendarDTO>(calendar));
            }
            foreach (var eventDTO in source.Events)
            {
                d.Events.Add(resolutionContext.Mapper.Map<EventDTO>(eventDTO));
            }
            return d;
        }
    }
}

