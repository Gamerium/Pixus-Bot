using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // pour la Class Process
using Pixus.Core;
using Pixus.Core.FightStuffs;
using Pixus.Lib;

namespace Pixus
{
    public partial class AddSpellForm : Form
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        private Panel GamePanel;
        private Process GameProcess;
        private ListBox FightIAListBox;

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public AddSpellForm(Panel gamePanel, Process gameProcess, ListBox fightIAListBox)
        {
            InitializeComponent();
            GamePanel = gamePanel;
            GameProcess = gameProcess;
            FightIAListBox = fightIAListBox;
        }

        //=========================================================================================================================
        //                                                      events
        //=========================================================================================================================

        // event. Load du formulaire
        private void AddSpellForm_Load(object sender, EventArgs e)
        {
            // Remplissage de la combobox des sorts
            //SpellNameComboBox.DataSource = Game.Spells;
            foreach (Spell spell in Game.Spells)
            {
                SpellNameComboBox.Items.Add(spell);
            }
            SpellNameComboBox.DisplayMember = "Name";
            // Séléction du premier élément
            SpellNameComboBox.SelectedIndex = 0;
        }

        // event. Click sur le boutton 'SelectSpellBtn'
        private void SelectSpellBtn_Click(object sender, EventArgs e)
        {
            SelectSpellBtn.LostFocus += this.GetSpell_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void GetSpell_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);

            SpellXPosTextBox.Text = MousePos.X.ToString();
            SpellYPosTextBox.Text = MousePos.Y.ToString();
            if (!AddSpellBtn.Enabled) AddSpellBtn.Enabled = true;

            SelectSpellBtn.LostFocus -= this.GetSpell_Event;
            this.Cursor = Cursors.Default;
        }

        // event. Click sur le boutton 'AddSpellBtn'
        private void AddSpellBtn_Click(object sender, EventArgs e)
        {
            if (SpellNameComboBox.Text.Length > 0)
            {
                AddSpell("SPELL::" + SpellNameComboBox.Text + "::" + SpellXPosTextBox.Text + "x" + SpellYPosTextBox.Text + "::" + SpellRelaunchTurnNumericUpDown.Value + "::" + SpellLaunchTurnNumericUpDown.Value);
                //MessageBox.Show("Sort ajouté !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veuillez définir le nom du sort !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // AddSpell(...) : ajoute une nouvelle entrée à la liste d'IA de combat
        private void AddSpell(String entry)
        {
            FightIAListBox.Items.Add(entry);
            FightIAListBox.TopIndex = FightIAListBox.Items.Count - 1; // scroll to the last item
        }
    }
}
