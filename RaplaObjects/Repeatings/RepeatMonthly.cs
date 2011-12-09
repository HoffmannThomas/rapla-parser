using System;
using System.Collections.Generic;

namespace RaplaObjects.Repeatings
{
    public class RepeatMonthly : Repeating
    {

        public RepeatMonthly(DateTime until, int count, List<DateTime> exceptions, bool isForever)
            : base(until, count, exceptions, isForever)
        {
        }
    }
}
