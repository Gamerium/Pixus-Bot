using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Pixus.Core
{
    class Log
    {
        //=========================================================================================================================
        //                                                      attributs
        //=========================================================================================================================
        public enum Level
        {
            Normal,
            Success,
            Debug,
            Error
        }

        private const String LogDir = "Log";
        private RichTextBox LogTextBox;
        private RichTextBox DebugTextBox;
        private const String DebugPrefix = "DEBUG::";
        private const String DebugSuffix = "";
        private const long MaxTextBoxLength = 10240;
        private const long MaxLogSize = 502400;

        //=========================================================================================================================
        //                                                      propriétés
        //=========================================================================================================================
        public String FilePath { get; set; }

        //=========================================================================================================================
        //                                                      constr.
        //=========================================================================================================================
        public Log(RichTextBox LogTextBox, RichTextBox DebugTextBox, String LogName)
        {
            this.LogTextBox = LogTextBox;
            this.DebugTextBox = DebugTextBox;
            FilePath = Path.Combine(Game.AppPath, LogDir);
            FilePath = Path.Combine(FilePath, LogName + "_" + DateTime.Now.ToString("dd-MM-yy") + ".log");
        }

        //=========================================================================================================================
        //                                                      méthodes
        //=========================================================================================================================

        // Write(...) : Ecrie dans le fichier log
        private void Write(String text)
        {
            String logDir = Path.GetDirectoryName(FilePath);

            if (logDir != null && !Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            // Delete the log file if it excedes the max log size
            var logFile = new FileInfo(FilePath);
            if (logFile.Exists && logFile.Length > MaxLogSize)
                logFile.Delete();

            // Write text to log file
            StreamWriter logWriter = new StreamWriter(FilePath, true);
            logWriter.Write(text);
            logWriter.Close();
        }

        // WriteLine(...) : Ecrie une ligne dans le fichier log
        private void WriteLine(String text)
        {
            Write(text + "\r\n");
        }

        // AddLogTime(...) : Ajoute la date et heure au texte passé en paramétre
        private string AddLogTime(String text)
        {
            return "[" + DateTime.Now.ToLongTimeString() + "] " + text;
        }

        // Entry(...) : Enregitre une entrée dans le log
        private void Entry(Level level, String text, bool noTime = false)
        {
            if (level != Level.Debug || Settings.EnableDebug)
            {
                RichTextBox textBox = level.Equals(Level.Debug) ? DebugTextBox : LogTextBox;
                Color textColor = Color.Black;

                switch(level)
                {
                    case Level.Normal:
                        break;
                    case Level.Success:
                        textColor = Color.Green;
                        break;
                    case Level.Error:
                        textColor = Color.Red;
                        break;
                    case Level.Debug:
                        break;
                }

                String logText = noTime ? text : AddLogTime(text);

                // reset textbox if TextBoxMaxLength reached
                if (textBox.Text.Length >= MaxTextBoxLength)
                    textBox.Text = "";
                // add to textbox
                textBox.SelectionColor = textColor;
                textBox.AppendText(logText + "\n");
                // set the current caret position to the end
                textBox.SelectionStart = textBox.Text.Length;
                // scroll it automatically
                textBox.ScrollToCaret();
                // add to log file
                WriteLine(logText);
            }
        }

        // Entry(...) : Enregitre une entrée dans le log
        public void Entry(String text)
        {
            Entry(Level.Normal, text);
        }

        // Error(...) : Enregitre une erreur dans le log
        public void Error(String text)
        {
            Entry(Level.Error, text);
        }

        // Success(...) : Enregitre une entrée de succès dans le log
        public void Success(String text)
        {
            Entry(Level.Success, text);
        }

        // Debug(...) : Enregitre une entrée de débeugage dans le log
        public void Debug(String text)
        {
            Entry(Level.Debug, DebugPrefix + text + DebugSuffix);
        }

        // Header(...) : ajoute une entête dans le log
        private void Header(Level level, String text, int length, char c)
        {
            double headerSize = (double)((length - text.Length)) / 2;

            StringBuilder builder = new StringBuilder();

            // Construct the first section
            for (var i = 0; i < (int)Math.Ceiling(headerSize); i++)
                builder.Append(c);

            // Add the text
            builder.Append(text);

            // Construct the last section
            for (var i = 0; i < ((int)Math.Floor(headerSize)); i++)
                builder.Append(c);

            Entry(level, builder.ToString(), true);
        }

        // Divider(...) : ajoute un séparateur dans le log
        public void Divider(Level level, int length = 100, char c = '-')
        {
            Header(level, "", length, c);
        }

        // NewLine(...) : ajoute une nouvelle ligne dans le log
        public void NewLine()
        {
            WriteLine("");
        }

        // Title(...) : ajoute un titre dans le log
        public void Title(Level level, String title, int length = 100, char c = '-')
        {
            if (level != Level.Debug || Settings.EnableDebug)
            {
                NewLine();
                Divider(level, length, c);
                Header(level, title, length, c);
                Divider(level, length, c);
            }
        }
    }
}
