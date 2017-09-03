using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Pixus
{
    //event handler
    public delegate void CheckChangedEventHandler(object sender, CheckChangedEventArgs e);

    [DefaultEvent("CheckChanged")]
    public partial class ToggleSwitch : UserControl
    {
        //internal variables and their defaults.
        private Color _ColorToggleOn = Color.SteelBlue;
        private Color _ColorToggleOff = Color.DarkGray;
        private Color _ColorButtonOn = Color.WhiteSmoke;
        private Color _ColorButtonOff = Color.WhiteSmoke;
        private string _TextON = "ON";
        private string _TextOFF = "OFF";
        private bool _checked = false;
        private bool _BorderExtraThin = true;
        private bool _BorderForButton = true;
        private int _ButtonWidthPercentage = 50;

        //public properties that will show in the designer
        public Color ColorToggleOn { get { return _ColorToggleOn; } set { _ColorToggleOn = value; UpdateColors(); } }
        public Color ColorToggleOff { get { return _ColorToggleOff; } set { _ColorToggleOff = value; UpdateColors(); } }
        public Color ColorButtonOn { get { return _ColorButtonOn; } set { _ColorButtonOn = value; UpdateColors(); } }
        public Color ColorButtonOff { get { return _ColorButtonOff; } set { _ColorButtonOff = value; UpdateColors(); } }
        public string TextON { get { return _TextON; } set { _TextON = value; UpdateColors(); } }
        public string TextOFF { get { return _TextOFF; } set { _TextOFF = value; UpdateColors(); } }
        public bool BorderExtraThin { get { return _BorderExtraThin; } set { _BorderExtraThin = value; UpdateBorders(); UpdateColors(); Refresh(); } }
        public bool BorderForButton { get { return _BorderForButton; } set { _BorderForButton = value; UpdateBorders(); UpdateColors(); Refresh(); } }
        public int ButtonWidthPercentage { get { return _ButtonWidthPercentage; } set { _ButtonWidthPercentage = value; UpdateBorders(); UpdateColors(); Refresh(); } }

        public bool Checked
        {
            get { return _checked; }

            set
            {
                //This is set up so change events don't even occur unless the value is really changing.
                //This behavior could be modified if for some reason you want a change event without
                //  the value actually changing; which explains the CheckChangedEventArgs having a
                //  old value and a new value, even though the current implementation guarantees
                //  they are actually different.

                if (Checked == value)
                    return;

                _checked = value;
                UpdateColors();

                CheckChanged(this, new CheckChangedEventArgs(!Checked, Checked));
            }
        }

        //we use a floating label control as the "button" and optional text.

        //CheckChanged is the default event for this control, and the only custom one we need.
        [Category("Action")]
        [Description("Fires when the switch is toggled, just like a checkbox")]
        public event CheckChangedEventHandler CheckChanged;

        void _CheckChangedDoNothing(object sender, CheckChangedEventArgs e)
        {
        }

        public ToggleSwitch()
        {
            CheckChanged = new CheckChangedEventHandler(_CheckChangedDoNothing);

            label1 = new Label();
            label1.ForeColor = this.ForeColor;
            label1.Visible = true;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.MouseDown += new MouseEventHandler(label1_MouseDown);

            this.Width = 60;
            this.Height = 16;
            this.Controls.Add(label1);

            UpdateColors();
        }

        void label1_MouseDown(object sender, EventArgs e)
        {
            Clicked();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Clicked();
        }

        private void Clicked()
        {
            Checked = !Checked;
        }

        private void UpdateBorders()
        {
            if (BorderExtraThin)
            {
                BorderStyle = System.Windows.Forms.BorderStyle.None;
            }

            if (BorderForButton)
            {
                label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            }
        }

        private void UpdateColors()
        {
            if (Checked)
            {
                this.BackColor = ColorToggleOn;
                this.label1.Dock = DockStyle.Right;
                label1.Width = (ClientRectangle.Width * ButtonWidthPercentage) / 100;
                this.label1.Text = TextON;
                this.label1.BackColor = ColorButtonOn;

                //not sure why but it seems to need a 1-px offset to look correct
                this.label1.Padding = new Padding(1, 0, 0, 0);


                this.Refresh();
            }
            else
            {
                this.BackColor = ColorToggleOff;
                this.label1.Dock = DockStyle.Left;
                label1.Width = (ClientRectangle.Width * ButtonWidthPercentage) / 100;
                this.label1.Text = TextOFF;
                this.label1.BackColor = ColorButtonOff;

                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int BORDER_SIZE = 1;

            if (BorderExtraThin)
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                    Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset,
                    Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset,
                    Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset,
                    Color.Black, BORDER_SIZE, ButtonBorderStyle.Inset);
            }
        }
    }

    //data for a change event. the current implementation
    //guarantees OldValue and NewValue are always different,
    //but the change event is built this way so you could change
    //internal behavior if you wanted.

    public class CheckChangedEventArgs : EventArgs
    {
        public bool OldValue { get; set; }
        public bool NewValue { get; set; }

        public CheckChangedEventArgs(bool old_value, bool new_value)
        {
            NewValue = new_value;
            OldValue = old_value;
        }
    }
}
