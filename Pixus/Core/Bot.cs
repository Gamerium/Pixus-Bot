using System;
using System.Collections.Generic;
using System.Text;
using System.Threading; // pour la Class ManualResetEvent
using System.Windows.Forms; // pour la Class TextBox
using System.Drawing; // pour la Class Point
using System.IO; // pour la Class File
using System.Text.RegularExpressions; // pour la Class Regex
using System.Diagnostics; // pour la Class Stopwatch
using Pixus.Lib;
using Pixus.Core.JobStuffs;
using Pixus.Core.FightStuffs;

namespace Pixus.Core
{
    class Bot
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        private BotForm ParentForm;
        private ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        private ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        private Thread _thread;
        private Log Log;
        private Trajet Trajet;
        private Job Job;
        private Fight Fight;
        private Button RunBotBtn, PauseBotBtn;
        private PictureBox BotStatePictureBox;
        private PictureBox MinimapPictureBox;
        private Panel GamePanel;
        private IntPtr GameHandle;
        private Pod Pod = new Pod();
        private CustomProgressBar PodProgressBar;
        private System.Windows.Forms.Timer Timer;
        private Stopwatch StopWatch;
        private long LastElapsedTime = 0;

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public Bot(BotForm parentForm, Log log, Trajet trajet, Job job, Fight fight, 
            System.Windows.Forms.Timer timer, Stopwatch stopWatch,
            Button runBotBtn, Button pauseBotBtn, PictureBox botStatePictureBox, PictureBox minimapPictureBox, Panel gamePanel, IntPtr gameHandle, CustomProgressBar podProgressBar)
        {
            this.ParentForm = parentForm;
            this.Log = log;
            this.Trajet = trajet;
            this.Job = job;
            this.Fight = fight;
            this.Timer = timer;
            this.StopWatch = stopWatch;
            this.RunBotBtn = runBotBtn;
            this.PauseBotBtn = pauseBotBtn;
            this.BotStatePictureBox = botStatePictureBox;
            this.MinimapPictureBox = minimapPictureBox;
            this.GamePanel = gamePanel;
            this.GameHandle = gameHandle;
            this.PodProgressBar = podProgressBar;
        }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // Start(...) : crée le thread du bot
        public void Start()
        {
            _thread = new Thread(Run);
            _thread.Start();

            StartTimer();

            RunBotBtn.Text = "Arrêter";
            RunBotBtn.Image = Pixus.Properties.Resources.bot_stop;
            PauseBotBtn.Enabled = true;
            BotStatePictureBox.Image = Pixus.Properties.Resources.load;
        }

        // Pause(...) : met le bot en pause
        public void Pause()
        {
            Log.Debug("Pause Bot");

            _pauseEvent.Reset();

            PauseTimer();

            PauseBotBtn.Text = "Reprendre";
            PauseBotBtn.Image = Pixus.Properties.Resources.bot_play_resume;
            //RunBotBtn.Enabled = false;
            BotStatePictureBox.Image = Pixus.Properties.Resources.pause;
        }

        // Resume(...) : sort le bot de la pause/reprend le bot
        public void Resume()
        {
            Log.Debug("Resume Bot");

            _pauseEvent.Set();

            StartTimer();

            PauseBotBtn.Text = "Pause";
            PauseBotBtn.Image = Pixus.Properties.Resources.bot_pause;
            //RunBotBtn.Enabled = true;
            BotStatePictureBox.Image = Fight.OnGoing ? Pixus.Properties.Resources.warrior : Pixus.Properties.Resources.load;
        }

        // Break(...) : Suspend les activités du bot
        private void Break()
        {
            // Signal the shutdown event
            _shutdownEvent.Set();

            // Make sure to resume any paused threads
            _pauseEvent.Set();
        }

        // Suspend(...) : Suspend les activités du bot et restaure les controls à l'état initial
        private void Suspend()
        {
            Log.Debug("Suspend Bot");

            Break();
            ResetControls();
        }

        // Stop(...) : arrête le bot
        public void Stop()
        {
            Log.Debug("Stop Bot");

            Break();

            // Wait for the thread to exit
            _thread.Join();

            ResetControls();
        }

