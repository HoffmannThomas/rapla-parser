using System;
using System.Xml;
using System.Collections.Generic;

namespace RaplaObjects
{
    public abstract class Repeating
    {
        public DateTime until { get; private set; }
        public int count { get; private set; }
        public List<DateTime> exceptions { get; private set; }
        public bool IsForever { get; private set; }

        protected Repeating(DateTime until, int count, List<DateTime> exceptions, bool isForever)
        {
            this.count = count;
            this.until = until;
            this.exceptions = exceptions;
            this.IsForever = isForever;

            if (until != DateTime.MinValue)
            {
                if (count != int.MinValue) { throw new Exception("When date is provided no count is allowed!"); }
            }

            if (count != int.MinValue)
            {
                if (until != DateTime.MinValue) { throw new Exception("When count is provided no until is allowed!"); }
            }

            this.until = until;
            this.count = count;
        }

        public virtual void print()
        {
            Logger.Log.message("Repeat " + this.GetType().Name.ToString() + " (" + this.count + " times) until " + this.until);

            foreach (DateTime exception in this.exceptions)
            {
                Logger.Log.message("Exception" + exception.ToString());
            }
        }
    }
}
