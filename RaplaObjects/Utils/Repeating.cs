using System;
using System.Xml;

namespace RaplaObjects.Utils
{
    public class Repeating
    {
        RepeatingType repeatingType;
        DateTime until;
        int count;

        public Repeating(RepeatingType repeatingType, DateTime until, int count)
        {
            this.repeatingType = repeatingType;

            if (this.repeatingType == RepeatingType.once)
            {
                if (until != DateTime.MinValue) { throw new RepeatException(this.repeatingType.ToString() + "should not have a until value!"); }
                if (count != int.MinValue) { throw new RepeatException(this.repeatingType.ToString() + "should not have a count value!"); }
            }

            if (until != DateTime.MinValue)
            {
                if (count != int.MinValue) { throw new RepeatException("When date is provided no count is allowed!"); }
            }

            if (count != int.MinValue)
            {
                if (until != DateTime.MinValue) { throw new RepeatException("When count is provided no until is allowed!"); }
            }

            this.until = until;
            this.count = count;
        }

        public void print()
        {
            Console.WriteLine("Repeat " + this.repeatingType.ToString() + " (" + this.count + " times) until " + this.until);
        }
    }

    public enum RepeatingType
    {
        once, daily, weekly, monthly, yearly, forever
    }

    public class RepeatException : Exception
    {

        public RepeatException(String message)
            : base(message)
        {

        }

    }
}
