using RaplaParser.Elements.Resources;

namespace RaplaParser.Elements
{
    public abstract class Resource
    {
        protected ResourceType type;

        public Resource(ResourceType type)
        {
            this.type = type;
        }

        public abstract void print();
    }
}
