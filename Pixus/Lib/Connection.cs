using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Pixus.Lib
{
    class Connection
    {
        // Importing dll (Win API)
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool State()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
    }
}
