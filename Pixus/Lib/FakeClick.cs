using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing; // pour la Class Point
using System.Threading; // pour la Class Thread

namespace Pixus.Lib
{
    class FakeClick
    {
        // Importing dll (Win API)
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private const int WM_LBUTTONDOWN = 0x201; //Left mousebutton down
        private const int WM_LBUTTONUP = 0x202;   //Left mousebutton up
        private const int WM_LBUTTONDBLCLK = 0x203; //Left mousebutton doubleclick
        private const int WM_RBUTTONDOWN = 0x204; //Right mousebutton down
        private const int WM_RBUTTONUP = 0x205;   //Right mousebutton up
        private const int WM_RBUTTONDBLCLK = 0x206; //Right mousebutton doubleclick

        private static int MAKELPARAM(int x, int y)
        {
            return ((y << 16) | (x & 0xFFFF));
        }

        public static void RightClickOnPoint(IntPtr wndHandle, Point clientPoint)
        {
            PostMessage(wndHandle, WM_RBUTTONDOWN, 0, MAKELPARAM(clientPoint.X, clientPoint.Y));
            Thread.Sleep(5);
            PostMessage(wndHandle, WM_RBUTTONUP, 0, MAKELPARAM(clientPoint.X, clientPoint.Y));
        }

        public static void ClickOnPoint(IntPtr wndHandle, Point clientPoint)
        {
            PostMessage(wndHandle, WM_LBUTTONDOWN, 0, MAKELPARAM(clientPoint.X, clientPoint.Y));
            Thread.Sleep(5);
            PostMessage(wndHandle, WM_LBUTTONUP, 0, MAKELPARAM(clientPoint.X, clientPoint.Y));
        }

        public static void DoubleClickOnPoint(IntPtr wndHandle, Point clientPoint)
        {
            //SendMessage(wndHandle, WM_LBUTTONDBLCLK, 0, MAKELPARAM(clientPoint.X, clientPoint.Y));
            ClickOnPoint(wndHandle, clientPoint);
            Thread.Sleep(5);
            ClickOnPoint(wndHandle, clientPoint);
        }
    }
}