        // ResetControls() : rénitialise les controls (bouttons...) du bot
        private void ResetControls()
        {
            StopTimer();

            RunBotBtn.Text = "Démarrer";
            RunBotBtn.Image = Pixus.Properties.Resources.bot_play_resume;
            PauseBotBtn.Enabled = false;
            if (PauseBotBtn.Text == "Reprendre")
            {
                PauseBotBtn.Text = "Pause";
                PauseBotBtn.Image = Pixus.Properties.Resources.bot_pause;
            }
            BotStatePictureBox.Image = null;
        }

        #region TimerFunctions
        // StartTimer() : démarre le timer du bot
        private void StartTimer()
        {
            StopWatch.Start();
            Timer.Start();
        }

        // PauseTimer() : met en pause le timer du bot
        private void PauseTimer()
        {
            StopWatch.Stop();
            Timer.Stop();
        }

        // StopTimer() : arrête le timer du bot
        private void StopTimer()
        {
            StopWatch.Stop();
            StopWatch.Reset();
            Timer.Stop();
        }

        // GetElapsedTime() : récupère le temp passé depuis la dernière fois qu'on a appelé cette fonction
        private long GetElapsedTime()
        {
            long ElapsedTime = StopWatch.ElapsedMilliseconds - LastElapsedTime;

            LastElapsedTime = StopWatch.ElapsedMilliseconds;

            return ElapsedTime;
        }
        #endregion

        //=========================================================================================================================
        //                                                  méthode/Thread Run()
        //=========================================================================================================================

        // Run(...) : démarre le bot
        private void Run()
        {
            Log.Debug("Run Bot");

            // Affichage du trajet et de l'IA utilisés
            Log.Entry("Trajet : " + Trajet.Name);
            Log.Debug("Trajet File " + Trajet.File + " Repeat: " + Tools.boolToString(Trajet.Repeat));
            Log.Debug("IA File " + Fight.IA.File);

            do
            {
                _pauseEvent.WaitOne();
                if (_shutdownEvent.WaitOne(0))
                    break;

                // Lecture du fichier du trajet
                string[] steps = File.ReadAllLines(Trajet.File);

                // Parcours des étapes du trajet
                foreach(string step in steps)
                {
                    _pauseEvent.WaitOne();
                    if (_shutdownEvent.WaitOne(0))
                        break;

                    // on s'assure que toutes les fenêtres popup sont fermés
                    CheckPopUps();

                    // Step
                    DoStep(step);

                    // on rénitialise la Minimap après le changement de Map (si jamais un repère n'a pas été enlevé ou bien le bot a été arrêté)
                    Log.Debug("Cleaning Minimap");
                    Minimap.Clear(MinimapPictureBox);
                }

                // si le bot n'a pas été arrêté
                if (!_shutdownEvent.WaitOne(0))
                {
                    Log.Entry("Nombre approximatif de ressources collectées : " + Job.CollectedResourceCount);
                }

            } while(Trajet.Repeat);

            // si le bot n'a pas été arrêté
            if (!_shutdownEvent.WaitOne(0))
            {
                TimeSpan ts = StopWatch.Elapsed;
                String elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                Log.Entry("Temps écoulé : " + elapsedTime);
                
                Log.Success("Trajet complété !");
                ResetControls();
            }
        }

        // Wait(...) : attend un temp demandé tout en respectant les pauses ou arrêt
        private void Wait(int timeout = 1000)
        {
            int waitedTime = 0;

            while (waitedTime < timeout)
            {
                _pauseEvent.WaitOne();
                if (_shutdownEvent.WaitOne(0))
                    break;

                Thread.Sleep(1000);
                waitedTime += 1000;
            }
        }

        //=========================================================================================================================
        //                                                  Trajet méthodes
        //=========================================================================================================================

