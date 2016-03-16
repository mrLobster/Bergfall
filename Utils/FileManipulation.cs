using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Bergfall.Utils
{
    public class FileManipulation
    {
        public static void SaveData(string file, object objectToSave, bool binary)
        {
            if (!binary)
            {
                FileStream fs = File.Create(@"C:\sf\" + file);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(objectToSave.ToString());
                sw.Close();
                fs.Close();
            }
        }

        public static void SaveData(string fullFilePath, object objectToSave)
        {
            BackupData(fullFilePath);
            BinaryFormatter serializer = new BinaryFormatter();
            using (FileStream fs = new FileStream(fullFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                serializer.Serialize(fs, objectToSave);
            }
        }

        public static object LoadData(string fullFilePath)
        {
            if (File.Exists(fullFilePath))
            {
                BinaryFormatter serializer = new BinaryFormatter();

                using (FileStream fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        return serializer.Deserialize(fs);
                    }
                }
            }
            else
            {
                return new object();
            }
            return new object();
        }

        public static void SerializeData(string file, Type type, object o)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            using (FileStream fs = new FileStream(@"A:\Development\BergfallFileManager\" + file, FileMode.Create, FileAccess.ReadWrite))
            {
                serializer.Serialize(fs, o);
            }
        }

        public static void BackupData(string file)
        {
            string filename = @"A:\Development\BergfallFileManager\" + file;
            if (File.Exists(filename))
            {
                File.Copy(filename, filename + "_" + new Random().Next(1, 1000000));
            }
        }

        public static object DeSerializeData(string file, Type type, object o)
        {
            if (File.Exists(@"A:\Development\BergfallFileManager\" + file))
            {
                XmlSerializer serializer = new XmlSerializer(type);
                using (FileStream fs = new FileStream(@"A:\Development\BergfallFileManager\" + file, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        return serializer.Deserialize(fs);
                    }
                }
            }
            else
            {
                return new object();
            }
            return new object();
        }
    }
}