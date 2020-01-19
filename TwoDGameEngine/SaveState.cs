using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Threading;

namespace TwoDGameEngine
{
    public static class SaveState
    {
        public static string PathPlayer = $"{ Environment.SpecialFolder.ApplicationData }" + @"\TwoD\" + "PlayerConfig.xml";
        public static string PathEnemies = $"{ Environment.SpecialFolder.ApplicationData }" + @"\TwoD\" + "EnemiesConfig.xml";

        //Controls AutoSave is enabled or not
        public static bool AutoSave = true;
        //AutoSave interval
        public static int AutoSaveInterval = 10000;

        public class Values
        {
            //declare values to be saved
            public int HP { get; set; }
            public int MaxHP { get; set; }
            public int PlayerX { get; set; }
            public int PlayerY { get; set; }
            public int PlayerLevel { get; set; }
            public int GraphicsOffsetX { get; set; }
            public int GraphicsOffsetY { get; set; }
            //TODO: Other variables still need to be added here
        }
        
        //Saves values ready to be stored
        public static void SaveValues()
        {
            //update values
            Values state = new Values();

            List<Enemies.Enemy> enemies = Enemies.E;

            //Values
            state.HP = (int)Player.HP;
            state.MaxHP = (int)Player.MaxHP;
            state.PlayerX = (int)Player.Location.X;
            state.PlayerY = (int)Player.Location.Y;
            state.PlayerLevel = Player.Level;
            state.GraphicsOffsetX = (int)UIElements.GraphicsOffset_X;
            state.GraphicsOffsetY = (int)UIElements.GraphicsOffset_Y;

            //writes values to config
            WriteConfig(state, enemies);
        }

        //AutoSave values
        public static async void AutoSaveValues()
        {
            while (true)
            {
                while (AutoSave)
                {
                    SaveValues();

                    Console.WriteLine("------------SAVED CONFIG------------");

                    await Task.Delay(AutoSaveInterval);
                }

                await Task.Delay(10000);
            }
        }

        //Load values from config
        public static void LoadConfig()
        {
            if (File.Exists(PathPlayer))
            {

                //Sets players info to config values
                Values statePlayer = new Values();

                XmlSerializer serializerPlayer = new XmlSerializer(typeof(Values));
                using (FileStream fsEnemies = File.OpenRead(PathPlayer))
                {
                    statePlayer = (Values)serializerPlayer.Deserialize(fsEnemies);
                }

                //Sets enemies info to config values
                List<Enemies.Enemy> stateEnemies = Enemies.E;

                XmlSerializer serializerEnemies = new XmlSerializer(typeof(List<Enemies.Enemy>));
                using (FileStream fsEnemies = File.OpenRead(PathEnemies))
                {
                    List<Enemies.Enemy> range = (List<Enemies.Enemy>)(serializerEnemies.Deserialize(fsEnemies));
                    stateEnemies.Clear();
                    stateEnemies.AddRange(range);
                }

                //Sets players info to config values
                Player.HP = statePlayer.HP;
                Player.MaxHP = statePlayer.MaxHP;
                Player.Location.X = statePlayer.PlayerX;
                Player.Location.Y = statePlayer.PlayerY;
                Player.Level = statePlayer.PlayerLevel;
                UIElements.GraphicsOffset_X = statePlayer.GraphicsOffsetX;
                UIElements.GraphicsOffset_Y = statePlayer.GraphicsOffsetY;
            }
            else
            {
                //Create configs if they do not exist
                Directory.CreateDirectory($"{ Environment.SpecialFolder.ApplicationData }" + @"\TwoD\");
                File.Create(PathPlayer);
                File.Create(PathEnemies);
            }
        }

        //Store values to config
        public static void WriteConfig(Values v, List<Enemies.Enemy> e)
        {
            if (File.Exists(PathPlayer))
            {
                //write player values to file in %appdata%
                XmlSerializer serializerPlayer = new XmlSerializer(typeof(Values));
                using (TextWriter twPlayer = new StreamWriter(PathPlayer))
                {
                    serializerPlayer.Serialize(twPlayer, v);
                }
            }

            if (File.Exists(PathEnemies))
            {
                //write enemies values to file in %appdata%
                XmlSerializer serializerEnemies = new XmlSerializer(typeof(List<Enemies.Enemy>));
                using (TextWriter twEnemies = new StreamWriter(PathEnemies))
                {
                    serializerEnemies.Serialize(twEnemies, e);
                }
            }
        }
    }
}
