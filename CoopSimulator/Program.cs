using CoopSimulator.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoopSimulator
{
    class Program
    {
        public static PoultryHandler PoultryHandler = new PoultryHandler();
        public static List<Data.PoultryDto> PoultryList = new List<PoultryDto>();
        public static Data.ConfigurationDto.Configuration Configuration { get; set; }
        public static Data.DateDto Date { get; set; }
        static void Main(string[] args)
        {
            Configuration = Util.Configuration();
            Date = new DateDto(Configuration.GlobalSetting.Reporting.DatePeriod, Configuration.GlobalSetting.Reporting.Value, DatePeriodTriggered) 
            {
                Date = new DateTime(2000, 1, 1)
            };

            Console.WriteLine("WELCOME TO COOP SIMULATOR OF " + Configuration.PoultryDetail.Name);

            Util.Init();

            Console.ReadLine();
        }

        public static void DatePeriodTriggered(object sender, EventArgs args)
        {
            Console.WriteLine(Program.Date.Date.ToString("dd-MM-yyyy") + 
                " | F: " + Program.PoultryList.Count(a => a.Sex == Enums.PoultrySex.Female) + 
                " | M: " + Program.PoultryList.Count(a => a.Sex == Enums.PoultrySex.Male) + 
                " | AVG: " + (int)Program.PoultryList.Average(a => (Program.Date.Date - a.BirthDate).TotalDays));
        }
    }
}