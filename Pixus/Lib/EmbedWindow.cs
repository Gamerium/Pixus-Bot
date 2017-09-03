using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Pixus.Lib
{
    class EmbedWindow
    {
        // Importing dll (Win API)
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        /*
        
        [DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hwnd, ref Rectangle lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);
         
         */

        private struct RECT
        {
            int left;
            int top;
            int right;
            int bottom;

            public int Left
            {
                get { return this.left; }
            }

            public int Top
            {
                get { return this.top; }
            }

            public int Width
            {
                get { return right - left; }
            }

            public int Height
            {
                get { return bottom - top; }
            }

            public static implicit operator Rectangle(RECT rect)
            {
                return new Rectangle(rect.left, rect.top, rect.Width, rect.Height);
            }
        }

        [DllImport("user32")]
        private static extern int GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32")]
        private static extern int PrintWindow(IntPtr hWnd, IntPtr dc, uint flags);

        private static Rectangle GetWindowPlacement(IntPtr hWnd)
        {
            RECT rect = new RECT();

            GetWindowRect(hWnd, ref rect);

            return rect;
        }

        public static Bitmap GetWindowImage(IntPtr hWnd)
        {
            try
            {
                Rectangle rect = GetWindowPlacement(hWnd);
                Size size = rect.Size;
                Bitmap bmp = new Bitmap(size.Width, size.Height);
                Graphics g = Graphics.FromImage(bmp);
                IntPtr dc = g.GetHdc();

                PrintWindow(hWnd, dc, 0);

                g.ReleaseHdc();
                g.Dispose();

                //return bmp;
                return ImageHelper.CropUnwantedBackground(bmp, true);
            }
            catch { return null; }
        }
    }
}
