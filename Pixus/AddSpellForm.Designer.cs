namespace Pixus
{
    partial class AddSpellForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddSpellForm));
            this.SpellYPosTextBox = new System.Windows.Forms.TextBox();
            this.SpellXPosTextBox = new System.Windows.Forms.TextBox();
            this.SpellNameComboBox = new System.Windows.Forms.ComboBox();
            this.AddSpellBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectSpellBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SpellRelaunchTurnNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SpellLaunchTurnNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.SpellRelaunchTurnNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpellLaunchTurnNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // SpellYPosTextBox
            // 
            this.SpellYPosTextBox.Location = new System.Drawing.Point(188, 46);
            this.SpellYPosTextBox.Name = "SpellYPosTextBox";
            this.SpellYPosTextBox.ReadOnly = true;
            this.SpellYPosTextBox.Size = new System.Drawing.Size(100, 20);
            this.SpellYPosTextBox.TabIndex = 47;
            // 
            // SpellXPosTextBox
            // 
            this.SpellXPosTextBox.Location = new System.Drawing.Point(72, 46);
            this.SpellXPosTextBox.Name = "SpellXPosTextBox";
            this.SpellXPosTextBox.ReadOnly = true;
            this.SpellXPosTextBox.Size = new System.Drawing.Size(100, 20);
            this.SpellXPosTextBox.TabIndex = 46;
            // 
            // SpellNameComboBox
            // 
            this.SpellNameComboBox.FormattingEnabled = true;
            this.SpellNameComboBox.Location = new System.Drawing.Point(72, 15);
            this.SpellNameComboBox.Name = "SpellNameComboBox";
            this.SpellNameComboBox.Size = new System.Drawing.Size(216, 21);
            this.SpellNameComboBox.TabIndex = 45;
            // 
            // AddSpellBtn
            // 
            this.AddSpellBtn.Enabled = false;
            this.AddSpellBtn.Location = new System.Drawing.Point(187, 145);
            this.AddSpellBtn.Name = "AddSpellBtn";
            this.AddSpellBtn.Size = new System.Drawing.Size(101, 26);
            this.AddSpellBtn.TabIndex = 43;
            this.AddSpellBtn.Text = "Ajouter";
            this.AddSpellBtn.UseVisualStyleBackColor = true;
            this.AddSpellBtn.Click += new System.EventHandler(this.AddSpellBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Position :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Nom :";
            // 
            // SelectSpellBtn
            // 
            this.SelectSpellBtn.Image = global::Pixus.Properties.Resources.color_picker;
            this.SelectSpellBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectSpellBtn.Location = new System.Drawing.Point(15, 145);
            this.SelectSpellBtn.Name = "SelectSpellBtn";
            this.SelectSpellBtn.Size = new System.Drawing.Size(160, 26);
            this.SelectSpellBtn.TabIndex = 44;
            this.SelectSpellBtn.Text = "Choisir le sort";
            this.SelectSpellBtn.UseVisualStyleBackColor = true;
            this.SelectSpellBtn.Click += new System.EventHandler(this.SelectSpellBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Relancer tout les :";
            // 
            // SpellRelaunchTurnNumericUpDown
            // 
            this.SpellRelaunchTurnNumericUpDown.Location = new System.Drawing.Point(111, 78);
            this.SpellRelaunchTurnNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpellRelaunchTurnNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpellRelaunchTurnNumericUpDown.Name = "SpellRelaunchTurnNumericUpDown";
            this.SpellRelaunchTurnNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.SpellRelaunchTurnNumericUpDown.TabIndex = 50;
            this.SpellRelaunchTurnNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 51;
            this.label5.Text = "tour(s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 52;
            this.label3.Text = "Lancer à partir du tour :";
            // 
            // SpellLaunchTurnNumericUpDown
            // 
            this.SpellLaunchTurnNumericUpDown.Location = new System.Drawing.Point(135, 109);
            this.SpellLaunchTurnNumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpellLaunchTurnNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpellLaunchTurnNumericUpDown.Name = "SpellLaunchTurnNumericUpDown";
            this.SpellLaunchTurnNumericUpDown.Size = new System.Drawing.Size(52, 20);
            this.SpellLaunchTurnNumericUpDown.TabIndex = 53;
            this.SpellLaunchTurnNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // AddSpellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 183);
            this.Controls.Add(this.SpellLaunchTurnNumericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SpellRelaunchTurnNumericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SpellYPosTextBox);
            this.Controls.Add(this.SpellXPosTextBox);
            this.Controls.Add(this.SpellNameComboBox);
            this.Controls.Add(this.SelectSpellBtn);
            this.Controls.Add(this.AddSpellBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddSpellForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ajouter un sort";
            this.Load += new System.EventHandler(this.AddSpellForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpellRelaunchTurnNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpellLaunchTurnNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SpellYPosTextBox;
        private System.Windows.Forms.TextBox SpellXPosTextBox;
        private System.Windows.Forms.ComboBox SpellNameComboBox;
        private System.Windows.Forms.Button SelectSpellBtn;
        private System.Windows.Forms.Button AddSpellBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown SpellRelaunchTurnNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown SpellLaunchTurnNumericUpDown;
    }
}