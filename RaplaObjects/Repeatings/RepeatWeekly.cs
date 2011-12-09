using System;
using System.Collections.Generic;

namespace RaplaObjects.Repeatings
{
    public class RepeatWeekly : Repeating
    {
        public int interval { get; private set; }

        public RepeatWeekly(int interval, DateTime until, int count, List<DateTime> exceptions, bool isForever)
            : base(until, count, exceptions, isForever)
        {
            this.interval = interval;
        }

        public override void print()
        {
            base.print();
            Console.WriteLine("Interval: Every " + this.interval + " day");
        }
    }
}
