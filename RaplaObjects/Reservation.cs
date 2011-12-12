using System;
using System.Collections.Generic;

namespace RaplaObjects
{
    public abstract class Reservation : RaplaObject
    {
        public String Owner { get; private set; }
        public DateTime DateStart { get; private set; }
        public DateTime DateEnd { get; private set; }
        public Repeating Repeating { get; private set; }
        public List<Room> Rooms { get; private set; }
        public List<Person> Attendants { get; private set; }

        public Reservation(String id, String owner, DateTime start, DateTime end, Repeating repeating) : base(id)
        {
            this.Rooms = new List<Room>();
            this.Attendants = new List<Person>();
            this.Owner = owner;
            this.DateStart = start;
            this.DateEnd = end;
            this.Repeating = repeating;
        }

        public override void print()
        {
            Logger.Log.message("Owner : " + this.Owner);
            Logger.Log.message("Start-Date : " + this.DateStart);
            Logger.Log.message("End-Date : " + this.DateEnd);

            if (this.Repeating != null)
            {
                this.Repeating.print();
            }

            Logger.Log.message("Attendants:");
            foreach (Person person in this.Attendants)
            {
                person.print();
            }

            Logger.Log.message("Rooms:");
            foreach (Room room in this.Rooms)
            {
                room.print();
            }
        }

        public void addAttendant(Person person)
        {
            this.Attendants.Add(person);
        }

        public void addRoom(Room room)
        {
            this.Rooms.Add(room);
        }

        public override string ToString()
        {
            return this.Owner + this.DateStart + this.DateEnd + Repeating.ToString();
        }
    }
}
