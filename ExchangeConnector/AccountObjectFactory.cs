using System;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeConnector
{
    public static class AccountObjectFactory
    {

        public static void SendEmail(EWSConnector connector, String subject, String body, String recipient)
        {
            Console.WriteLine("Sending message...");

            // Create the e-mail message, set its properties, and send it to user2@contoso.com, saving a copy to the Sent Items folder. 
            EmailMessage message = new EmailMessage(connector.getEWSService());
            message.Subject = subject;
            message.Body = body;
            message.ToRecipients.Add(recipient + "@" + connector.getFQDN());
            message.SendAndSaveCopy();

            // Write confirmation message to console window.
            Console.WriteLine("Message sent!");
        }

        public static void CreateCalendarObject(EWSConnector connector, String subject, String body, DateTime start, DateTime end, String location)
        {
            Console.WriteLine("Creating appointment...");

            // Create the appointment.
            Appointment appointment = new Appointment(connector.getEWSService());

            // Set properties on the appointment. Add two required attendees and one optional attendee.
            appointment.Subject = subject;
            appointment.Body = body;
            appointment.Start = start;
            appointment.End = end;
            appointment.Location = location;

            // Send the meeting request to all attendees and save a copy in the Sent Items folder.
            appointment.Save(SendInvitationsMode.SendToAllAndSaveCopy);

            Console.WriteLine("Appointment createt!");
        }
    }
}
