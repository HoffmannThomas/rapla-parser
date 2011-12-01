using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaplaObjects.Rooms
{
    public class CourseRoom : Room
    {
        int number;

        public CourseRoom(String id, int number)
            : base(id)
        {
            this.number = number;
        }

        public override void print()
        {
            Console.WriteLine("    " + "CourseRoom" + ": " + this.number);
        }
    }
}
