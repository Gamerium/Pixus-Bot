using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Pixus.Core
{
    [Serializable]
    class Popup
    {
        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public string Name { get; set; }
        public Point Position { get; set; }
        public Color Color { get; set; }

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public Popup(string name, Point position, Color color)
        {
            this.Name = name;
            this.Position = position;
            this.Color = color;
        }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // GetDefaults() : retourne la liste des fenêtres popups par défaut
        public static List<Popup> GetDefaults()
        {
            List<Popup> Popups = new List<Popup>();

            Popups.Add(new Popup("Invitation groupe/koliz ou notification (<)", new Point(36, 228), Color.FromArgb(217, 88, 15)));
            Popups.Add(new Popup("Action indisponible (<)", new Point(36, 230), Color.FromArgb(146, 134, 126)));
            Popups.Add(new Popup("Fenêtre large (x)", new Point(503, 29), Color.FromArgb(230, 117, 41)));
            Popups.Add(new Popup("Inventaire (x)", new Point(524, 35), Color.FromArgb(217, 88, 15)));
            Popups.Add(new Popup("Caractéristiques (x)", new Point(182, 31), Color.FromArgb(242, 123, 43)));
            Popups.Add(new Popup("Métier Level up (ok)", new Point(279, 235), Color.FromArgb(241, 106, 28)));
            Popups.Add(new Popup("XP Level up (x)", new Point(386, 157), Color.FromArgb(141, 72, 25)));
            Popups.Add(new Popup("Succès (Tout accepter)", new Point(273, 302), Color.FromArgb(241, 118, 39)));
            Popups.Add(new Popup("Succès (coffret)", new Point(278, 343), Color.FromArgb(135, 58, 14)));
            Popups.Add(new Popup("Echange (non)", new Point(282, 233), Color.FromArgb(235, 91, 17)));
            Popups.Add(new Popup("Défi (non)", new Point(282, 238), Color.FromArgb(235, 89, 15)));
            Popups.Add(new Popup("Fin combat [Monstre x1] (Fermer)", new Point(273, 246), Color.FromArgb(209, 105, 35)));
            Popups.Add(new Popup("Abandonner combat (ok)", new Point(246, 235), Color.FromArgb(241, 111, 33)));
            Popups.Add(new Popup("Havre-Sac débloqué (x)", new Point(418, 128), Color.FromArgb(118, 61, 20)));

            return Popups;
        }
    }
}
