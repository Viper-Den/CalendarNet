using System.Collections.Generic;

namespace DestinyNet
{

    public class Data
    {
        public List<DateRange> DateRanges { get; set; }
        public List<Calendar> Calendars { get; set; }
        public List<Event> Events { get; set; }

        public Data()
        {
            DateRanges = new List<DateRange>();
            Calendars = new List<Calendar>();
            Events = new List<Event>();
            //DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-06-08"), Finish = DateTime.Parse("2021-06-13") });
            //DateRanges.Add(new DateRange() { Start = DateTime.Parse("2021-06-01"), Finish = DateTime.Parse("2021-06-01") });

            //Calendars.Add(new Calendar() { Name = "Work", Enabled = true, Color = Brushes.Blue });
            //Calendars.Add(new Calendar() { Name = "Home", Enabled = true, Color = Brushes.Green });

            //Events.Add(new Event() { Caption = "Work", Date = DateTime.Parse("2021-06-08"), Color = Brushes.Blue });
            //Events.Add(new Event() { Caption = "Work 2", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Green });
            //Events.Add(new Event() { Caption = "222", Date = DateTime.Parse("2021-06-11"), Color = Brushes.GreenYellow });
            //Events.Add(new Event() { Caption = "33333", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Red });
            //Events.Add(new Event() { Caption = "4444", Date = DateTime.Parse("2021-06-11"), Color = Brushes.Bisque });
        }
    }
}
