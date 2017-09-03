using System;
using System.Collections.Generic;
using System.Text;

namespace Pixus.Core.FightStuffs
{
    [Serializable]
    class IA : Script
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        public const int ActionWaitTimeout = 2000; // 2 seconde

        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public List<string> NextAction { get; set; }

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public IA(String File)
            : base(File)
        {
            this.NextAction = new List<string>();
        }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // Next(...) : sauvegarde la prochaine action de l'IA
        public void Next(string[] nextActions)
        {
            if (this.NextAction.Count > 0)
                this.NextAction.Clear();

            foreach (string nextAction in nextActions)
                this.NextAction.Add(nextAction);
        }

        // NextActionToString(...) : retourne NextAction sous forme de chaine de caractère séparée par des ','
        public string NextActionToString()
        {
            string nextActionString = "";

            for (int i = 0; i < this.NextAction.Count; i++)
                nextActionString += this.NextAction[i] + (i < this.NextAction.Count - 1 ? "," : "");

            return nextActionString;
        }
    }
}
