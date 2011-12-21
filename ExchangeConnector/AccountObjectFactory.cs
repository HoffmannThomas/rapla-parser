using System;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.Generic;

namespace ExchangeConnector
{
    public static class AccountObjectFactory
    {

        public static void SendEmail(EWSConnector connector, String subject, String body, String recipient)
        {
            Logger.Log.message("Sending message...");

            // Create the e-mail message, set its properties, and send it to user2@contoso.com, saving a copy to the Sent Items folder. 
            EmailMessage message = new EmailMessage(connector.getEWSService());
            message.Subject = subject;
            message.Body = body;
            message.ToRecipients.Add(recipient + "@" + connector.getFQDN());
            message.SendAndSaveCopy();

            // Write confirmation message to console window.
            Logger.Log.message("Message sent!");
        }

        public static void CreateCalendarObject(ExchangeService service, FolderId folderID, String id, String subject, String body, DateTime start, DateTime end, String location, Recurrence recurrence, List<EmailAddress> roomAddresses, List<EmailAddress> attendantAddresses)
        {
            Logger.Log.message("Creating appointment " + id + "...");

            // Create the appointment.
            Appointment appointment = new Appointment(service);

            // Set properties on the appointment. Add two required attendees and one optional attendee.
            appointment.Subject = subject;
            appointment.Body = id;
            appointment.Start = start;
            appointment.End = end;
            appointment.Location = location;
            appointment.Recurrence = recurrence;

            foreach (EmailAddress roomAddress in roomAddresses)
            {
                appointment.Resources.Add(new Attendee(roomAddress));
            }

            foreach (EmailAddress attendantAddress in attendantAddresses)
            {
                appointment.Resources.Add(new Attendee(attendantAddress));
            }

            // Send the meeting request to all attendees and save a copy in the Sent Items folder.
            appointment.Save(folderID, SendInvitationsMode.SendToAllAndSaveCopy);

            Logger.Log.message("Appointment " + id +" createt.");
        }

        public static Recurrence createRecurrence(DateTime start, DateTime end, int occurrences, Recurrence pattern, bool forever)
        {
            pattern.EndDate = end;

            if (occurrences != int.MinValue)
            {
                pattern.NumberOfOccurrences = occurrences;
            }

            if (forever == true)
            {
                pattern.NeverEnds();
            }

            return pattern;
        }
    }
}
