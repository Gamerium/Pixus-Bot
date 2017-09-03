using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // pour la Class Process
using Pixus.Core;
using Pixus.Lib;

namespace Pixus
{
    public partial class SettingsForm : Form
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        private Panel GamePanel;
        private Process GameProcess;

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public SettingsForm(Panel gamePanel, Process gameProcess)
        {
            InitializeComponent();
            GamePanel = gamePanel;
            GameProcess = gameProcess;
        }

        //=========================================================================================================================
        //                                                      events.
        //=========================================================================================================================

        // event. Load du formulaire
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Chargement des préférences
            //Settings.Load();

            // affichage

            // onglet [Général]
            MapChangeCheckPrecisionComboBox.SelectedIndex = Settings.MapChangeCheckPrecision;
            MapLoadTimeoutNumericUpDown.Value = Settings.MapLoadTimeout;
            InventoryXPosTextBox.Text = Settings.InventoryPosition.X.ToString();
            InventoryYPosTextBox.Text = Settings.InventoryPosition.Y.ToString();
            EnableDebugToggleSwitch.Checked = Settings.EnableDebug;
            LoadHistoryToggleSwitch.Checked = Settings.LoadHistory;

            // onglet [Popups]
            foreach (Popup Popup in Settings.Popups)
            {
                ListViewItem item = new ListViewItem(Popup.Name);
                item.SubItems.Add(Popup.Position.X + "x" + Popup.Position.Y);
                item.SubItems.Add(Popup.Color.R + "," + Popup.Color.G + "," + Popup.Color.B);
                PopupListView.Items.Add(item);
            }

            // onglet [Historique]
            foreach (Trajet Trajet in Settings.History.TrajetBot)
            {
                ListViewItem item = new ListViewItem(Trajet.Name);
                item.SubItems.Add(Trajet.File);
                TrajetBotListView.Items.Add(item);
            }

            foreach (Script Script in Settings.History.FightIA)
            {
                ListViewItem item = new ListViewItem(Script.Name);
                item.SubItems.Add(Script.File);
                FightIAListView.Items.Add(item);
            }

            // onglet [Combat]
            FightDetectionXPosTextBox.Text = Settings.FightDetection.Position.X.ToString();
            FightDetectionYPosTextBox.Text = Settings.FightDetection.Position.Y.ToString();
            FightDetectionColorPictureBox.BackColor = Settings.FightDetection.Color;

            TurnDetectionXPosTextBox.Text = Settings.TurnDetection.Position.X.ToString();
            TurnDetectionYPosTextBox.Text = Settings.TurnDetection.Position.Y.ToString();
            TurnDetectionColorPictureBox.BackColor = Settings.TurnDetection.Color;

            CloseFightToggleSwitch.Checked = Settings.CloseFight;
            CloseFightXPosTextBox.Text = Settings.CloseFightPixel.Position.X.ToString();
            CloseFightYPosTextBox.Text = Settings.CloseFightPixel.Position.Y.ToString();
            CloseFightColorPictureBox.BackColor = Settings.CloseFightPixel.Color;

            DisableSpectatorModeToggleSwitch.Checked = Settings.DisableSpectatorMode;
            DisableSpectatorModeXPosTextBox.Text = Settings.DisableSpectatorModePixel.Position.X.ToString();
            DisableSpectatorModeYPosTextBox.Text = Settings.DisableSpectatorModePixel.Position.Y.ToString();
            DisableSpectatorModeColorPictureBox.BackColor = Settings.DisableSpectatorModePixel.Color;

            StartPassTurnXPosTextBox.Text = Settings.StartPassTurnPosition.X.ToString();
            StartPassTurnYPosTextBox.Text = Settings.StartPassTurnPosition.Y.ToString();

