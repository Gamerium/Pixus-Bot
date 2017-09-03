using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing; // pour la Class Color
using Pixus.Core;

namespace Pixus.Lib
{
    [Serializable]
    class Pixel
    {
        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public Point Position { get; set; }
        public Color Color { get; set; }

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public Pixel(Point Position, Color Color)
        {
            this.Position = Position;
            this.Color = Color;
        }

        //=========================================================================================================================
        //                                                      autres
        //=========================================================================================================================

        // Importing dll (Win API)
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindowDC(IntPtr window);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern uint GetPixel(IntPtr dc, int x, int y);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ReleaseDC(IntPtr window, IntPtr dc);

        private static Point PointToGameWindow(Point point)
        {
            point.X += Game.BorderPoint.X;
            point.Y += Game.BorderPoint.Y;

            return point;
        }

        public static Color GetColorAt(IntPtr window, int x, int y)
        {
            IntPtr dc = GetWindowDC(window);
            Point p = PointToGameWindow(new Point(x, y));
            int a = (int)GetPixel(dc, p.X, p.Y);
            ReleaseDC(window, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }

        // ColorChanged(...) : vérifie si les pixels passé en paramétre ont changés de couleur
        public static bool ColorChanged(Point[] pixelsToCheck, List<Color> pixelsColor, IntPtr GameHandle)
        {
            if (HaveColors(pixelsToCheck, pixelsColor, GameHandle)) // si les pixels ont toujours la même couleur
                return false;
            else
                return true;
        }

        // AllColorChanged(...) : vérifie si tout les pixels passé en paramétre ont changés tous de couleur
        public static bool AllColorChanged(Point[] pixelsToCheck, List<Color> pixelsColor, IntPtr GameHandle)
        {
            for (int i = 0; i < pixelsToCheck.Length; i++)
            {
                // si la couleur d'un seul pixel n'a pas changée, on s'arrête
                if (HasColor(pixelsToCheck[i], pixelsColor[i], GameHandle))
                    return false;
            }

            return true;
        }

        // HaveColors(...) : vérifie si les pixels passé en paramétre ont toujours leur couleurs
        private static bool HaveColors(Point[] pixelsToCheck, List<Color> pixelsColor, IntPtr GameHandle)
        {
            for (int i = 0; i < pixelsToCheck.Length; i++)
            {
                // si la couleur d'un seul pixel a changée 
                if (!HasColor(pixelsToCheck[i], pixelsColor[i], GameHandle))
                    return false;
            }

            return true;
        }

        // HasColor(...) : vérifie si le pixel passé en paramétre a la couleur passée en paramétre
        public static bool HasColor(Point pixel, Color color, IntPtr GameHandle)
        {
            Color pixelColor = GetColorAt(GameHandle, pixel.X, pixel.Y);

            if (pixelColor.R == color.R && pixelColor.G == color.G && pixelColor.B == color.B)
                return true;

            return false;
        }

        // FitCoordinateTo(...) : adapte la cordonnée selon les paramétres passés
        public static int FitCoordinateTo(int coordinate, int width_or_height, int screen_x_or_y)
        {
            // screen x/y > width/height
            if (screen_x_or_y > width_or_height)
                return (int)((float)(coordinate / ((float)screen_x_or_y / width_or_height)));
            else
                return (int)((float)(coordinate * ((float)screen_x_or_y / width_or_height)));
        }

        // FitXTo(...) : adapte x selon les paramétres passés
        public static int FitXTo(int x, int width)
        {
            return FitCoordinateTo(x, width, Move.SCREEN_X);
        }

        // FitYTo(...) : adapte y selon les paramétres passés
        public static int FitYTo(int y, int height)
        {
            return FitCoordinateTo(y, height, Move.SCREEN_Y);
        }
    }
}
