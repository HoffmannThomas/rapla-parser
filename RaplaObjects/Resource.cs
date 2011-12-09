using System;

namespace RaplaObjects
{
    public abstract class Resource : RaplaObject
    {
        public Resource(String id) :base(id)
        {
        }
        public override void print()
        {
            Console.WriteLine("    " + this.ToString());
        }

        public override string ToString()
        {
            return this.id.ToString();
        }
    }
}
