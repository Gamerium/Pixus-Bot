using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Pixus.Core
{
    class App
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        public const String Name = "Pixus";
        public const String Version = "1.0.1";
        public static String CurrentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); //Application.StartupPath;
    }
}
