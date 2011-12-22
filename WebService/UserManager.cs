using System.Xml;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
namespace WebService
{
    class UserManager
    {
        private static String path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "user.txt";
        public Dictionary<String, User> dictionary {get; private set;}

        public UserManager()
        {
            dictionary = new Dictionary<String, User>();

            if (File.Exists(path) == false)
            {
                this.save();
            }
            else
            {
                this.load();
            }
        }

        public void addUser(User user)
        {
            if (dictionary.ContainsKey(user.rapla_user_id))
            {
                dictionary.Remove(user.rapla_user_id);
            }
            dictionary.Add(user.rapla_user_id, user);
            this.save();
        }

        public void removeUser(String rapla_user_id)
        {
            dictionary.Remove(rapla_user_id);
            this.save();
        }

        public User getUser(String rapla_user_id)
        {
            return dictionary[rapla_user_id];
        }

        private void save()
        {
            StreamWriter sw = new StreamWriter(path);

            foreach (User user in dictionary.Values)
            {
                sw.WriteLine(user.ToString());
            }
            sw.Close();
        }

        private void load()
        {
            StreamReader sr = new StreamReader(path);
            String user_string;

            if (sr.EndOfStream == false)
            {
                user_string = sr.ReadLine();

                while (user_string != null)
                {
                    String[] user_array = user_string.Split(";".ToCharArray());

                    User user = new User(user_array[0], user_array[1], user_array[2], false);

                    this.dictionary.Add(user.rapla_user_id, user);

                    user_string = sr.ReadLine();
                }
            }
            sr.Close();
        }
    }
}