        #region Trajet
        // DoStep(...) : effectue l'étape spécifiée du trajet
        private void DoStep(String step)
        {
            // on s'assure que toutes les fenêtres popup sont fermés (après une action : métier/combat..)
            CheckPopUps();

            // on vérifie si un combat est en cours
            CheckFight();

            // si le bot n'a pas été arrêté
            if (!_shutdownEvent.WaitOne(0))
            {
                // Mouvement
                Log.Debug("Step " + step);

                try
                {
                    // découpage des coordonnées
                    string[] stepXY = Regex.Split(step, "::");

                    // exécution du trajet
                    bool shouldWaitForMapToChange = true;
                    switch (stepXY[0])
                    {
                        case "UP":
                            Move.Up(Int32.Parse(stepXY[1]), GamePanel.Width, GameHandle);
                            break;
                        case "DOWN":
                            Move.Down(Int32.Parse(stepXY[1]), GamePanel.Width, GameHandle);
                            break;
                        case "LEFT":
                            Move.Left(Int32.Parse(stepXY[1]), GamePanel.Height, GameHandle);
                            break;
                        case "RIGHT":
                            Move.Right(Int32.Parse(stepXY[1]), GamePanel.Height, GameHandle);
                            break;
                        case "LEFTCLICK":
                            ClickOn(stepXY[1]);
                            Wait();
                            shouldWaitForMapToChange = false;
                            break;
                        case "RIGHTCLICK":
                            ClickOn(stepXY[1], true);
                            Wait();
                            shouldWaitForMapToChange = false;
                            break;
                        case "WAIT":
                            Wait(Int32.Parse(stepXY[1]));
                            shouldWaitForMapToChange = false;
                            break;
                        case "AREA":
                            DoJob(stepXY[1]);
                            GetPod();
                            GoBanque();
                            shouldWaitForMapToChange = false;
                            break;
                        default:
                            Log.Error("Erreur sur le trajet.");
                            this.Suspend();
                            shouldWaitForMapToChange = false;
                            break;
                    }

                    // on attend le changement de la Map
                    if (shouldWaitForMapToChange) WaitForMapToChange();
                }
                catch (Exception ex)
                {
                    Log.Debug("Execption occured : " + ex.Message);
                }
            }
        }

        // CheckPopUps(...) : vérifie et ferme les fenêtres popup
        private void CheckPopUps()
        {
            // on s'assure que toutes les fenêtres popup sont fermés
            while(Game.ClosePopUps(GameHandle, Log))
            {
                _pauseEvent.WaitOne();
                if (_shutdownEvent.WaitOne(0))
                    break;

                Wait();
            }
        }

        // WaitForMapToChange() : attend le changement de la Map
        private void WaitForMapToChange()
        {
            Log.Title(Log.Level.Debug, "WaitForMapToChange");

            // on vérifie que si la Map a changée ou pas, en respectant un temp maximum de vérification
            int checkTime = 0;
            List<Color> PixelsToCheckColor = Map.GetPixelsToCheckColor(GameHandle); // couleur actuelle des pixels à vérifier

            // informations de débogage
            int i = 0;
            foreach (Point pixel in Map.PixelsToCheck)
            {
                Log.Debug("Map Pixel (X: " + pixel.X + " Y: " + pixel.Y + ") Color (R: " + PixelsToCheckColor[i].R + " G: " + PixelsToCheckColor[i].G + " B: " + PixelsToCheckColor[i].B + ")");
                i++;
            }
            Log.Divider(Log.Level.Debug);

            while (checkTime < Map.MaxLoadTimeout)
            {
                _pauseEvent.WaitOne();
                if (_shutdownEvent.WaitOne(0))
                    break;

                // si la Map a changée
                bool mapChanged = Settings.MapChangeCheckPrecision == 0 ? Pixel.ColorChanged(Map.PixelsToCheck, PixelsToCheckColor, GameHandle) : Pixel.AllColorChanged(Map.PixelsToCheck, PixelsToCheckColor, GameHandle);
                if (mapChanged)
                {
                    Wait(Settings.MapLoadTimeout);
                    return;
                }

                Thread.Sleep(1000);
                checkTime += 1000;
            } // fin while {}

            if (!_shutdownEvent.WaitOne(0)) // si le bot n'a pas été arrêté
            {
                // si on arrive içi c'est qu'il y'a eu un problème (Timeout dépassé, la map n'a pas changée, on est bloqué, beug, ...)
                if (Connection.State())
                {
                    Log.Error("La map ne change pas, veuillez vérifier/modifier le trajet.");
                }
                else // connexion perdue
                    Log.Error("Connexion perdue");

                this.Suspend();
            }
        }

        // ClickOn(...) : effectue un click sur la zone/position spécifiée
        private void ClickOn(String ClickPosition, bool RightClick = false)
        {
            try
            {
                // découpage des coordonnées de la position de click
                string[] clickXY = Regex.Split(ClickPosition, "x");
                Point ClickPoint = new Point(Int32.Parse(clickXY[0]), Int32.Parse(clickXY[1]));
                if (RightClick)
                    FakeClick.RightClickOnPoint(GameHandle, ClickPoint);
                else
                    FakeClick.ClickOnPoint(GameHandle, ClickPoint);
            }
            catch(Exception ex)
            {
                Log.Debug(ex.Message);
            }
        }
        #endregion

