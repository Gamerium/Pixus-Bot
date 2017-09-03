using System;
using System.Collections.Generic;
using System.Text;

namespace Pixus.Core
{
    [Serializable]
    class Trajet : Script
    {
        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public bool Repeat { get; set; }

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public Trajet(String Name, String File, bool repeat = false) : base(File)
        {
            this.Name = Name;
            //this.File = File;
            this.Repeat = repeat;
        }

        public Trajet(String File, bool repeat = false) : base(File)
        {
            this.Repeat = repeat;
        }
    }
}
