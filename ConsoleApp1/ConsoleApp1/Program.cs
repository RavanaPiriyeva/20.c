using System;
using System.Text.Json.Serialization;
using System.Net;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    internal class Program
    {
        static string _path = @"C:\Users\DERS\Desktop\Yeni klasör\";
        static void Main(string[] args)
        {
            object str;
            User user = new User();
            user.Name = "Revane";
            user.Password = "123456";
            SerializableDat(user);
            User user1 = DeSerializableDat();
            Console.WriteLine(user1.Name + user1.Password);
            SerializableJson(user);
            User user2 = DeSerializableJson();
            Console.WriteLine(user.Name);
            SerializeXml(user);
            User user3 = DeSerializeXml();
            Console.WriteLine(user3.Name+"   "+user3.Password);

            WebClient webClient = new WebClient();
            string date = DateTime.UtcNow.AddHours(4).ToString("dd.MM.yyyy");
            var stream = webClient.OpenRead("https://www.cbar.az/currencies/" + date + ".xml");
            
            XmlSerializer xml = new XmlSerializer(typeof(ValCurs));
            str = xml.Deserialize(stream);
            ValCurs currency1 = (ValCurs)str;
          foreach ( var item in currency1.ValType)
            {
                foreach( var val in item.Valute)
                {
                    Console.WriteLine(val.Name+"   "+val.Value);
                }
            }

        }
        static void SerializableDat(User user)
        {
            FileStream fileStream = new FileStream(_path + "user.dat", FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, user);
            fileStream.Close();
        }
        static User DeSerializableDat()
        {
            object data;
            using (FileStream fileStream = new FileStream(_path + "user.dat", FileMode.Open))
            {

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                data = binaryFormatter.Deserialize(fileStream);
            }
            return (User)data;
        }
        static void SerializableJson(User user)

        {
            var objects = JsonConvert.SerializeObject(user);
            using (FileStream fileStream = new FileStream(_path + "user.json", FileMode.Create))
            {
                using (StreamWriter stream = new StreamWriter(fileStream))
                {
                    stream.Write(objects);
                }
            }
        }

        static User DeSerializableJson()
        {
            ;
            string str;
            using (FileStream fileStream = new FileStream(_path + "user.json", FileMode.Open))
            {
                using (StreamReader stream = new StreamReader(fileStream))
                {
                    str = stream.ReadToEnd();

                }
            }
            var user = JsonConvert.DeserializeObject<User>(str);
            return user;
        }
        static void SerializeXml(User user)
        {
            using (FileStream stream = new FileStream(_path + "user.xml", FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(User));
                xml.Serialize(stream, user);
            }
        }
        static User DeSerializeXml()
        {
            
            object str;
            using (FileStream fileStream = new FileStream(_path + "user.xml", FileMode.Open))
            {

                XmlSerializer xml = new XmlSerializer(typeof(User));
                str = xml.Deserialize(fileStream);


            }

            return (User)str;
        }
    }
}