        //=========================================================================================================================
        //                                                  Job méthodes
        //=========================================================================================================================

        #region Job
        // DoJob(...) : gère le métier du bot
        private void DoJob(String AreaFile)
        {
            Log.Title(Log.Level.Debug, "DoJob");
            //Log.Debug("AreaFile Path : " + AreaFile);
            
            try
            {
                // Lecture du fichier des ressources
                string[] resources = File.ReadAllLines(AreaFile);

                // Parcours des emplacements des ressources
                List<Point> resourceLocations = new List<Point>();
                foreach (string resource in resources)
                {
                    _pauseEvent.WaitOne();
                    if (_shutdownEvent.WaitOne(0))
                        break;

                    Log.Debug("Resource : " + resource);

                    // découpage des données de la ressource
                    string[] resourceData = Regex.Split(resource, "::");
                    string[] resourceXY = Regex.Split(resourceData[1], "x");
                    string[] resourceRGB = Regex.Split(resourceData[2], ",");

                    int resourceX = Int32.Parse(resourceXY[0]);
                    int resourceY = Int32.Parse(resourceXY[1]);
                    Point resourceLocation = new Point(resourceX, resourceY);

                    Color ResourceColor = Color.FromArgb(Int32.Parse(resourceRGB[0]), Int32.Parse(resourceRGB[1]), Int32.Parse(resourceRGB[2]));
                    bool checkResourceColor = Tools.stringToBool(resourceData[3]);

                    // s'il faut vérifier la couleur de la ressource + le pixel n'a pas la bonne couleur, on saute cette ressource alors
                    if (checkResourceColor && !Pixel.HasColor(resourceLocation, ResourceColor, GameHandle))
                        continue;
                    
                    resourceLocations.Add(resourceLocation);
                }

                // si ressource(s) trouvée(s)
                if (resourceLocations.Count > 0)
                {
                    // affichage des positions des ressources sur la Minimap
                    foreach (Point loc in resourceLocations)
                    {
                        Minimap.NewPin(MinimapPictureBox, GamePanel.Size, loc, Minimap.ResourcePinColor);
                    }

                    // Au boulot !
                    foreach (Point location in resourceLocations)
                    {
                        _pauseEvent.WaitOne();
                        if (_shutdownEvent.WaitOne(0))
                            break;

                        // vérification de la connexion
                        if (!Connection.State())
                        {
                            Log.Error("Connexion perdue");
                            this.Suspend();
                            break;
                        }

                        // on s'assure que toutes les fenêtres popup sont fermés
                        CheckPopUps();

                        // on collecte/récolte la ressource si possible
                        if (location.X > 0 && location.Y > 0)
                        {
                            Log.Debug("Collecting Resource (X: " + location.X + " Y: " + location.Y + ")");

                            FakeClick.ClickOnPoint(GameHandle, location);
                            Wait(Job.MaxCollectTime);
                            Minimap.RemovePin(MinimapPictureBox, GamePanel.Size, location);
                            Job.CollectedResourceCount++;
                        }

                        // on vérifie si un combat est en cours
                        CheckFight();
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Debug("Execption occured : " + ex.Message);
            }
            
            if (!_shutdownEvent.WaitOne(0)) // si le bot n'a pas été arrêté
            {
                Log.Debug("CollectedResourceCount : " + Job.CollectedResourceCount);

                Log.Divider(Log.Level.Debug);
            }
        }
        
        // GetPod() : récupère le pod en pourcentage et fait avancé la progressbar
        public void GetPod()
        {
            Log.Title(Log.Level.Debug, "GetPod");

            try
            {
                // Click sur l'inventaire pour l'ouvrir
                FakeClick.ClickOnPoint(GameHandle, Settings.InventoryPosition);
                Wait();

                // Nouvelle Image de la Map
                Bitmap MapImage = EmbedWindow.GetWindowImage(GameHandle);

                // MapBitmap
                LockBitmap MapBitmap = new LockBitmap(MapImage);
                MapBitmap.LockBits();

                Color? PodBarColor = null;
                int PodValue = 0;

                // Get PodValue
                for (int x = Pod.PodBarX; x < Pod.PodBarEndX; x++)
                {
                    _pauseEvent.WaitOne();
                    if (_shutdownEvent.WaitOne(0))
                        break;

                    if (PodBarColor == null) // on récupère la couleur dominente
                    {
                        PodBarColor = MapBitmap.GetPixel(x, Pod.PodBarY);
                    }

                    // si la couleur est toujours dominente on continue
                    if (MapBitmap.GetPixel(x, Pod.PodBarY) == PodBarColor)
                    {
                        PodValue++;
                    }
                    else // si nn on s'arrete
                    {
                        break;
                    }
                }

                MapBitmap.UnlockBits();

                if (!_shutdownEvent.WaitOne(0)) // si le bot n'a pas été arrêté
                {
                    // Set Pod PercentValue (%)
                    this.Pod.PercentValue = (int)((float)((float)PodValue / Pod.PodBarWidth) * 100);
                    Log.Debug("Pod PodValue: " + PodValue + " PercentValue: " + this.Pod.PercentValue + "%");

                    // Set Progressbar FillProcent
                    PodProgressBar.BarFillProcent = this.Pod.PercentValue;

                    // ReClick sur l'inventaire pour le fermer
                    FakeClick.ClickOnPoint(GameHandle, Settings.InventoryPosition);
                    Wait();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Execption occured : " + ex.Message);
            }
            finally
            {
                Log.Divider(Log.Level.Debug);
            }
        }

        // GoBanque() : utilise le trajet de la banque pour s'y rendre
        public void GoBanque()
        {
            Log.Title(Log.Level.Debug, "GoBanque");

            if (this.Job.GoBanque.Enabled)
            {
                Log.Debug("Bot Pod: " + this.Pod.PercentValue + "%");

                // si bot full pod
                if (this.Pod.isFull())
                {
                    // Lecture du fichier du trajet
                    string[] steps = File.ReadAllLines(Job.GoBanque.Trajet.File);

                    // Parcours des étapes du trajet
                    foreach (string step in steps)
                    {
                        _pauseEvent.WaitOne();
                        if (_shutdownEvent.WaitOne(0))
                            break;

                        // Step
                        DoStep(step);
                    }
                }
            }
            else
            {
                Log.Debug("GoBanque est désactivé.");
                // si bot full pod + GoBanque désactivé
                if (this.Pod.isFull())
                {
                    Log.Error("Le bot est full pod.");
                    this.Suspend();
                }
            }

            Log.Divider(Log.Level.Debug);
        }
        #endregion

        //=========================================================================================================================
        //                                                  Fight méthodes
        //=========================================================================================================================

        #region Fight
        // GoFight(...) : gère le(s) combat(s) du bot
        private void GoFight()
        {
            Log.Title(Log.Level.Debug, "GoFight");

            Log.Entry("Combat en cours...");
            Fight.OnGoing = true;

            try
            {
                // on ferme le combat (si ouvert)
                CloseFight();

                // on désactive le mode spectateur (si activé)
                DisableSpectatorMode();

                // on clic sur Prêt
                Log.Debug("Start Fight");
                FakeClick.ClickOnPoint(GameHandle, Settings.StartPassTurnPosition);
                Thread.Sleep(100); //Wait();

                // on clic ailleur pour que la détection de fin de combat fonctionne correctement
                FakeClick.ClickOnPoint(GameHandle, new Point(10, 10));
                Wait(Fight.WaitTimeAfterStartFight);

                // initialisation
                Fight.Turn = 0;

                // boucle de combat (tant que le combat n'est pas terminé)
                while (!CheckFightEnd())
                {
                    _pauseEvent.WaitOne();
                    if (_shutdownEvent.WaitOne(0))
                        break;

                    // on attend le début du tour du bot
                    if (!_shutdownEvent.WaitOne(0) && WaitForBotTurn()) // si le bot n'a pas été arrêté + c'est le tour du bot
                    {
                        // si le bot n'a pas été arrêté
                        if (!_shutdownEvent.WaitOne(0))
                        {
                            // on abandonne le combat s'il se prolonge beaucoup
                            if (Fight.Turn >= Settings.ExitFightTurn)
                            {
                                FakeClick.ClickOnPoint(GameHandle, Settings.ExitFightPosition);
                                Wait();
                                Log.Entry("Combat abandonné.");
                                break;
                            }

                            // Lecture du fichier d'IA
                            string[] actions = File.ReadAllLines(Fight.IA.File);

                            // Parcours des actions de l'IA
                            foreach (string action in actions)
                            {
                                _pauseEvent.WaitOne();
                                if (_shutdownEvent.WaitOne(0))
                                    break;

                                // on vérifie si le combat est terminé, si oui on sort de la boucle d'IA
                                if (CheckFightEnd()) break;

                                // on fait appel à l'IA de combat du bot
                                FightIA(action);
                            }

                            // si le bot n'a pas été arrêté
                            if (!_shutdownEvent.WaitOne(0))
                            {
                                // on vérifie si le combat est terminé, si oui on sort de la boucle while
                                if (CheckFightEnd()) break;

                                // on passe le tour
                                Log.Debug("Pass Turn");
                                FakeClick.ClickOnPoint(GameHandle, Settings.StartPassTurnPosition);
                                Thread.Sleep(100); //Wait();

                                // on clic ailleur pour que la détection de fin de combat fonctionne correctement
                                FakeClick.ClickOnPoint(GameHandle, new Point(10, 10));
                                Wait(Fight.WaitTimeAfterPassTurn);
                            }
                        }
                        else
                            break;
                    }
                }

                // si le bot n'a pas été arrêté
                if (!_shutdownEvent.WaitOne(0))
                {
                    // on ferme la fenêtre de fin de combat/level up
                    CheckPopUps();

                    Log.Entry("Combat terminé. [durée: " + Fight.Turn + " tour(s)]");
                    Fight.OnGoing = false;

                    BotStatePictureBox.Image = Pixus.Properties.Resources.load;

                    // si combat abandonné, on arrête le bot (pour le moment)
                    if (Fight.Turn >= Settings.ExitFightTurn) this.Suspend();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Execption occured : " + ex.Message);
            }
            
            Log.Divider(Log.Level.Debug);
        }

        // FightIA(...) : IA de combat du bot
        private void FightIA(String action)
        {
            // si le bot n'a pas été arrêté
            if (!_shutdownEvent.WaitOne(0))
            {
                // action IA
                Log.Debug("FightIA action : " + action);

                try
                {
                    // découpage des données
                    string[] actionSplited = Regex.Split(action, "::");

                    // si l'action attendu par l'IA n'est pas la bonne on saute/ignore l'action en cours
                    if (Fight.IA.NextAction.Count > 0 && !Fight.IA.NextAction.Contains(actionSplited[0]))
                    {
                        Log.Debug("FightIA NextAction : " + Fight.IA.NextActionToString());
                        Log.Debug("FightIA action Ignored.");
                        return;
                    }

                    // initialisation/rénitialisation
                    Fight.IA.NextAction.Clear();

                    // exécution de l'action
                    switch (actionSplited[0])
                    {
                        case "SPELL":
                            int spellRelaunchTurn = Int32.Parse(actionSplited[3]);
                            int spellLaunchTurn = Int32.Parse(actionSplited[4]);
                            // si c'est le bon tour pour lancer le sort
                            if (Fight.Turn >= spellLaunchTurn && (Fight.Turn == 1 || Fight.Turn % spellRelaunchTurn == 0))
                            {
                                // on lance le sort
                                string[] spellPosition = Regex.Split(actionSplited[2], "x");
                                FakeClick.ClickOnPoint(GameHandle, new Point(Int32.Parse(spellPosition[0]), Int32.Parse(spellPosition[1])));
                                Fight.IA.Next(new string[] { "TARGET" }); // la prochaine action doit être une cible
                            }
                            else
                            {
                                Fight.IA.Next(new string[] { "SPELL", "WAIT" }); // la prochaine action doit être un sort ou bien un WAIT
                            }
                            break;
                        case "TARGET":
                            string[] targetPosition = Regex.Split(actionSplited[1], "x");
                            FakeClick.ClickOnPoint(GameHandle, new Point(Int32.Parse(targetPosition[0]), Int32.Parse(targetPosition[1])));
                            break;
                        case "WAIT":
                            Wait(Int32.Parse(actionSplited[1]));
                            break;
                        default:
                            Log.Error("Erreur sur l'IA de combat.");
                            this.Suspend();
                            break;
                    }

                    // on attend une seconde
                    Wait(IA.ActionWaitTimeout);

                    Log.Debug("FightIA action Done.");
                }
                catch (Exception ex)
                {
                    Log.Debug("Execption occured : " + ex.Message);
                    Log.Error("Erreur sur l'IA de combat.");
                    this.Suspend();
                }
            }
        }

        // WaitForBotTurn() : attend le tour du bot
        private bool WaitForBotTurn()
        {
            Log.Title(Log.Level.Debug, "WaitForBotTurn");
            Log.Debug("Turn Detection " + Tools.pointToString(Settings.TurnDetection.Position) + " " + Tools.colorToString(Settings.TurnDetection.Color));

            // on vérifie si le c'est le tour du bot ou pas encore
            int checkTime = 0;

            while (checkTime < Fight.TurnTimeout)
            {
                _pauseEvent.WaitOne();
                if (_shutdownEvent.WaitOne(0))
                    break;

                if (CheckFightEnd()) return false;

                // si c'est le tour du bot
                if (Pixel.HasColor(Settings.TurnDetection.Position, Settings.TurnDetection.Color, GameHandle))
                {
                    Fight.Turn++;
                    Log.Debug("Bot Turn [" + Fight.Turn + "]");
                    return true;
                }

                Thread.Sleep(1000);
                checkTime += 1000;
            } // fin while {}

            if (!_shutdownEvent.WaitOne(0)) // si le bot n'a pas été arrêté
            {
                // si on arrive içi c'est qu'il y'a eu un problème (Timeout dépassé, beug, ...)
                if (Connection.State())
                {
                    Log.Error("Le tour du bot n'a pas commencé.");
                }
                else // connexion perdue
                    Log.Error("Connexion perdue");

                this.Suspend();
            }

            return false;
        }

        // CheckFight() : vérifie si un combat est en cours
        private void CheckFight()
        {
            Log.Title(Log.Level.Debug, "CheckFight");
            Log.Debug("Fight Detection " + Tools.pointToString(Settings.FightDetection.Position) + " " + Tools.colorToString(Settings.FightDetection.Color));

            if (Pixel.HasColor(Settings.FightDetection.Position, Settings.FightDetection.Color, GameHandle))
            {
                Log.Debug("Fight Detected.");
                BotStatePictureBox.Image = Pixus.Properties.Resources.warrior;
                GoFight();
            }
            else
            {
                Log.Debug("Fight Not Detected.");
            }
        }

        // CheckFightEnd() : vérifie si le combat en cours est terminé ou pas encore
        private bool CheckFightEnd()
        {
            Log.Title(Log.Level.Debug, "CheckFightEnd");
            Log.Debug("Fight Detection " + Tools.pointToString(Settings.FightDetection.Position) + " " + Tools.colorToString(Settings.FightDetection.Color));

            return !Pixel.HasColor(Settings.FightDetection.Position, Settings.FightDetection.Color, GameHandle);
        }

        // DisableSpectatorMode() : désactive le mode spectateur
        private void DisableSpectatorMode()
        {
            if (Settings.DisableSpectatorMode)
            {
                Log.Title(Log.Level.Debug, "DisableSpectatorMode");
                Log.Debug("DisableSpectatorMode Pixel " + Tools.pointToString(Settings.DisableSpectatorModePixel.Position) + " " + Tools.colorToString(Settings.DisableSpectatorModePixel.Color));

                if (Pixel.HasColor(Settings.DisableSpectatorModePixel.Position, Settings.DisableSpectatorModePixel.Color, GameHandle))
                {
                    FakeClick.ClickOnPoint(GameHandle, Settings.DisableSpectatorModePixel.Position);
                    Wait(); // on attend une seconde
                    Log.Debug("SpectatorMode Disabled.");
                }
                else
                {
                    Log.Debug("SpectatorMode Already Disabled.");
                }
            }
        }

        // CloseFight() : ferme le combat
        private void CloseFight()
        {
            if (Settings.CloseFight)
            {
                Log.Title(Log.Level.Debug, "CloseFight");
                Log.Debug("CloseFight Pixel " + Tools.pointToString(Settings.CloseFightPixel.Position) + " " + Tools.colorToString(Settings.CloseFightPixel.Color));

                if (Pixel.HasColor(Settings.CloseFightPixel.Position, Settings.CloseFightPixel.Color, GameHandle))
                {
                    FakeClick.ClickOnPoint(GameHandle, Settings.CloseFightPixel.Position);
                    Wait(); // on attend une seconde
                    Log.Debug("Fight Closed.");
                }
                else
                {
                    Log.Debug("Fight Already Closed.");
                }
            }
        }
        #endregion
    }
}
