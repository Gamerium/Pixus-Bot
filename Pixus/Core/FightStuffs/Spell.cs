using System;
using System.Collections.Generic;
using System.Text;

namespace Pixus.Core.FightStuffs
{
    class Spell
    {
        //=========================================================================================================================
        //                                                      attr.
        //=========================================================================================================================
        private String name;

        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public String Name {
            get { return getDisplayName(); }
            set { this.name = value; }
        }
        public int Pa { get; set;}
        public Element S_Element { get; set; }
        public Type S_Type { get; set; }

        //=========================================================================================================================
        //                                                      enum.
        //=========================================================================================================================
        public enum Element
        {
            None,
            Neutral,
            Earth,
            Fire,
            Water,
            Air
        }

        public enum Type
        {
            Attack,
            Heal,
            Boost
        }

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public Spell(String name, int pa, Element element, Type type)
        {
            this.Name = name;
            this.Pa = pa;
            this.S_Element = element;
            this.S_Type = type;
        }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================
        private String getDisplayName()
        {
            String suffixe = "";

            switch (this.S_Type)
            {
                case Type.Attack:
                    suffixe = "[Attaque]";
                    break;
                case Type.Heal:
                    suffixe = "[Heal]";
                    break;
                case Type.Boost:
                    suffixe = "[Boost]";
                    break;
            }

            return this.name + " " + suffixe;
        }
    }
}
