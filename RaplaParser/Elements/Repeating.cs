using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RaplaParser.Elements
{
    class Repeating
    {
        String type = "na";
        int count = 0;
        String until = "na";

        public Repeating(XmlElement element)
        {
            try
            {
                this.type = element.GetElementsByTagName("rapla:repeating").Item(0).Attributes.GetNamedItem("type").Value;
                try
                {
                    this.count = int.Parse(element.GetElementsByTagName("rapla:repeating").Item(0).Attributes.GetNamedItem("number").Value);
                }
                catch (Exception e)
                {
                }

                try
                {
                    this.until = element.GetElementsByTagName("rapla:date").Item(0).Attributes.GetNamedItem("date").Value;
                }
                catch (Exception e)
                {
                }

            }
            catch (Exception e)
            {
                this.type = "single";
            }
        }

        public void print()
        {
            Console.WriteLine("Repeating : " + this.type + " (" + this.count + " times) until " + this.until);
        }
    }
}
