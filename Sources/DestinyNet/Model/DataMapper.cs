using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using Destiny.Core;
using DestinyNet.ViewModels;

namespace DestinyNet
{
    public class DestinyNetMapper
    {
        public static IMapper GetMapper()
        {
            IMapper mapper = null;
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonDTO, PersonViewModel>().ReverseMap();
                cfg.CreateMap<PersonViewModel, PersonDTO>().ReverseMap();

                cfg.CreateMap<CalendarDTO, Calendar>().ReverseMap();
                cfg.CreateMap<Calendar, CalendarDTO>().ReverseMap();

                cfg.CreateMap<EventDTO, Event>().ConvertUsing(x => MappingEventDTOToEvent(x));
                cfg.CreateMap<Event, EventDTO>().ConvertUsing(x => MappingEventToEventDTO(x));

                cfg.CreateMap<TaskDTO, DTask>().ConvertUsing(x => MappingTaskDTOToDTask(x));
                cfg.CreateMap<DTask, TaskDTO>().ConvertUsing(x => MappingDTaskToTaskDTO(x));

                cfg.CreateMap<DataDTO, Data>().ConvertUsing((x, y, z) => MappingDataDTOToData(x, y, z));
                cfg.CreateMap<Data, DataDTO>().ConvertUsing((x, y, z) => MappingDataToDataDTO(x, y, z));
            }
            );
            mapperConfiguration.AssertConfigurationIsValid();
            mapper = mapperConfiguration.CreateMapper();
            return mapper;
        }

        private static TaskDTO MappingDTaskToTaskDTO(DTask x)
        {
            var t = new TaskDTO();
            t.Name = x.Name;
            t.GUID = x.GUID;
            t.Start = x.Start;
            t.Finish = x.Finish;
            t.CalendarGUID = (x.Calendar == null) ? "" : x.Calendar.GUID;
            return t;
        }
        private static DTask MappingTaskDTOToDTask(TaskDTO x)
        {
            var t = new DTask();
            t.Name = x.Name;
            t.GUID = x.GUID;
            t.Start = x.Start;
            t.Finish = x.Finish;
            t.Calendar = null;
            return t;
        }
        private static Event MappingEventDTOToEvent(EventDTO source)
        {
            var d = new Event();
            d.Caption = source.Caption;
            d.RuleType = source.Type;
            d.IsAllDay = source.IsAllDay;
            d.Calendar = null;

            d.Rule.Start = source.Start;
            d.Rule.Finish = source.Finish;
            d.Rule.Step = source.Step;
            d.Rule.FinishRepeatDate = source.FinishRepeatDate;
            if (source.Type == RuleRepeatTypes.Days)
            {
                (d.Rule as RuleRepeatDay).IsDayStep = source.IsDayStep;
                (d.Rule as RuleRepeatDay).IsWorkDayStep = source.IsWorkDayStep;
                (d.Rule as RuleRepeatDay).IsRepeatDay = source.IsRepeatDay;
                (d.Rule as RuleRepeatDay).IsMonday = source.IsMonday;
                (d.Rule as RuleRepeatDay).IsTuesday = source.IsTuesday;
                (d.Rule as RuleRepeatDay).IsWednesday = source.IsWednesday;
                (d.Rule as RuleRepeatDay).IsThursday = source.IsThursday;
                (d.Rule as RuleRepeatDay).IsFriday = source.IsFriday;
                (d.Rule as RuleRepeatDay).IsSaturday = source.IsSaturday;
                (d.Rule as RuleRepeatDay).IsSunday = source.IsSunday;
            }
            if (source.Type == RuleRepeatTypes.SpecialDays)
                (d.Rule as RuleRepeatSpecialDays).SpecialDays = new List<DateTime>(source.SpecialDays);

            return d;
        }
        private static EventDTO MappingEventToEventDTO(Event source)
        {
            var d = new EventDTO();
            d.Start = source.Rule.Start;
            d.Finish = source.Rule.Finish;
            d.Caption = source.Caption;
            d.IsAllDay = source.IsAllDay;
            d.CalendarGUID = "";


            d.Start = source.Rule.Start;
            d.Finish = source.Rule.Finish;
            d.Step = source.Rule.Step;
            d.Type = source.RuleType;
            d.FinishRepeatDate = source.Rule.FinishRepeatDate;
            if (source.RuleType == RuleRepeatTypes.Days)
            {
                d.IsDayStep = (source.Rule as RuleRepeatDay).IsDayStep;
                d.IsWorkDayStep = (source.Rule as RuleRepeatDay).IsWorkDayStep;
                d.IsRepeatDay = (source.Rule as RuleRepeatDay).IsRepeatDay;
                d.IsMonday = (source.Rule as RuleRepeatDay).IsMonday;
                d.IsTuesday = (source.Rule as RuleRepeatDay).IsTuesday;
                d.IsWednesday = (source.Rule as RuleRepeatDay).IsWednesday;
                d.IsThursday = (source.Rule as RuleRepeatDay).IsThursday;
                d.IsFriday = (source.Rule as RuleRepeatDay).IsFriday;
                d.IsSaturday = (source.Rule as RuleRepeatDay).IsSaturday;
                d.IsSunday = (source.Rule as RuleRepeatDay).IsSunday;
            }
            if (source.RuleType == RuleRepeatTypes.SpecialDays)
              d.SpecialDays = new List<DateTime>((source.Rule as RuleRepeatSpecialDays).SpecialDays);

            if (source.Calendar != null)
                d.CalendarGUID = source.Calendar.GUID;
            return d;
        }
        private static Data MappingDataDTOToData(DataDTO source, Data destination, ResolutionContext resolutionContext)
        {
            var d = new Data();
            var calendarsDictionary = new Dictionary<string, Calendar>();

            foreach (var calendarDTO in source.Calendars)
            {
                var c = resolutionContext.Mapper.Map<Calendar>(calendarDTO);
                calendarsDictionary.Add(calendarDTO.GUID, c);
                d.Calendars.Add(c);
            }


            DestinyNetMapper.ConvertToDTask(d.Tasks, source.Tasks, resolutionContext);


            foreach (var eventDTO in source.Events)
            {
                var e = resolutionContext.Mapper.Map<Event>(eventDTO);
                if (calendarsDictionary.ContainsKey(eventDTO.CalendarGUID))
                    e.Calendar = calendarsDictionary[eventDTO.CalendarGUID];
                else
                    continue;
                d.Events.Add(e);
            }

            foreach (var p in source.People)
                d.People.Add(resolutionContext.Mapper.Map<PersonViewModel>(p));

            return d;
        }
        private static DataDTO MappingDataToDataDTO(Data source, DataDTO destination, ResolutionContext resolutionContext)
        {
            var d = new DataDTO();

            foreach (var calendar in source.Calendars)
                d.Calendars.Add(resolutionContext.Mapper.Map<CalendarDTO>(calendar));

            foreach (var eventDTO in source.Events)
                d.Events.Add(resolutionContext.Mapper.Map<EventDTO>(eventDTO));

            DestinyNetMapper.ConvertToTaskDTO(source.Tasks, d.Tasks, resolutionContext);

            foreach (var p in source.People)
                d.People.Add(resolutionContext.Mapper.Map<PersonDTO>(p));

            return d;
        }
        public static void ConvertToTaskDTO(ObservableCollection<DTask> tasks, List<TaskDTO> list, ResolutionContext resolutionContext)
        {
            foreach (var t in tasks)
            {
                var ts = resolutionContext.Mapper.Map<TaskDTO>(t);
                list.Add(ts);
                DestinyNetMapper.ConvertToTaskDTO(t.SubTasks, ts.Tasks, resolutionContext);
            }
        }
        public static void ConvertToDTask(ObservableCollection<DTask> tasks, List<TaskDTO> list, ResolutionContext resolutionContext)
        {
            foreach (var t in list)
            {
                var ts = resolutionContext.Mapper.Map<DTask>(t);
                tasks.Add(ts);
                DestinyNetMapper.ConvertToDTask(ts.SubTasks, t.Tasks, resolutionContext);
            }
        }
    }
}

