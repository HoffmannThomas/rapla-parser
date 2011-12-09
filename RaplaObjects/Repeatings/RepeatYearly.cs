using System;
using System.Collections.Generic;

namespace RaplaObjects.Repeatings
{
    public class RepeatYearly : Repeating
    {
        public RepeatYearly(DateTime until, int count, List<DateTime> exceptions, bool isForever)
            : base(until, count, exceptions, isForever)
        {
        }
    }
}
