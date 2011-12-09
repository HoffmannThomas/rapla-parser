using System;
using ConfigurationManager;
using RaplaParser;
using RaplaObjects;
using ExchangeConnector;
using RaplaObjects.Reservations;
using Microsoft.Exchange.WebServices.Data;
using RaplaObjects.Repeatings;
using RaplaObjects.Rooms;
using System.Collections.Generic;
using RaplaObjects.Persons;

namespace RaplaConnector
{
    class Program
    {
        private static Parser parser = new Parser(ConfigManager.getConfigString("rapla_data_path"));

        static void Main(string[] args)
        {
            string selection = "";

            parser.printReservations();

            Console.WriteLine("Please enter your username:");
            String user = Console.ReadLine();

            Console.WriteLine("Please enter your password:");
            String password = PasswordReader.ReadPassword();

            ExchangeConnector.EWSConnector ewsConnector = null;

            try
            {
                ewsConnector = new ExchangeConnector.EWSConnector(ConfigManager.getConfigString("fqdn"), user, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            saveReservations(ewsConnector);

            while (selection.ToLower() != "q")
            {
                Console.WriteLine();
                Console.WriteLine("Enter 'Q' to exit.");
                Console.Write("> ");
                selection = Console.ReadKey().KeyChar.ToString();
            }
        }

        private static FolderId getRaplaFolder(ExchangeService service)
        {
            string raplaCalendar = "Rapla";
            FolderView view = new FolderView(50);
            SearchFilter filter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, raplaCalendar);

            Folder parent = Folder.Bind(service, WellKnownFolderName.Calendar);
            FindFoldersResults result = parent.FindFolders(filter, view);

            Console.WriteLine("Looking for Rapla-Calendar...");

            if (result.Folders.Count == 0)
            {
                Console.WriteLine("Not found, creating new...");
                CalendarFolder folder = new CalendarFolder(service);
                folder.DisplayName = "Rapla";
                folder.Save(WellKnownFolderName.Calendar);
                Console.WriteLine("Rapla-Calendar created.");
                return folder.Id;
            }
            else
            {
                Console.WriteLine("Found.");
                return result.Folders[0].Id;
            }
        }

        private static Recurrence createRecurrence(Reservation reservation)
        {
            Recurrence recurrence = null;
            Recurrence pattern = null;
            DayOfTheWeek[] daysOfWeek = new DayOfTheWeek[7];
            daysOfWeek[0] = DayOfTheWeek.Monday;

            if (reservation.Repeating != null)
            {
                if (reservation.Repeating.GetType().Equals(typeof(RepeatDaily)))
                {
                    RepeatDaily daily = (RepeatDaily)reservation.Repeating;
                    pattern = new Recurrence.DailyPattern(reservation.DateStart, daily.interval);
                }
                if (reservation.Repeating.GetType().Equals(typeof(RepeatWeekly)))
                {
                    RepeatWeekly weekly = (RepeatWeekly)reservation.Repeating;
                    pattern = new Recurrence.WeeklyPattern(reservation.DateStart, weekly.interval, daysOfWeek);
                }
                if (reservation.Repeating.GetType().Equals(typeof(RepeatDaily)))
                {
                    RepeatMonthly monthly = (RepeatMonthly)reservation.Repeating;
                    pattern = new Recurrence.MonthlyPattern();
                }
                if (reservation.Repeating.GetType().Equals(typeof(RepeatDaily)))
                {
                    RepeatYearly yearly = (RepeatYearly)reservation.Repeating;
                    pattern = new Recurrence.YearlyPattern();
                }
                recurrence = AccountObjectFactory.createRecurrence(
                                reservation.DateStart,
                                reservation.Repeating.until,
                                reservation.Repeating.count,
                                pattern,
                                reservation.Repeating.IsForever
                                );
            }
            return recurrence;
        }

        private static void saveReservations(ExchangeConnector.EWSConnector ewsConnector)
        {
            FolderId raplaId = getRaplaFolder(ewsConnector.getEWSService());

            foreach (Reservation reservation in parser.getReservations().Values)
            {
                Course course = (Course)reservation;

                List<EmailAddress> roomAddresses = new List<EmailAddress>();
                List<EmailAddress> attendantAddresses = new List<EmailAddress>();
                String locationString = "";

                foreach (Room room in reservation.Rooms)
                {
                    if (room.GetType() == typeof(CourseRoom))
                    {
                        roomAddresses.Add(new EmailAddress(((CourseRoom)room).number + "@" + ConfigManager.getConfigString("fqdn").ToString()));
                        locationString += "Room " + ((CourseRoom)room).number + ";";
                    }
                }

                foreach (Person person in reservation.Attendants)
                {
                    if (person.GetType() == typeof(StudentsClass))
                    {
                        attendantAddresses.Add(new EmailAddress(((StudentsClass)person).course_name + "@" + ConfigManager.getConfigString("fqdn").ToString()));
                    }
                }

                AccountObjectFactory.CreateCalendarObject(
                    ewsConnector,
                    raplaId,
                    course.getID(),
                    course.name,
                    course.name,
                    course.DateStart,
                    course.DateEnd,
                    locationString,
                    createRecurrence(reservation),
                    roomAddresses,
                    attendantAddresses
                );
            }
        }
    }
}
