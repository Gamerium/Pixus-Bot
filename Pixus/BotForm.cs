/*
 * Pixus Bot
 * 
 * version : 1.0.1
 * 
 * Date de création : 12/06/2016 -> 26/06/2016
 * 
 * Date de reprise : (v1.0.1 - stable) 25/08/2016 -> 30/08/2016
 * 
 * Auteur : AXeL
 * 
 * Contact : axel.prog.25@gmail.com
 * 
 *      Remarques       : - J'ai découvert que le multi boting ne fonctionnera pas avec un bot pixel, car il faut que la fenêtre du bot soit celle actif (oui on peut y remedier autrement, en changeant automatiquement les fenêtres des bots à tour de role, mais il faudra comme meme une superbe synchronisation, autrement dit un super travail à faire)
 *                        - La réduction de la fenêtre du bot ne fonctionnera pas non plus >< (pour les mouvements ça peux marcher mais pour la recherche des ressources (recherche d'image) il faut que la fenêtre soit celle actif ><), il faut donc laisser l'inteface du bot toujours au premier plan..
 *                        - La gestion des combats n'est pas encore au point (alors qu'on peux se faire agro en faisant un métier ><)
 *                        - L'interface de dofus changera à la prochaine version je parle de la version 2.36, du coup faudra changer toutes les images utilisées de l'ancienne version (un des défauts d'un bot pixel..)
 *                        - Des fois le bot plante, pour une raison un peu inconnu pour le moment (je dis bien des fois), ça peut etre à cause des fonctions de recherche d'images, même si au début je croyais que c'était à cause des RichTextBox du Log et Débogage voir meme à cause de la taille du fichier Log (on y écrit beaucoup après tout) mais bon j'en ai pas la preuve formelle puis ce que le bot plante tout simplement sans avoir d'erreur (comme cause je pense aussi aux pointeurs utilisés dans les fonctions de recherche d'images et ou le passage de données au fonctions/threads par adresse/pointeur, il se peut qu'il y'a des collisions/soucis avec le garbage collector ou autres, bref il faut bien creuser cette piste pour en avoir le coeur net)
 *                        - J'ai pu constaté sur la version 1.0.1 que le bot peut planté à cause du thread (après avoir stoppé le bot manuellement), plus précisament à cause des boucles qui ne contiennent pas les 3 instructions d'arret/pause de bot au début.
 *                        - En cherchant un peu plus, j'ai plutôt constater que l'écriture sur le Log en mode débogage pendant l'exécution du thread peut etre la vrai cause des beugs, du coup j'ai ajouter une option dans les préférences pour désactiver les journaux de débogage et j'ai effectuer certains changements sur la classe Log, et ça à l'air d'avoir bien marcher, je ne remarque plus de beug.
 * 
 * Idées d'amélioration : - Améliorer l'IA de combat > ajouter la possibilité de bouger (alors la, y'a 2 manières de faire différentes que j'ai pu trouvé, la 1ère auquel j'ai pensé, consiste à repérer les cases de la map (position + hauteur et largeur des cases) ainsi que la position du bot et ainsi on pourra le faire bouger, certes c'est dur à faire, 2ème manière que j'ai vu sur un tuto et que je trouve non efficace, consiste a générer des positions aléatoires a l'intérieur de la map et de clické dessus jusqu'a ce que la position du bot change, certes des fois ça ne fonctionnera même pas (imaginons que le bot ne bouge pas même après des centaines de tentatives), mais ça reste une possibilité assez simple à mettre en oeuvre).
 *                        - Travailler sur l'Antibot.
 *                        - Donner libre à vos idées!
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // pour la Class Process
using System.IO; // pour la Class StreamWriter
using System.Text.RegularExpressions; // pour la Class Regex
using Pixus.Core;
using Pixus.Core.JobStuffs;
using Pixus.Core.FightStuffs;
using Pixus.Lib;

namespace Pixus
{
    public partial class BotForm : Form
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        private Log Log;
        private Process GameProcess = null;
        private String BotName = "Bot";
        private Bot Bot;
        private Stopwatch BotStopWatch = new Stopwatch();

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public BotForm()
        {
            InitializeComponent();
        }

        //=========================================================================================================================
        //                                                      events
        //=========================================================================================================================

        // event. Load du formulaire
        private void BotForm_Load(object sender, EventArgs e)
        {
            GamePanel.Enabled = true;
            GameWindowComboBox.DataSource = Game.getOpenWindowsList();
            // Chargement de la Minimap
            Minimap.Load(MinimapPictureBox);
            // Chargement des préférences
            Settings.Load();
            // chargement de l'historique
            if (Settings.LoadHistory)
            {
                foreach (Trajet Trajet in Settings.History.TrajetBot)
                    TrajetBotComboBox.Items.Add(new ComboBoxItem(Trajet.Name, Trajet.File));
                foreach (Script FightIA in Settings.History.FightIA)
                    FightIAComboBox.Items.Add(new ComboBoxItem(FightIA.Name, FightIA.File));
            }
        }

        // event. Click sur le boutton 'Raffraichir'
        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            GameWindowComboBox.DataSource = Game.getOpenWindowsList();
        }

        // event. Click sur le boutton 'Intégrer'
        private void EmbedBtn_Click(object sender, EventArgs e)
        {
            if (GameWindowComboBox.Items.Count > 0)
            {
                BotName = GameWindowComboBox.SelectedValue.ToString();
                GameProcess = Game.getProcessByWindowTitle(BotName);
                if (GameProcess != null)
                {
                    // suppression du control de choix de la fenêtre du jeu
                    GamePanel.Controls.Remove(SelectWindowGroupBox);

                    // intégration du jeu dans le bot
                    EmbedWindow.SetParent(GameProcess.MainWindowHandle, GamePanel.Handle);
                    EmbedWindow.MoveWindow(GameProcess.MainWindowHandle, -Game.BorderPoint.X, -Game.BorderPoint.Y, GamePanel.Width + Game.BorderSize.Width, GamePanel.Height + Game.BorderSize.Height, true);
                    // changement du nom de la fenêtre/bot
                    this.Text = BotName;
                    // Désactivation du panel du jeu
                    GamePanel.Enabled = false;
                    // Activation des outils du trajet
                    TrajetActionsImageGroupBox.Enabled = true;
                    // Activation de la possibilité de simuler les mouvements d'un trajet
                    SimulateMoveCheckBox.Enabled = true;
                    // Activation de la possibilité de capturer l'écran du jeu
                    ScreenshotToolStripMenuItem.Enabled = true;
                    // Activation de la possibilité de vérouiller/dévérouiller l'interface du jeu
                    LockUnlockGameToolStripMenuItem.Enabled = true;
                    // Activation de la possibilité de la configuration des ressources/IA combat
                    AddResourceBtn.Enabled = true;
                    AddSpellBtn.Enabled = true;
                    AddTargetBtn.Enabled = true;
                    // on permet aux threads d'accéder aux controls du BotForm
                    BotForm.CheckForIllegalCrossThreadCalls = false;

                    // initialisation du Log du bot
                    Log = new Log(LogRichTextBox, DebugRichTextBox, BotName);
                    Log.Debug("Game Window : " + BotName);
                    Log.Debug("Game Panel Resolution : " + GamePanel.Width + "x" + GamePanel.Height);

                    // Adaptation des coordonnées des mouvements au Game Panel
                    Core.Move.FitMovesTo(GamePanel.Width, GamePanel.Height);

                    Log.Debug("Move UP Point : " + Core.Move.UP.X + "x" + Core.Move.UP.Y);
                    Log.Debug("Move RIGHT Point : " + Core.Move.RIGHT.X + "x" + Core.Move.RIGHT.Y);
                    Log.Debug("Move DOWN Point : " + Core.Move.DOWN.X + "x" + Core.Move.DOWN.Y);
                    Log.Debug("Move LEFT Point : " + Core.Move.LEFT.X + "x" + Core.Move.LEFT.Y);

                    // Adaptation des coordonnées des pixels à vérifier de la Map au Game Panel
                    Map.FitPixelsToCheckTo(GamePanel.Width, GamePanel.Height);

                    int i = 0;
                    foreach(Point pixel in Map.PixelsToCheck)
                    {
                        Log.Debug("Map Pixel To Check [" + ++i + "] ( X: " + pixel.X + " Y: " + pixel.Y + ")");
                    }
                }
            }
        }

        // event. FormClosing du formulaire
        private void BotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GameProcess == null || MessageBox.Show("Voulez-vous vraiment quitter le bot ?", App.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (GameProcess != null)
                {
                    // Arret du thread du bot s'il a été démarré
                    if (RunBotBtn.Text != "Démarrer") Bot.Stop();
                    // remise/réaffichage du jeu sur le bureau
                    EmbedWindow.SetParent(GameProcess.MainWindowHandle, IntPtr.Zero); // IntPtr.Zero == NULL (si NULL le parent sera le bureau/desktop)
                    EmbedWindow.MoveWindow(GameProcess.MainWindowHandle, Game.DefaultRect.X, Game.DefaultRect.Y, Game.DefaultRect.Width, Game.DefaultRect.Height, true);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        // event. Click sur le boutton 'RunBotBtn'
        private void RunBotBtn_Click(object sender, EventArgs e)
        {
            switch (RunBotBtn.Text)
            {
                case "Démarrer":
                    if (GamePanel.Controls.Contains(SelectWindowGroupBox))
                    {
                        MessageBox.Show("Veuillez intégrer une fenêtre du jeu !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (TrajetBotComboBox.Items.Count == 0 || TrajetBotComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("Veuillez spécifier un trajet pour le bot !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (FightIAComboBox.Items.Count == 0 || FightIAComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("Veuillez spécifier une IA de combat pour le bot !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (GoBanqueCheckGroupBox.Checked && BanqueTrajetComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("Veuillez ajouter le trajet de la banque dans l'onglet Métier !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        // Trajet
                        Trajet TrajetBot = new Trajet(((ComboBoxItem)TrajetBotComboBox.SelectedItem).Value.ToString());
                        TrajetBot.Repeat = RepeatTrajetBotCheckBox.Checked;
                        // Metier
                        Job Job = new Job((int)CollectTimeNumericUpDown.Value * 1000); // conversion en milliseconde
                        if (Job.GoBanque.Enabled = GoBanqueCheckGroupBox.Checked)
                        {
                            Job.GoBanque.Trajet = new Trajet(((ComboBoxItem)BanqueTrajetComboBox.SelectedItem).Value.ToString());
                        }
                        // Combat
                        Fight Fight = new Fight(new IA(((ComboBoxItem)FightIAComboBox.SelectedItem).Value.ToString()));

                        Bot = new Bot(this, Log, TrajetBot, Job, Fight, BotTimer, BotStopWatch, RunBotBtn, PauseBotBtn, BotStatePictureBox, MinimapPictureBox, GamePanel, GameProcess.MainWindowHandle, PodProgressBar);
                        Bot.Start();
                    }
                    break;
                case "Arrêter":
                    Bot.Stop();
                    break;
            }
        }

        // event. Click sur le boutton 'PauseBotBtn'
        private void PauseBotBtn_Click(object sender, EventArgs e)
        {
            switch (PauseBotBtn.Text)
            {
                case "Pause":
                    Bot.Pause();
                    break;
                case "Reprendre":
                    Bot.Resume();
                    break;
            }
        }

        // event. Tick du 'BotTimer'
        private void BotTimer_Tick(object sender, EventArgs e)
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = BotStopWatch.Elapsed;

            String elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

            BotElapsedTimeLabel.Text = elapsedTime;
        }

        //=========================================================================================================================
        //                                              events du Menu
        //=========================================================================================================================

        // [Menu] event. Click on 'SettingsToolStripMenuItem'
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form fen = new SettingsForm(GamePanel, GameProcess);
            fen.ShowDialog();
        }

        // [Menu] event. Click on 'ExitToolStripMenuItem'
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // [Menu] event. Click on 'LockUnlockGameToolStripMenuItem'
        private void LockUnlockGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GamePanel.Enabled = !LockUnlockGameToolStripMenuItem.Checked;
        }

        // [Menu] event. Click on 'ScreenshotToolStripMenuItem'
        private void ScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = App.CurrentPath;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Screenshot_" + DateTime.Now.ToString("dd-MM-yy_HH-mm-ss");
            saveFileDialog1.Title = "Capturer une image";
            saveFileDialog1.DefaultExt = "bmp";
            saveFileDialog1.Filter = "Image BMP (*.bmp)|*.bmp";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // capture de l'image et sauvegarde dans le fichier spécifié
                Bitmap image = EmbedWindow.GetWindowImage(GameProcess.MainWindowHandle);
                image.Save(saveFileDialog1.FileName);
                MessageBox.Show("Image Capturée !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //=========================================================================================================================
        //                                              events de l'onglet 'Trajet'
        //=========================================================================================================================

        // [Trajet] event. Click sur la PictureBox 'MoveUpPictureBox'
        private void MoveUpPictureBox_Click(object sender, EventArgs e)
        {
            AddTrajetStep("UP::" + HorizentalTrackBar.Value);

            if (SimulateMoveCheckBox.Checked)
                Core.Move.Up(HorizentalTrackBar.Value, GamePanel.Width, GameProcess.MainWindowHandle);
        }

        // [Trajet] event. Click sur la PictureBox 'MoveRightPictureBox'
        private void MoveRightPictureBox_Click(object sender, EventArgs e)
        {
            int VerticalTrackBarValue = Tools.minToMax(VerticalTrackBar.Value, VerticalTrackBar.Minimum, VerticalTrackBar.Maximum);
            AddTrajetStep("RIGHT::" + VerticalTrackBarValue);

            if (SimulateMoveCheckBox.Checked)
                Core.Move.Right(VerticalTrackBarValue, GamePanel.Height, GameProcess.MainWindowHandle);
        }

        // [Trajet] event. Click sur la PictureBox 'MoveDownPictureBox'
        private void MoveDownPictureBox_Click(object sender, EventArgs e)
        {
            AddTrajetStep("DOWN::" + HorizentalTrackBar.Value);

            if (SimulateMoveCheckBox.Checked)
                Core.Move.Down(HorizentalTrackBar.Value, GamePanel.Width, GameProcess.MainWindowHandle);
        }

        // [Trajet] event. Click sur la PictureBox 'MoveLeftPictureBox'
        private void MoveLeftPictureBox_Click(object sender, EventArgs e)
        {
            int VerticalTrackBarValue = Tools.minToMax(VerticalTrackBar.Value, VerticalTrackBar.Minimum, VerticalTrackBar.Maximum);
            AddTrajetStep("LEFT::" + VerticalTrackBarValue);

            if (SimulateMoveCheckBox.Checked)
                Core.Move.Left(VerticalTrackBarValue, GamePanel.Height, GameProcess.MainWindowHandle);
        }

        // [Trajet] event. Click sur le boutton 'WaitBtn'
        private void WaitBtn_Click(object sender, EventArgs e)
        {
            if (TrajetListBox.Items.Count > 0)
            {
                int LastItem = TrajetListBox.Items.Count - 1;
                String LastStep = TrajetListBox.Items[LastItem].ToString();
                if (LastStep.StartsWith("WAIT"))
                {
                    String OldWaitTime = Regex.Split(LastStep, "::")[1];
                    int NewWaitTime = Int32.Parse(OldWaitTime) + Game.TrajetWaitTime;
                    TrajetListBox.Items[LastItem] = "WAIT::" + NewWaitTime;
                    TrajetListBox.TopIndex = TrajetListBox.Items.Count - 1; // scroll to the last item
                    return;
                }
            }

            AddTrajetStep("WAIT::" + Game.TrajetWaitTime);
        }

        // [Trajet] event. Click sur le boutton 'AddAreaBtn'
        private void AddAreaBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"D:\";
            openFileDialog1.Title = "Choisir une zone de récolte";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AddTrajetStep("AREA::" + openFileDialog1.FileName);
            }
        }

        // [Trajet] event. Click sur le boutton 'MouseLeftClickBtn'
        private void MouseLeftClickBtn_Click(object sender, EventArgs e)
        {
            MouseLeftClickBtn.LostFocus += this.GetLeftClickPosition_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void GetLeftClickPosition_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            AddTrajetStep("LEFTCLICK::" + MousePos.X + "x" + MousePos.Y);

            if (SimulateMoveCheckBox.Checked)
                FakeClick.ClickOnPoint(GameProcess.MainWindowHandle, MousePos);

            MouseLeftClickBtn.LostFocus -= this.GetLeftClickPosition_Event;
            this.Cursor = Cursors.Default;
        }

        // [Trajet] event. Click sur le boutton 'MouseRightClickBtn'
        private void MouseRightClickBtn_Click(object sender, EventArgs e)
        {
            MouseRightClickBtn.LostFocus += this.GetRightClickPosition_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void GetRightClickPosition_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);
            AddTrajetStep("RIGHTCLICK::" + MousePos.X + "x" + MousePos.Y);

            if (SimulateMoveCheckBox.Checked)
                FakeClick.RightClickOnPoint(GameProcess.MainWindowHandle, MousePos); // right click

            MouseRightClickBtn.LostFocus -= this.GetRightClickPosition_Event;
            this.Cursor = Cursors.Default;
        }

        // [Trajet] event. Click on button 'Supprimer'
        private void DeleteTrajetBtn_Click(object sender, EventArgs e)
        {
            if (TrajetListBox.SelectedIndex != -1)
                TrajetListBox.Items.RemoveAt(TrajetListBox.SelectedIndex);
        }

        // [Trajet] event. Click on button 'Effacer Tout'
        private void ResetTrajetBtn_Click(object sender, EventArgs e)
        {
            if (TrajetListBox.Items.Count > 0)
                TrajetListBox.Items.Clear();
        }

        // [Trajet] event. Click on button 'Sauvegarder'
        private void SaveTrajetBtn_Click(object sender, EventArgs e)
        {
            if (TrajetListBox.Items.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = App.CurrentPath;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "Trajet_" + DateTime.Now.ToString("dd-MM-yy_HH-mm-ss");
                saveFileDialog1.Title = "Sauvegarder un trajet";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // sauvegarde du trajet dans le fichier spécifié
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    foreach (string s in TrajetListBox.Items)
                        sw.WriteLine(s);
                    sw.Close();
                    MessageBox.Show("Trajet sauvegardé !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Liste de trajet vide !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //=========================================================================================================================
        //                                              events de l'onglet 'Général'
        //=========================================================================================================================

        // [Général] event. Click on button 'AddTrajetBtn'
        private void AddTrajetBtn_Click(object sender, EventArgs e)
        {
            if (AddScript(TrajetBotComboBox))
            {
                Trajet TrajetBot = new Trajet(((ComboBoxItem)TrajetBotComboBox.Items[TrajetBotComboBox.Items.Count - 1]).Value.ToString());

                // vérification de doublon
                foreach (Trajet Trajet in Settings.History.TrajetBot)
                {
                    if (Trajet.Equals(TrajetBot))
                        return;
                }

                Settings.History.TrajetBot.Add(TrajetBot);
                Settings.Save();
            }
        }

        // [Général] event. Click on button 'AddFightIABtn'
        private void AddFightIABtn_Click(object sender, EventArgs e)
        {
            if (AddScript(FightIAComboBox))
            {
                Script FightIA = new Script(((ComboBoxItem)FightIAComboBox.Items[FightIAComboBox.Items.Count - 1]).Value.ToString());

                // vérification de doublon
                foreach (Script Script in Settings.History.FightIA)
                {
                    if (Script.Equals(FightIA))
                        return;
                }

                Settings.History.FightIA.Add(FightIA);
                Settings.Save();
            }
        }

        //=========================================================================================================================
        //                                              events de l'onglet 'Metier'
        //=========================================================================================================================

        // [Métier] event. Click on Button 'AddResourceBtn'
        private void AddResourceBtn_Click(object sender, EventArgs e)
        {
            Form fen = new AddResourceForm(GamePanel, GameProcess, AreaListBox);
            fen.ShowDialog();
        }

        // [Métier] event. Click on Button 'DeleteResourceBtn'
        private void DeleteResourceBtn_Click(object sender, EventArgs e)
        {
            if (AreaListBox.SelectedIndex != -1)
                AreaListBox.Items.RemoveAt(AreaListBox.SelectedIndex);
        }

        // [Métier] event. Click on Button 'UpResourceBtn'
        private void UpResourceBtn_Click(object sender, EventArgs e)
        {
            if (AreaListBox.SelectedIndex > 0) // > 0 => 1 - 1 = 0 (maximum up position)
            {
                object itemToMove = AreaListBox.SelectedItem;
                int indexTo = AreaListBox.SelectedIndex - 1;
                AreaListBox.Items.RemoveAt(AreaListBox.SelectedIndex);
                AreaListBox.Items.Insert(indexTo, itemToMove);
                AreaListBox.SelectedIndex = indexTo;
            }
        }

        // [Métier] event. Click on Button 'DownResourceBtn'
        private void DownResourceBtn_Click(object sender, EventArgs e)
        {
            if (AreaListBox.SelectedIndex != -1 && AreaListBox.SelectedIndex < AreaListBox.Items.Count - 1)
            {
                object itemToMove = AreaListBox.SelectedItem;
                int indexTo = AreaListBox.SelectedIndex + 1;
                AreaListBox.Items.RemoveAt(AreaListBox.SelectedIndex);
                AreaListBox.Items.Insert(indexTo, itemToMove);
                AreaListBox.SelectedIndex = indexTo;
            }
        }

        // [Métier] event. Click on Button 'DeleteAllResourceBtn'
        private void DeleteAllResourceBtn_Click(object sender, EventArgs e)
        {
            if (AreaListBox.Items.Count > 0)
                AreaListBox.Items.Clear();
        }

        // [Métier] event. Click on Button 'SaveAreaBtn'
        private void SaveAreaBtn_Click(object sender, EventArgs e)
        {
            if (AreaListBox.Items.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = App.CurrentPath;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "Zone_" + AreaNameTextBox.Text;
                saveFileDialog1.Title = "Sauvegarder une zone de récolte";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // sauvegarde du trajet dans le fichier spécifié
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    foreach (string s in AreaListBox.Items)
                        sw.WriteLine(s);
                    sw.Close();
                    MessageBox.Show("Zone de récolte sauvegardée !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Liste de ressource vide !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // [Métier] event. Click on Button 'AddBanqueTrajetBtn'
        private void AddBanqueTrajetBtn_Click(object sender, EventArgs e)
        {
            AddScript(BanqueTrajetComboBox);
        }

        //=========================================================================================================================
        //                                              events de l'onglet 'Combat'
        //=========================================================================================================================

        // [Combat] event. Click on Button 'AddSpellBtn'
        private void AddSpellBtn_Click(object sender, EventArgs e)
        {
            Form fen = new AddSpellForm(GamePanel, GameProcess, FightIAListBox);
            fen.ShowDialog();
        }

        // [Combat] event. Click on Button 'AddTargetBtn'
        private void AddTargetBtn_Click(object sender, EventArgs e)
        {
            AddTargetBtn.LostFocus += this.GetTargetPos_Event;
            this.Cursor = Cursors.No;
        }

        // event. (ajouté dynamiquement)
        private void GetTargetPos_Event(object sender, EventArgs e)
        {
            Point MousePos = GamePanel.PointToClient(Cursor.Position);

            FightIAListBox.Items.Add("TARGET::" + MousePos.X + "x" + MousePos.Y);
            FightIAListBox.TopIndex = FightIAListBox.Items.Count - 1; // scroll to the last item

            AddTargetBtn.LostFocus -= this.GetTargetPos_Event;
            this.Cursor = Cursors.Default;
        }

        // [Combat] event. Click on Button 'CopyFightIABtn'
        private void CopyFightIABtn_Click(object sender, EventArgs e)
        {
            if (FightIAListBox.SelectedIndex != -1)
            {
                FightIAListBox.Items.Add(FightIAListBox.SelectedItem);
                FightIAListBox.TopIndex = FightIAListBox.Items.Count - 1; // scroll to the last item
            }
        }

        // [Combat] event. Click on Button 'DeleteFightIABtn'
        private void DeleteFightIABtn_Click(object sender, EventArgs e)
        {
            if (FightIAListBox.SelectedIndex != -1)
                FightIAListBox.Items.RemoveAt(FightIAListBox.SelectedIndex);
        }

        // [Combat] event. Click on Button 'DeleteAllFightIABtn'
        private void DeleteAllFightIABtn_Click(object sender, EventArgs e)
        {
            if (FightIAListBox.Items.Count > 0)
                FightIAListBox.Items.Clear();
        }

        // [Combat] event. Click on Button 'SaveFightIABtn'
        private void SaveFightIABtn_Click(object sender, EventArgs e)
        {
            if (FightIAListBox.Items.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = App.CurrentPath;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "IA_combat";
                saveFileDialog1.Title = "Sauvegarder une IA de combat";
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // sauvegarde du trajet dans le fichier spécifié
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                    foreach (string s in FightIAListBox.Items)
                        sw.WriteLine(s);
                    sw.Close();
                    MessageBox.Show("IA sauvegardée !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Liste vide !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // [Combat] event. Click on Button 'FightIAWaitBtn'
        private void FightIAWaitBtn_Click(object sender, EventArgs e)
        {
            if (FightIAListBox.Items.Count > 0)
            {
                int LastItem = FightIAListBox.Items.Count - 1;
                String LastStep = FightIAListBox.Items[LastItem].ToString();
                if (LastStep.StartsWith("WAIT"))
                {
                    String OldWaitTime = Regex.Split(LastStep, "::")[1];
                    int NewWaitTime = Int32.Parse(OldWaitTime) + Game.FightWaitTime;
                    FightIAListBox.Items[LastItem] = "WAIT::" + NewWaitTime;
                    FightIAListBox.TopIndex = TrajetListBox.Items.Count - 1; // scroll to the last item
                    return;
                }
            }

            FightIAListBox.Items.Add("WAIT::" + Game.FightWaitTime);
            FightIAListBox.TopIndex = FightIAListBox.Items.Count - 1; // scroll to the last item
        }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // AddScript(...) : ajoute le fichier/script séléctionné à la liste spécifiée
        private bool AddScript(ComboBox TrajetComboBox)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = @"D:\";
            openFileDialog1.Title = "Choisir un fichier/script";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Trajet Trajet = new Trajet(openFileDialog1.FileName);

                if (TrajetComboBox.FindStringExact(Trajet.Name) == -1)
                {
                    // on ajoute le fichier qui contient le trajet à la combobox
                    TrajetComboBox.Items.Add(new ComboBoxItem(Trajet.Name, Trajet.File));
                    TrajetComboBox.SelectedIndex = TrajetComboBox.Items.Count - 1; // on le séléctionne
                    return true;
                }
                else
                {
                    MessageBox.Show("Fichier existant !", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            return false;
        }

        // AddTrajetStep(...) : ajoute une nouvelle entrée à la liste de trajet
        private void AddTrajetStep(String entry)
        {
            TrajetListBox.Items.Add(entry);
            TrajetListBox.TopIndex = TrajetListBox.Items.Count - 1; // scroll to the last item
        }
    }
}
