using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaplaObjects.Rooms
{
    public class CourseRoom : Room
    {
        public int number { get; private set; }

        public CourseRoom(String id, int number)
            : base(id)
        {
            this.number = number;
        }

        public override string ToString()
        {
            return "CourseRoom" + this.number.ToString();
        }
    }
}
