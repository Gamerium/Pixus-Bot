using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // pour la Class Point
using Pixus.Lib;

namespace Pixus.Core
{
    class Move
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        public static Point UP = new Point(400, 1); // Defaults values
        public static Point DOWN = new Point(400, 485);
        public static Point LEFT = new Point(40, 270);
        public static Point RIGHT = new Point(755, 270);

        public const int SCREEN_X = 800; // UP.X * 2;
        public const int SCREEN_Y = 540; // RIGHT.Y * 2;
        private const int SCREEN_Y_NO_LIFE_BAR = 485; // DOWN.Y;

        private static bool MovesAlreadyFit = false;

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // FitMovesTo(...) : adapte les coordonnées des mouvements
        public static void FitMovesTo(int width, int height)
        {
            if (!MovesAlreadyFit)
            {
                UP.X = Pixel.FitXTo(UP.X, width);
                UP.Y = Pixel.FitYTo(UP.Y, height);

                DOWN.X = Pixel.FitXTo(DOWN.X, width);
                DOWN.Y = Pixel.FitCoordinateTo(Math.Min(DOWN.Y, height), height, SCREEN_Y_NO_LIFE_BAR);

                RIGHT.X = Pixel.FitXTo(RIGHT.X, width);
                RIGHT.Y = Pixel.FitYTo(RIGHT.Y, height);

                LEFT.X = Pixel.FitXTo(LEFT.X, width);
                LEFT.Y = Pixel.FitYTo(LEFT.Y, height);

                MovesAlreadyFit = true;
            }
        }

        // Step(...) : Bouge le Bot
        private static void Step(IntPtr GameHandle, Point to)
        {
            FakeClick.ClickOnPoint(GameHandle, to);
        }

        // Up(...) : Bouge le Bot en haut
        public static void Up(int x, int width, IntPtr GameHandle)
        {
            Step(GameHandle, new Point(Pixel.FitXTo(x, width), UP.Y));
        }

        // Down(...) : Bouge le Bot en bas
        public static void Down(int x, int width, IntPtr GameHandle)
        {
            Step(GameHandle, new Point(Pixel.FitXTo(x, width), DOWN.Y));
        }

        // Left(...) : Bouge le Bot à gauche
        public static void Left(int y, int height, IntPtr GameHandle)
        {
            Step(GameHandle, new Point(LEFT.X, Pixel.FitYTo(y, height)));
        }

        // Right(...) : Bouge le Bot à droite
        public static void Right(int y, int height, IntPtr GameHandle)
        {
            Step(GameHandle, new Point(RIGHT.X, Pixel.FitYTo(y, height)));
        }
    }
}
