using System;

namespace RaplaObjects
{
    public abstract class Resource
    {
        protected String id;

        public Resource(String id)
        {
            this.id = id;
        }
        public abstract void print();

        public String getID(){
            return this.id;
        }
    }
}
