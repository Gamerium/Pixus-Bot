using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Pixus.Lib
{
    class Tools
    {
        // minToMax(...) : change la valeur minimale en valeur maximale
        public static int minToMax(int value, int min, int max)
        {
            return (max - value) + min;
        }

        // boolToString(...) : retourne la valeur d'un booléan sous forme d'une chaine de caractère
        public static String boolToString(bool value)
        {
            return value ? "true" : "false";
        }

        // stringToBool(...) : retourne la valeur d'une chaine de caractère sous forme d'un booléan
        public static bool stringToBool(string value)
        {
            return value.ToLower() == "true" ? true : false;
        }

        // colorToString(...) : retourne la valeur d'un object color sous forme d'une chaine de caractère
        public static string colorToString(Color value)
        {
            return "(R: " + value.R + " G: " + value.G + " B: " + value.B + ")";
        }

        // pointToString(...) : retourne la valeur d'un object point sous forme d'une chaine de caractère
        public static string pointToString(Point value)
        {
            return "(X: " + value.X + " Y: " + value.Y + ")";
        }
    }
}
