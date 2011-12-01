using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaplaObjects.Utils;

namespace RaplaObjects.Reservations
{
    public class Course : Reservation
    {
        private String name;

        public Course(String id, String owner, String name, DateTime start, DateTime end, Repeating repeating)
            : base(id, owner, start, end, repeating)
        {
            this.name = name;
        }
    }
}
