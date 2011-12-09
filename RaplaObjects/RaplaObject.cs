using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaplaObjects
{
    public abstract class RaplaObject
    {
        protected String id;

        public RaplaObject(String id)
        {
            this.id = id;
        }
        public abstract void print();

        public String getID()
        {
            return this.id;
        }

        public abstract override string ToString();
    }
}