            ExitFightTurnNumericUpDown.Value = Settings.ExitFightTurn;
            ExitFightXPosTextBox.Text = Settings.ExitFightPosition.X.ToString();
            ExitFightYPosTextBox.Text = Settings.ExitFightPosition.Y.ToString();
        }

        // event. Click on button 'CancelSettingsBtn'
        private void CancelSettingsBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // event. Click on button 'ApplySettingsBtn'
        private void ApplySettingsBtn_Click(object sender, EventArgs e)
        {
            // sauvegarde des préférences

            // onglet [Général]
            Settings.MapChangeCheckPrecision = MapChangeCheckPrecisionComboBox.SelectedIndex;
            Settings.MapLoadTimeout = (int)MapLoadTimeoutNumericUpDown.Value;
            Settings.InventoryPosition = new Point(Int32.Parse(InventoryXPosTextBox.Text), Int32.Parse(InventoryYPosTextBox.Text));
            Settings.EnableDebug = EnableDebugToggleSwitch.Checked;
            Settings.LoadHistory = LoadHistoryToggleSwitch.Checked;

            // onglet [Popups]
            Settings.Popups.Clear();

            foreach (ListViewItem item in PopupListView.Items)
            {
                string name = item.SubItems[0].Text;
                string[] pos = item.SubItems[1].Text.Split('x');
                string[] colorSplited = item.SubItems[2].Text.Split(',');
                Point position = new Point(Int32.Parse(pos[0]), Int32.Parse(pos[1]));
                Color color = Color.FromArgb(Int32.Parse(colorSplited[0]), Int32.Parse(colorSplited[1]), Int32.Parse(colorSplited[2]));

                Popup popup = new Popup(name, position, color);
                Settings.Popups.Add(popup);
            }

            // onglet [Historique]
            Settings.History.TrajetBot.Clear();

            foreach (ListViewItem item in TrajetBotListView.Items)
            {
                Trajet trajet = new Trajet(item.SubItems[1].Text);
                Settings.History.TrajetBot.Add(trajet);
            }

            Settings.History.FightIA.Clear();

            foreach (ListViewItem item in FightIAListView.Items)
            {
                Script IA = new Script(item.SubItems[1].Text);
                Settings.History.FightIA.Add(IA);
            }

            // onglet [Combat]
            Settings.FightDetection = new Pixel(new Point(Int32.Parse(FightDetectionXPosTextBox.Text), Int32.Parse(FightDetectionYPosTextBox.Text)), FightDetectionColorPictureBox.BackColor);

            Settings.TurnDetection = new Pixel(new Point(Int32.Parse(TurnDetectionXPosTextBox.Text), Int32.Parse(TurnDetectionYPosTextBox.Text)), TurnDetectionColorPictureBox.BackColor);

            Settings.CloseFight = CloseFightToggleSwitch.Checked;
            Settings.CloseFightPixel = new Pixel(new Point(Int32.Parse(CloseFightXPosTextBox.Text), Int32.Parse(CloseFightYPosTextBox.Text)), CloseFightColorPictureBox.BackColor);

            Settings.DisableSpectatorMode = DisableSpectatorModeToggleSwitch.Checked;
            Settings.DisableSpectatorModePixel = new Pixel(new Point(Int32.Parse(DisableSpectatorModeXPosTextBox.Text), Int32.Parse(DisableSpectatorModeYPosTextBox.Text)), DisableSpectatorModeColorPictureBox.BackColor);

            Settings.StartPassTurnPosition = new Point(Int32.Parse(StartPassTurnXPosTextBox.Text), Int32.Parse(StartPassTurnYPosTextBox.Text));

            Settings.ExitFightTurn = (int)ExitFightTurnNumericUpDown.Value;
            Settings.ExitFightPosition = new Point(Int32.Parse(ExitFightXPosTextBox.Text), Int32.Parse(ExitFightYPosTextBox.Text));

            Settings.Save();

            ApplySettingsBtn.Enabled = false;
            CancelSettingsBtn.Text = "Fermer";
        }

        //=========================================================================================================================
        //                                             events. de l'onglet [Général]
        //=========================================================================================================================

        // [Général] event. Click on button 'GetInventoryPosBtn'
        private void GetInventoryPosBtn_Click(object sender, EventArgs e)
        {
            GetInventoryPosBtn.LostFocus += this.GetInventoryPos_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void GetInventoryPos_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);

            InventoryXPosTextBox.Text = MousePos.X.ToString();
            InventoryYPosTextBox.Text = MousePos.Y.ToString();

            GetInventoryPosBtn.LostFocus -= this.GetInventoryPos_Event;
            this.Cursor = Cursors.Default;
        }

        //=========================================================================================================================
        //                                             events. de l'onglet [Popups]
        //=========================================================================================================================

        // [Popups] event. Click on button 'DeletePopupBtn'
        private void DeletePopupBtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in PopupListView.SelectedItems)
                PopupListView.Items.Remove(item);
        }

        // [Popups] event. Click on button 'DownPopupBtn'
        private void DownPopupBtn_Click(object sender, EventArgs e)
        {
            if (PopupListView.SelectedIndices.Count > 0 && PopupListView.SelectedIndices[0] < PopupListView.Items.Count - 1)
            {
                ListViewItem itemToMove = PopupListView.SelectedItems[0];
                int indexTo = PopupListView.SelectedIndices[0] + 1;
                PopupListView.Items.RemoveAt(PopupListView.SelectedIndices[0]);
                PopupListView.Items.Insert(indexTo, itemToMove);
                PopupListView.Items[indexTo].Selected = PopupListView.Items[indexTo].Focused = true;
                PopupListView.Select();
            }
        }

        // [Popups] event. Click on button 'UpPopupBtn'
        private void UpPopupBtn_Click(object sender, EventArgs e)
        {
            if (PopupListView.SelectedIndices.Count > 0 && PopupListView.SelectedIndices[0] > 0) // > 0 => 1 - 1 = 0 (maximum up position)
            {
                ListViewItem itemToMove = PopupListView.SelectedItems[0];
                int indexTo = PopupListView.SelectedIndices[0] - 1;
                PopupListView.Items.RemoveAt(PopupListView.SelectedIndices[0]);
                PopupListView.Items.Insert(indexTo, itemToMove);
                PopupListView.Items[indexTo].Selected = PopupListView.Items[indexTo].Focused = true;
                PopupListView.Select();
            }
        }

        // [Popups] event. Click on button 'SelectPopupBtn'
        private void SelectPopupBtn_Click(object sender, EventArgs e)
        {
            if (GameProcess != null)
            {
                SelectPopupBtn.LostFocus += this.GetPopup_Event;
                this.Cursor = Cursors.No;
            }
            else
            {
                MessageBox.Show("Veuillez intégrer une fenêtre du jeu !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // event. (ajouté dynamiquement)
        private void GetPopup_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            int X = MousePos.X;
            int Y = MousePos.Y;
            Color ResourceColor = Pixel.GetColorAt(GameProcess.MainWindowHandle, X, Y);

            PopupXPosTextBox.Text = X.ToString();
            PopupYPosTextBox.Text = Y.ToString();
            PopupColorImagePictureBox.BackColor = ResourceColor;
            if (!AddPopupBtn.Enabled) AddPopupBtn.Enabled = true;

            SelectPopupBtn.LostFocus -= this.GetPopup_Event;
            this.Cursor = Cursors.Default;
        }

        // [Popups] event. Click on button 'AddPopupBtn'
        private void AddPopupBtn_Click(object sender, EventArgs e)
        {
            if (PopupLabelTextBox.Text.Length > 0)
            {
                ListViewItem item = new ListViewItem(PopupLabelTextBox.Text);
                item.SubItems.Add(PopupXPosTextBox.Text + "x" + PopupYPosTextBox.Text);
                item.SubItems.Add(PopupColorImagePictureBox.BackColor.R + "," + PopupColorImagePictureBox.BackColor.G + "," + PopupColorImagePictureBox.BackColor.B);
                PopupListView.Items.Add(item);
                PopupListView.EnsureVisible(PopupListView.Items.Count - 1);
                //MessageBox.Show("Popup ajoutée !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veuillez définir un libellé !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //=========================================================================================================================
        //                                             events. de l'onglet [Historique]
        //=========================================================================================================================

        // [Historique] event. Click on button 'DeleteTrajetBotBtn'
        private void DeleteTrajetBotBtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in TrajetBotListView.SelectedItems)
                TrajetBotListView.Items.Remove(item);
        }

        // [Historique] event. Click on button 'DeleteAllTrajetBotBtn'
        private void DeleteAllTrajetBotBtn_Click(object sender, EventArgs e)
        {
            if (TrajetBotListView.Items.Count > 0)
                TrajetBotListView.Items.Clear();
        }

        // [Historique] event. Click on button 'DeleteFightIABtn'
        private void DeleteFightIABtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in FightIAListView.SelectedItems)
                FightIAListView.Items.Remove(item);
        }

        // [Historique] event. Click on button 'DeleteAllFightIABtn'
        private void DeleteAllFightIABtn_Click(object sender, EventArgs e)
        {
            if (FightIAListView.Items.Count > 0)
                FightIAListView.Items.Clear();
        }

        //=========================================================================================================================
        //                                             events. de l'onglet [Combat]
        //=========================================================================================================================

        // [Combat] event. Click on button 'FightDetectionChangeBtn'
        private void FightDetectionChangeBtn_Click(object sender, EventArgs e)
        {
            if (GameProcess != null)
            {
                FightDetectionChangeBtn.LostFocus += this.FightDetection_Event;
                this.Cursor = Cursors.No;
            }
            else
            {
                MessageBox.Show("Veuillez intégrer une fenêtre du jeu !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // event. (ajouté dynamiquement)
        private void FightDetection_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            int X = MousePos.X;
            int Y = MousePos.Y;
            Color PixelColor = Pixel.GetColorAt(GameProcess.MainWindowHandle, X, Y);

            FightDetectionXPosTextBox.Text = X.ToString();
            FightDetectionYPosTextBox.Text = Y.ToString();
            FightDetectionColorPictureBox.BackColor = PixelColor;

            FightDetectionChangeBtn.LostFocus -= this.FightDetection_Event;
            this.Cursor = Cursors.Default;
        }

        // [Combat] event. Click on button 'TurnDetectionChangeBtn'
        private void TurnDetectionChangeBtn_Click(object sender, EventArgs e)
        {
            if (GameProcess != null)
            {
                TurnDetectionChangeBtn.LostFocus += this.TurnDetection_Event;
                this.Cursor = Cursors.No;
            }
            else
            {
                MessageBox.Show("Veuillez intégrer une fenêtre du jeu !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // event. (ajouté dynamiquement)
        private void TurnDetection_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            int X = MousePos.X;
            int Y = MousePos.Y;
            Color PixelColor = Pixel.GetColorAt(GameProcess.MainWindowHandle, X, Y);

            TurnDetectionXPosTextBox.Text = X.ToString();
            TurnDetectionYPosTextBox.Text = Y.ToString();
            TurnDetectionColorPictureBox.BackColor = PixelColor;

            TurnDetectionChangeBtn.LostFocus -= this.TurnDetection_Event;
            this.Cursor = Cursors.Default;
        }

        // [Combat] event. Click on button 'DisableSpectatorModeChangeBtn'
        private void DisableSpectatorModeChangeBtn_Click(object sender, EventArgs e)
        {
            if (GameProcess != null)
            {
                DisableSpectatorModeChangeBtn.LostFocus += this.DisableSpectatorMode_Event;
                this.Cursor = Cursors.No;
            }
            else
            {
                MessageBox.Show("Veuillez intégrer une fenêtre du jeu !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // event. (ajouté dynamiquement)
        private void DisableSpectatorMode_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            int X = MousePos.X;
            int Y = MousePos.Y;
            Color PixelColor = Pixel.GetColorAt(GameProcess.MainWindowHandle, X, Y);

            DisableSpectatorModeXPosTextBox.Text = X.ToString();
            DisableSpectatorModeYPosTextBox.Text = Y.ToString();
            DisableSpectatorModeColorPictureBox.BackColor = PixelColor;

            DisableSpectatorModeChangeBtn.LostFocus -= this.DisableSpectatorMode_Event;
            this.Cursor = Cursors.Default;
        }

        // [Combat] event. Click on button 'PassTurnChangeBtn'
        private void PassTurnChangeBtn_Click(object sender, EventArgs e)
        {
            StartPassTurnChangeBtn.LostFocus += this.PassTurn_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void PassTurn_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);

            StartPassTurnXPosTextBox.Text = MousePos.X.ToString();
            StartPassTurnYPosTextBox.Text = MousePos.Y.ToString();

            StartPassTurnChangeBtn.LostFocus -= this.PassTurn_Event;
            this.Cursor = Cursors.Default;
        }

        // [Combat] event. Click on button 'ExitFightSelectBtn'
        private void ExitFightSelectBtn_Click(object sender, EventArgs e)
        {
            ExitFightSelectBtn.LostFocus += this.ExitFight_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void ExitFight_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);

            ExitFightXPosTextBox.Text = MousePos.X.ToString();
            ExitFightYPosTextBox.Text = MousePos.Y.ToString();

            ExitFightSelectBtn.LostFocus -= this.ExitFight_Event;
            this.Cursor = Cursors.Default;
        }

        // [Combat] event. Click on button 'CloseFightChangeBtn'
        private void CloseFightChangeBtn_Click(object sender, EventArgs e)
        {
            if (GameProcess != null)
            {
                CloseFightChangeBtn.LostFocus += this.CloseFight_Event;
                this.Cursor = Cursors.No;
            }
            else
            {
                MessageBox.Show("Veuillez intégrer une fenêtre du jeu !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // event. (ajouté dynamiquement)
        private void CloseFight_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            int X = MousePos.X;
            int Y = MousePos.Y;
            Color PixelColor = Pixel.GetColorAt(GameProcess.MainWindowHandle, X, Y);

            CloseFightXPosTextBox.Text = X.ToString();
            CloseFightYPosTextBox.Text = Y.ToString();
            CloseFightColorPictureBox.BackColor = PixelColor;

            CloseFightChangeBtn.LostFocus -= this.CloseFight_Event;
            this.Cursor = Cursors.Default;
        }
    }
}
