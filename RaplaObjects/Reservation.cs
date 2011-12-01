using System;
using System.Collections.Generic;
using RaplaObjects.Utils;

namespace RaplaObjects
{
    public abstract class Reservation : Resource
    {
        private String owner;
        private DateTime start;
        private DateTime end;
        private Repeating repeating;
        private List<Resource> resources = new List<Resource>();

        public Reservation(String id, String owner, DateTime start, DateTime end, Repeating repeating)
            : base(id)
        {
            this.owner = owner;
            this.start = start;
            this.end = end;
            this.repeating = repeating;
        }

        public override void print()
        {
            Console.WriteLine("Owner : " + this.owner);
            Console.WriteLine("Start-Date : " + this.start);
            Console.WriteLine("End-Date : " + this.end);

            this.repeating.print();

            Console.WriteLine("Resources:");
            foreach (Resource resource in this.resources)
            {
                resource.print();

            }
        }

        public void addResource(Resource resource)
        {
            this.resources.Add(resource);
        }
    }
}
