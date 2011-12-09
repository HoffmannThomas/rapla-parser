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


        private static void saveReservations(ExchangeConnector.EWSConnector ewsConnector)
        {
            foreach (Reservation reservation in parser.getReservations().Values)
            {
                Course course = (Course)reservation;

                Recurrence recurrence = null;
                Recurrence pattern = null;
                List<EmailAddress> roomAddresses = new List<EmailAddress>();
                List<EmailAddress> attendantAddresses = new List<EmailAddress>();
                String locationString = "";
                DayOfTheWeek[] daysOfWeek = new DayOfTheWeek[7];
                daysOfWeek[0] = DayOfTheWeek.Monday;

                foreach (Room room in course.Rooms)
                {
                    if (room.GetType() == typeof(CourseRoom))
                    {
                        roomAddresses.Add(new EmailAddress(((CourseRoom)room).number + "@" + ConfigManager.getConfigString("fqdn").ToString()));
                        locationString += "Room " + ((CourseRoom)room).number + ";";
                    }
                }

                foreach (Person person in course.Attendants)
                {
                    if (person.GetType() == typeof(StudentsClass))
                    {
                        attendantAddresses.Add(new EmailAddress(((StudentsClass)person).course_name + "@" + ConfigManager.getConfigString("fqdn").ToString()));
                    }
                }

                if (course.Repeating != null)
                {
                    if (course.Repeating.GetType().Equals(typeof(RepeatDaily)))
                    {
                        RepeatDaily daily = (RepeatDaily)course.Repeating;
                        pattern = new Recurrence.DailyPattern(reservation.DateStart, daily.interval);
                    }
                    if (course.Repeating.GetType().Equals(typeof(RepeatWeekly)))
                    {
                        RepeatWeekly weekly = (RepeatWeekly)course.Repeating;
                        pattern = new Recurrence.WeeklyPattern(reservation.DateStart, weekly.interval, daysOfWeek);
                    }
                    if (course.Repeating.GetType().Equals(typeof(RepeatDaily)))
                    {
                        RepeatMonthly monthly = (RepeatMonthly)course.Repeating;
                        pattern = new Recurrence.MonthlyPattern();
                    }
                    if (course.Repeating.GetType().Equals(typeof(RepeatDaily)))
                    {
                        RepeatYearly yearly = (RepeatYearly)course.Repeating;
                        pattern = new Recurrence.YearlyPattern();
                    }


                    recurrence = AccountObjectFactory.createRecurrence(
                        course.DateStart,
                        course.Repeating.until,
                        course.Repeating.count,
                        pattern,
                        course.Repeating.IsForever);
                }

                AccountObjectFactory.CreateCalendarObject(ewsConnector,
                    course.name,
                    course.name,
                    course.DateStart,
                    course.DateEnd,
                    locationString,
                    recurrence,
                    roomAddresses,
                    attendantAddresses
                );
            }
        }
    }
}
