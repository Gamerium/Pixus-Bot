using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // pour la Class Rectangle
using System.Windows.Forms; // pour la Class Screen
using System.Diagnostics; // pour la Class Process
using System.Threading; // pour la Class Thread
using Pixus.Lib;
using Pixus.Core.JobStuffs;
using Pixus.Core.FightStuffs;

namespace Pixus.Core
{
    class Game
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        public const String ProcessName = "Dofus";
        private static Size DefaultSize = new Size(990, 685);
        public static Rectangle DefaultRect = new Rectangle(Screen.PrimaryScreen.Bounds.Width / 2 - DefaultSize.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - DefaultSize.Height / 2, DefaultSize.Width, DefaultSize.Height);
        public static Point BorderPoint = new Point(10, 30);
        public static Size BorderSize = new Size(BorderPoint.X * 2, BorderPoint.Y + 10);
        public static String AppPath = App.CurrentPath;
        public const int TrajetWaitTime = 1000; // 1 seconde
        public const int FightWaitTime = 1000; // 1 seconde

        // Resources
        public static Resource[] Resources = { 
                                                 new Resource("Ble", 2),
                                                 new Resource("Lin", 2),
                                                 new Resource("Malt", 2),
                                                 new Resource("Houblon", 2),
                                                 new Resource("Avoine", 2),
                                                 new Resource("Seigle", 2),
                                                 new Resource("Bois de Bombu", 5),
                                                 new Resource("Bois de Cendre", 5),
                                                 new Resource("Bois de Châtaigne", 5),
                                                 new Resource("Bois de Chêne", 5),
                                                 new Resource("Bois de Noyer", 5)
                                             };

        // Spells
        public static Spell[] Spells = {
                                            new Spell("Flèche magique", 4, Spell.Element.Fire, Spell.Type.Attack),
                                            new Spell("Flèche de recul", 4, Spell.Element.Air, Spell.Type.Attack),
                                            new Spell("Tir eloigné", 3, Spell.Element.None, Spell.Type.Boost)
                                        };

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // getOpenWindowsList() : récupère les fenêtres ouvertes du jeu
        public static List<String> getOpenWindowsList()
        {
            List<String> gameWindowsList = new List<String>();

            // on récupère tout les processus
            Process[] processList = Process.GetProcesses();

            // on cherche des processus/fenêtres ouvertes du jeu
            foreach (Process process in processList)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle) && process.ProcessName == Game.ProcessName)
                    gameWindowsList.Add(process.MainWindowTitle);
            }

            return gameWindowsList;
        }

        // getProcessByWindowTitle(...) : retourne le processus du nom de la fenêtre passé en paramétre
        public static Process getProcessByWindowTitle(String windowTitle)
        {
            // on récupère tout les processus
            Process[] processList = Process.GetProcesses();

            foreach (Process process in processList)
            {
                if (process.MainWindowTitle == windowTitle)
                    return process;
            }

            return null;
        }

        // ClosePopUps(...) : ferme les fenêtres popup du jeu
        public static bool ClosePopUps(IntPtr GameHandle, Log Log)
        {
            bool popupsClosed = false;

            foreach (Popup popup in Settings.Popups)
            {
                if (Pixel.HasColor(popup.Position, popup.Color, GameHandle))
                {
                    Log.Debug("Closing Popup : " + popup.Name);
                    FakeClick.ClickOnPoint(GameHandle, new Point(popup.Position.X, popup.Position.Y));
                    Thread.Sleep(100);
                    popupsClosed = true;
                }
            }

            return popupsClosed;
        }
    }
}
