using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Pixus
{
     /// <summary>
    ///   Enumerates possible actions to be performed by a
    ///   CheckGroupBox whenever its Check state changes.
    /// </summary>
    public enum CheckGroupBoxCheckAction
    {
        None, Enable, Disable
    }

    /// <summary>
    ///   Represents a Windows control that displays a frame around
    ///   a group of controls with an optional caption and checkbox.
    /// </summary>
    public partial class CheckGroupBox : GroupBox
    {

        private CheckBox m_checkBox;
        private bool m_contentsEnabled = true;
        private CheckGroupBoxCheckAction m_checkAction =
            CheckGroupBoxCheckAction.Enable;

        /// <summary>
        ///   Initializes a new instance of CheckGroupBox class.
        /// </summary>
        public CheckGroupBox()
        {
            this.SuspendLayout();

            this.m_checkBox = new CheckBox();
            this.m_checkBox.AutoSize = true;
            this.m_checkBox.Location = new Point(8, 0);
            this.m_checkBox.Padding = new Padding(3, 0, 0, 0);
            this.m_checkBox.Checked = true;
            this.m_checkBox.TextAlign = ContentAlignment.MiddleLeft;
            this.m_checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
            this.Controls.Add(this.m_checkBox);

            this.ResumeLayout(true);
        }

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether the
        /// CheckGroupBox is in the checked state.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool Checked
        {
            get { return this.m_checkBox.Checked; }
            set { this.m_checkBox.Checked = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the controls
        /// contained inside this container can respond to user
        /// interaction.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ContentsEnabled
        {
            get { return this.m_contentsEnabled; }
            set
            {
                this.m_contentsEnabled = value;
                this.OnContentsEnabledChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// The text associated with the control.
        /// </summary>
        public override string Text
        {
            get { return this.m_checkBox.Text; }
            set { this.m_checkBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating how a CheckGroupBox
        /// should behave when its CheckBox is in the checked state.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(CheckGroupBoxCheckAction.Enable)]
        public CheckGroupBoxCheckAction CheckAction
        {
            get { return this.m_checkAction; }
            set
            {
                this.m_checkAction = value;
                this.OnCheckedChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the underlying CheckBox control contained
        /// in the CheckGroupBox control.
        /// </summary>
        [Category("Misc")]
        public CheckBox CheckBox
        {
            get { return this.m_checkBox; }
        }
        #endregion

        #region Event Handling
        /// <summary>
        /// CheckGroupBox.CheckBox CheckedChanged event.
        /// </summary>
        /// <param name=”e”></param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (this.m_checkAction != CheckGroupBoxCheckAction.None)
            {
                // Toggle action depending on the value of checkAction.
                //   The ^ means a xor operation. The xor operation
                //   acts as a inversor, inverting the second operand
                //   whenever the first operand is true.

                this.ContentsEnabled =
                    (this.m_checkAction == CheckGroupBoxCheckAction.Disable)
                     ^ this.m_checkBox.Checked;
            }
        }

        /// <summary>
        /// ContentsEnabled Changed event.
        /// </summary>
        /// <param name=”e”></param>
        protected virtual void OnContentsEnabledChanged(EventArgs e)
        {
            this.SuspendLayout();
            foreach (Control control in this.Controls)
            {
                if (control != this.m_checkBox)
                {
                    // Set action for every control, except for
                    //  the CheckBox, which should remain intact.
                    control.Enabled = this.m_contentsEnabled;
                }
            }
            this.ResumeLayout(true);
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.OnCheckedChanged(e);
        }
        #endregion

    }
}
