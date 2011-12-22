using System;
using ConfigurationManager;
using RaplaObjects;
using ExchangeConnector;
using RaplaObjects.Reservations;
using Microsoft.Exchange.WebServices.Data;
using RaplaObjects.Repeatings;
using RaplaObjects.Rooms;
using System.Collections.Generic;
using RaplaObjects.Persons;
using System.Text.RegularExpressions;

namespace Connector
{
    public static class RaplaConnectorTools
    {
        //public static FolderId getRaplaFolder(ExchangeService service)
        //{
        //    string raplaCalendar = "Rapla";
        //    FolderView view = new FolderView(50);
        //    SearchFilter filter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, raplaCalendar);

        //    Folder parent = Folder.Bind(service, WellKnownFolderName.Calendar);
        //    FindFoldersResults result = parent.FindFolders(filter, view);

        //    Logger.Log.message("Looking for Rapla-Calendar...");

        //    if (result.Folders.Count == 0)
        //    {
        //        Logger.Log.message("Not found, creating new...");
        //        CalendarFolder folder = new CalendarFolder(service);
        //        folder.DisplayName = "Rapla";
        //        folder.Save(WellKnownFolderName.Calendar);
        //        Logger.Log.message("Rapla-Calendar created.");
        //        return folder.Id;
        //    }
        //    else
        //    {
        //        Logger.Log.message("Found.");
        //        return result.Folders[0].Id;
        //    }
        //}

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

        public static ItemId SaveReservation(ExchangeService service, Reservation reservation)
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

            return AccountObjectFactory.CreateCalendarObject(
                service,
                course.getID(),
                course.name,
                ConfigManager.getConfigString("appointment_body"),
                course.DateStart,
                course.DateEnd,
                locationString,
                createRecurrence(reservation),
                roomAddresses,
                attendantAddresses
            );
        }

        public static ItemId UpdateReservation(ExchangeService service, Reservation reservation)
        {
            DeleteReservation(service, reservation.getID());
            return SaveReservation(service, reservation);
        }
        public static void DeleteReservation(ExchangeService service, String itemId)
        {
            ItemId id = new ItemId(itemId);
            List<ItemId> ids = new List<ItemId>();
            ids.Add(id);
            service.DeleteItems(ids, DeleteMode.HardDelete, SendCancellationsMode.SendToNone, AffectedTaskOccurrence.AllOccurrences);
        }

        public static List<Appointment> getEWSAppointments(ExchangeService service, SearchFilter filter)
        {
            List<Appointment> appointments = new List<Appointment>();

            ItemView view = new ItemView(1000);
            FindItemsResults<Item> items = service.FindItems(WellKnownFolderName.Calendar, filter, view);

            foreach (Appointment appointment in items) {
                appointments.Add(appointment);
            }

            return appointments;
        }
    }
}
