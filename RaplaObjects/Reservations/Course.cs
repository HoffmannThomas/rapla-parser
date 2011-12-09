using System;

namespace RaplaObjects.Reservations
{
    public class Course : Reservation
    {
        public String name { get; private set; }

        public Course(String id, String owner, String name, DateTime start, DateTime end, Repeating repeating)
            : base(id, owner, start, end, repeating)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return base.ToString() + this.name;
        }
    }
}
