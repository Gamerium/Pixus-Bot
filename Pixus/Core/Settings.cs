using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // pour la class Point
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Pixus.Lib;

namespace Pixus.Core
{
    class Settings
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        private const string DEFAULT_FILENAME = "settings.pixus";

        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public static int MapChangeCheckPrecision { get; set; }
        public static int MapLoadTimeout { get; set; }
        public static Point InventoryPosition { get; set; }
        public static bool EnableDebug { get; set; }
        public static List<Popup> Popups { get; set; }
        public static bool LoadHistory { get; set; }
        public static History History { get; set; }
        public static Pixel FightDetection { get; set; }
        public static Pixel TurnDetection { get; set; }
        public static bool CloseFight { get; set; }
        public static Pixel CloseFightPixel { get; set; }
        public static bool DisableSpectatorMode { get; set; }
        public static Pixel DisableSpectatorModePixel { get; set; }
        public static Point StartPassTurnPosition { get; set; }
        public static int ExitFightTurn { get; set; }
        public static Point ExitFightPosition { get; set; }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // SetDefaults(...) : affecte les valeurs par défaut aux préférences
        private static void SetDefaults()
        {
            MapChangeCheckPrecision = 0;
            MapLoadTimeout = 10;
            InventoryPosition = new Point(367, 407);
            EnableDebug = false;
            Popups = Popup.GetDefaults();
            LoadHistory = true;
            History = new History();
            FightDetection = new Pixel(new Point(280, 410), Color.FromArgb(164, 57, 3));
            TurnDetection = new Pixel(new Point(286, 358), Color.FromArgb(253, 92, 15));
            CloseFight = true;
            CloseFightPixel = new Pixel(new Point(260, 357), Color.FromArgb(216, 215, 214));
            DisableSpectatorMode = true;
            DisableSpectatorModePixel = new Pixel(new Point(243, 394), Color.FromArgb(229, 228, 228));
            StartPassTurnPosition = new Point(280, 405);
            ExitFightTurn = 10;
            ExitFightPosition = new Point(247, 408);
        }

        // Init(...) : initialise les préférences puis les enregistre
        public static void Init(string fileName = DEFAULT_FILENAME)
        {
            SetDefaults();
            Save(fileName);
        }

        // Dispose(...) : supprime le fichier de configuration
        public static void Dispose(string fileName = DEFAULT_FILENAME)
        {
            File.Delete(fileName);
        }

        // Load(...) : charge les préférences du fichier de configuration
        public static bool Load(string fileName = DEFAULT_FILENAME)
        {
            if (!File.Exists(fileName))
                Init(fileName); //return false;

            Stream StreamFile = File.Open(fileName, FileMode.Open);
            BinaryFormatter binformat = new BinaryFormatter();

            try
            {
                foreach (PropertyInfo prop in typeof(Settings).GetProperties())
                {
                    object objValue = binformat.Deserialize(StreamFile);
                    prop.SetValue(typeof(Settings), objValue, null);
                }

                StreamFile.Close();
            }
            catch(Exception)// ex)
            {
                StreamFile.Close();
                Dispose(fileName);
                Init(fileName);
            }

            return true;
        }

        // Save(...) : enregistre les préférences dans le fichier de configuration
        public static void Save(string fileName = DEFAULT_FILENAME)
        {
            if (File.Exists(fileName))
                Dispose(fileName);

            Stream StreamFile = File.Open(fileName, FileMode.CreateNew);
            BinaryFormatter binformat = new BinaryFormatter();

            foreach (PropertyInfo prop in typeof(Settings).GetProperties())
            {
                binformat.Serialize(StreamFile, prop.GetValue(typeof(Settings), null));
            }

            StreamFile.Close();
        }
    }
}
