using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedParentalCli
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine(new DateTime(2021,1,24).AddDays(18));

            var option1 = new List<Period> {
                new Period(new DateTime(2020,12,24), "Christmas annual leave", new DatesCount()),
                new Period(new DateTime(2021,1,1), "Working from South Africa", new DatesCount()),
                new Period(new DateTime(2021,1,29), "Shared parental leave", new DatesCount()),
                new Period(new DateTime(2021,3,19), "Working from UK", new DatesDontCount())
            };

            PrintDates("Option 1: All the time in one block", option1);

            var option2 = new List<Period> {
                new Period(new DateTime(2020,12,24), "Christmas annual leave", new DatesCount()),
                new Period(new DateTime(2021,1,1), "Working from South Africa", new DatesCount()),
                new Period(new DateTime(2021,2,5), "Working from UK", new DatesDontCount()),
                new Period(new DateTime(2021,2,23), "Shared parental leave", new DatesCount()),
                new Period(new DateTime(2021,4,6), "Working from UK", new DatesDontCount())
            };

            PrintDates("Option 2: Come back before shared parental leave", option2);

            var option3 = new List<Period> {
                new Period(new DateTime(2020,12,24), "Christmas annual leave", new DatesCount()),
                new Period(new DateTime(2021,1,1), "Working from South Africa", new DatesCount()),
                new Period(new DateTime(2021,2,1), "Shared parental leave", new DatesCount()),
                new Period(new DateTime(2021,3,15), "Working from UK", new DatesDontCount()),
                new Period(new DateTime(2021,4,2), "Working from South Africa", new DatesCount()),
                new Period(new DateTime(2021,4,6), "Working from UK", new DatesDontCount())
            };

            PrintDates("Option 3: Come back after shared parental leave", option3);

            var option4 = new List<Period> {
                new Period(new DateTime(2020,12,24), "Christmas annual leave", new DatesCount()),
                new Period(new DateTime(2021,1,1), "Working from South Africa", new DatesCount()),
                new Period(new DateTime(2021,1,24), "Working from UK", new DatesDontCount()),
                new Period(new DateTime(2021,1,30), "Snowboarding", new DatesDontCount()),
                new Period(new DateTime(2021,2,7), "Working from UK", new DatesDontCount()),
                new Period(new DateTime(2021,2,13), "Shared parental leave", new DatesCount()),
                new Period(new DateTime(2021,3,27), "Working from South Africa", new DatesCount()),
                new Period(new DateTime(2021,4,6), "Working from UK", new DatesDontCount())
            };

            PrintDates("Option 4: Douglas goes snowboarding", option4);
        }

        private static void PrintDates(string periodsName, IEnumerable<Period> periods)
        {
            Console.WriteLine(periodsName);

            Period lastPeriod = periods.First();
            var totalDays = 0;
            foreach(var period in periods.Skip(1))
            {
                Console.WriteLine(period.Visit((period) => {
                    var periodDays = lastPeriod.DaysDifference(period);
                    totalDays += lastPeriod.CountedDaysDifference(period);
                    return $"{periodDays} days, {totalDays} total days.  {lastPeriod.Description} - {period.Description}";
                }));

                lastPeriod = period;
            }
        }
    }

    public class DatesCount : DatesRelevance
    {
        public int RelevantDays(int days)
        {
            return days;
        }
    }

    public class DatesDontCount : DatesRelevance
    {
        public int RelevantDays(int Days)
        {
            return 0;
        }
    }

    public interface DatesRelevance
    {
        int RelevantDays(int days);
    }

    public class Period
    {
        private DateTime _startDate;
        private string _description;
        private DatesRelevance _datesRelevance;
        public Period(DateTime startDate, string description, DatesRelevance datesRelevance)
        {
            _startDate = startDate;
            _description = description;
            _datesRelevance = datesRelevance;
        }

        public int DaysDifference(Period otherPeriod)
        {
            return (otherPeriod._startDate - _startDate).Days;
        }

        public int CountedDaysDifference(Period otherPeriod)
        {
            return _datesRelevance.RelevantDays(DaysDifference(otherPeriod));
        }

        public string Visit(Func<Period, string> visitor)
        {
            return visitor.Invoke(this);
        }

    public string Description
        {
            get { return _description; }
        }
    }
}
