using System;
using System.Collections.Generic;
using System.Text;
using Pixus.Lib;
using System.Drawing; // pour la Class Color

namespace Pixus.Core
{
    class Map
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        public static Point[] PixelsToCheck = { new Point(100, 5), // haut gauche
                                                //new Point(275, 5), // haut milieu
                                                new Point(650, 5), // haut droite
                                                //new Point(100, 245), // milieu gauche
                                                //new Point(275, 245), // milieu milieu
                                                //new Point(650, 245), // milieu droite
                                                new Point(650, 490), // bas droite
                                                //new Point(275, 490), // bas milieu
                                                new Point(100, 490) // bas gauche
                                              };
        //public static Color LoadColor = Color.FromArgb(17, 17, 17); // noir
        public const int LoadTimeout = 1000; // incrémentation par seconde (1000)
        public const int MaxLoadTimeout = 10000; // 10 seconde

        private const int MAP_Y_NO_LIFE_BAR = 490;
        private static bool PixelsToCheckAlreadyFit = false;

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // GetPixelsToCheckColor(...) : retourne la/les couleur(s) des pixels à vérifier
        public static List<Color> GetPixelsToCheckColor(IntPtr GameHandle)
        {
            // on récupère les couleurs des pixels (un peu éparpillé) de la Map
            List<Color> pixelsColor = new List<Color>();

            for (int i = 0; i < PixelsToCheck.Length; i++)
            {
                pixelsColor.Add(Pixel.GetColorAt(GameHandle, PixelsToCheck[i].X, PixelsToCheck[i].Y));
            }

            return pixelsColor;
        }

        // FitPixelsToCheckTo(...) : adapte les coordonnées des pixels à vérifier
        public static void FitPixelsToCheckTo(int width, int height)
        {
            if (!PixelsToCheckAlreadyFit)
            {
                for (int i = 0; i < PixelsToCheck.Length; i++)
                {
                    PixelsToCheck[i].X = Pixel.FitXTo(PixelsToCheck[i].X, width);
                    if (PixelsToCheck[i].Y == MAP_Y_NO_LIFE_BAR)
                        PixelsToCheck[i].Y = Pixel.FitCoordinateTo(Math.Min(PixelsToCheck[i].Y, height), height, MAP_Y_NO_LIFE_BAR);
                    else
                        PixelsToCheck[i].Y = Pixel.FitYTo(PixelsToCheck[i].Y, height);
                }

                PixelsToCheckAlreadyFit = true;
            }
        }
    }
}
