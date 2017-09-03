using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // pour la Class Process
using Pixus.Core;
using Pixus.Core.JobStuffs;
using Pixus.Lib;

namespace Pixus
{
    public partial class AddResourceForm : Form
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        private Panel GamePanel;
        private Process GameProcess;
        private ListBox AreaListBox;

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public AddResourceForm(Panel gamePanel, Process gameProcess, ListBox areaListBox)
        {
            InitializeComponent();
            GamePanel = gamePanel;
            GameProcess = gameProcess;
            AreaListBox = areaListBox;
        }

        //=========================================================================================================================
        //                                                      events
        //=========================================================================================================================

        // event. Load du formulaire
        private void AddResourceForm_Load(object sender, EventArgs e)
        {
            // Remplissage de la combobox des Ressources
            //ResourceNameComboBox.DataSource = Game.Resources;
            foreach (Resource resource in Game.Resources)
            {
                ResourceNameComboBox.Items.Add(resource);
            }
            ResourceNameComboBox.DisplayMember = "Name";
            // Séléction du premier élément
            ResourceNameComboBox.SelectedIndex = 0;
        }

        // event. Click sur le boutton 'SelectResourceBtn'
        private void SelectResourceBtn_Click(object sender, EventArgs e)
        {
            SelectResourceBtn.LostFocus += this.GetResource_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void GetResource_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            int X = MousePos.X;
            int Y = MousePos.Y;
            Color ResourceColor = Pixel.GetColorAt(GameProcess.MainWindowHandle, X, Y);

            ResourceXPosTextBox.Text = X.ToString();
            ResourceYPosTextBox.Text = Y.ToString();
            ResourceColorImagePictureBox.BackColor = ResourceColor;
            if (!AddResourceBtn.Enabled) AddResourceBtn.Enabled = true;

            SelectResourceBtn.LostFocus -= this.GetResource_Event;
            this.Cursor = Cursors.Default;
        }

        // event. Click sur le boutton 'AddResourceBtn'
        private void AddResourceBtn_Click(object sender, EventArgs e)
        {
            if (ResourceNameComboBox.Text.Length > 0)
            {
                AddResource(ResourceNameComboBox.Text + "::" + ResourceXPosTextBox.Text + "x" + ResourceYPosTextBox.Text + "::" + ResourceColorImagePictureBox.BackColor.R + "," + ResourceColorImagePictureBox.BackColor.G + "," + ResourceColorImagePictureBox.BackColor.B + "::" + Tools.boolToString(!NoResourceColorCheckBox.Checked));
                //MessageBox.Show("Ressource ajoutée !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veuillez définir le nom de la ressource !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // AddResource(...) : ajoute une nouvelle entrée à la liste des ressources d'une zone
        private void AddResource(String entry)
        {
            AreaListBox.Items.Add(entry);
            AreaListBox.TopIndex = AreaListBox.Items.Count - 1; // scroll to the last item
        }
    }
}
