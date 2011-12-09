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
            Console.WriteLine("Owner : " + this.Owner);
            Console.WriteLine("Start-Date : " + this.DateStart);
            Console.WriteLine("End-Date : " + this.DateEnd);

            if (this.Repeating != null)
            {
                this.Repeating.print();
            }

            Console.WriteLine("Attendants:");
            foreach (Person person in this.Attendants)
            {
                person.print();
            }

            Console.WriteLine("Rooms:");
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
