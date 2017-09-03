using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Pixus.Lib;

namespace Pixus.Core
{
    class Minimap
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        public static Color ResourcePinColor = Color.Red;
        public static Color MonsterPinColor = Color.Blue;

        private const int GridCellSize = 4;
        private static Color GridColor = Color.LightGray;
        private static Color GridBackColor = SystemColors.Control;
        private static Size PinSize = new Size(3, 3); // @! impair
        private static Color PinBorderColor = Color.Black;

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // DrawGrid(...) : Dessine la grid de la Minimap
        private static Bitmap DrawGrid(PictureBox MinimapPictureBox)
        {
            if (MinimapPictureBox.Image == null)
                MinimapPictureBox.Image = new Bitmap(MinimapPictureBox.Width, MinimapPictureBox.Height);

            Bitmap MinimapImage = (Bitmap)MinimapPictureBox.Image;
            Graphics g = Graphics.FromImage(MinimapImage);
            int cellSize = Minimap.GridCellSize;
            int numOfCells = MinimapPictureBox.Width / cellSize;

            Pen p = new Pen(Minimap.GridColor);
            g.Clear(GridBackColor);

            for (int i = 0; i <= numOfCells; i++)
            {
                // Vertical
                g.DrawLine(p, i * cellSize, 0, i * cellSize, numOfCells * cellSize);
                // Horizontal
                g.DrawLine(p, 0, i * cellSize, numOfCells * cellSize, i * cellSize);
            }

            return MinimapImage;
        }

        // DrawPin(...) : Dessine le repère
        private static Bitmap DrawPin(Bitmap MinimapImage, Point Pin, Color PinColor, Color BorderColor)
        {
            Rectangle rect = new Rectangle(Pin.X - ((PinSize.Width - 1) / 2), Pin.Y - ((PinSize.Height - 1) / 2), PinSize.Width, PinSize.Height);
            Graphics gr = Graphics.FromImage(MinimapImage);
            gr.FillEllipse(new SolidBrush(PinColor), rect);
            gr.DrawEllipse(new Pen(BorderColor), rect);

            return MinimapImage;
        }

        // NewPin(...) : Ajoute un nouveau repère à la Minimap
        public static void NewPin(PictureBox MinimapPictureBox, Size Map, Point Pin, Color PinColor)
        {
            // Réglage de la position du répère
            Pin.X = Pixel.FitCoordinateTo(Pin.X, MinimapPictureBox.Width, Map.Width);
            Pin.Y = Pixel.FitCoordinateTo(Pin.Y, MinimapPictureBox.Height, Map.Height);

            // Affichage du repère sur la minimap
            if (MinimapPictureBox.Image == null)
                MinimapPictureBox.Image = DrawGrid(MinimapPictureBox);

            MinimapPictureBox.Image = DrawPin(((Bitmap)MinimapPictureBox.Image), Pin, PinColor, PinBorderColor);
            //MinimapPictureBox.Refresh();
        }

        // RemovePin(...) : enlève un repère de la Minimap
        public static void RemovePin(PictureBox MinimapPictureBox, Size Map, Point Pin)
        {
            // Réglage de la position du répère
            Pin.X = Pixel.FitCoordinateTo(Pin.X, MinimapPictureBox.Width, Map.Width);
            Pin.Y = Pixel.FitCoordinateTo(Pin.Y, MinimapPictureBox.Height, Map.Height);

            // Suppression du répère
            MinimapPictureBox.Image = DrawPin(((Bitmap)MinimapPictureBox.Image), Pin, GridBackColor, GridBackColor);
            //MinimapPictureBox.Refresh();
        }

        // Clear(...) : efface/néttoie la Minimap
        public static void Clear(PictureBox MinimapPictureBox)
        {
            MinimapPictureBox.Image = DrawGrid(MinimapPictureBox);
        }

        // Load(...) : initialise la Minimap
        public static void Load(PictureBox MinimapPictureBox)
        {
            Clear(MinimapPictureBox);
        }

        // Zoom(...) : effectue un zoom sur l'image de la Minimap
        private static Bitmap Zoom(Bitmap originalBitmap, int zoomFactor = 2) // x2 (double)
        {
            Size newSize = new Size((int)(originalBitmap.Width * zoomFactor), (int)(originalBitmap.Height * zoomFactor));
            Bitmap bmp = new Bitmap(originalBitmap, newSize);

            return bmp;
        }
    }
}
