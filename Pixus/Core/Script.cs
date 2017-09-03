using System;
using System.Collections.Generic;
using System.Text;
using System.IO; // pour la class Path

namespace Pixus.Core
{
    [Serializable]
    class Script
    {
        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public String Name { get; set; }
        public String File { get; set; }

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public Script(String File)
        {
            this.File = File;
            this.Name = Path.GetFileNameWithoutExtension(File);
        }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // Equals(...) : retourne si le script actuel est égal à celui passé en paramétre
        public bool Equals(Script script)
        {
            return this.Name == script.Name;
        }
    }
}
