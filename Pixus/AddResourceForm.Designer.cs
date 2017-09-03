namespace Pixus
{
    partial class AddResourceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddResourceForm));
            this.NoResourceColorCheckBox = new System.Windows.Forms.CheckBox();
            this.ResourceYPosTextBox = new System.Windows.Forms.TextBox();
            this.ResourceXPosTextBox = new System.Windows.Forms.TextBox();
            this.ResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.AddResourceBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ResourceColorImagePictureBox = new System.Windows.Forms.PictureBox();
            this.SelectResourceBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ResourceColorImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // NoResourceColorCheckBox
            // 
            this.NoResourceColorCheckBox.AutoSize = true;
            this.NoResourceColorCheckBox.Location = new System.Drawing.Point(147, 72);
            this.NoResourceColorCheckBox.Name = "NoResourceColorCheckBox";
            this.NoResourceColorCheckBox.Size = new System.Drawing.Size(141, 17);
            this.NoResourceColorCheckBox.TabIndex = 40;
            this.NoResourceColorCheckBox.Text = "ne pas vérifier la couleur";
            this.NoResourceColorCheckBox.UseVisualStyleBackColor = true;
            // 
            // ResourceYPosTextBox
            // 
            this.ResourceYPosTextBox.Location = new System.Drawing.Point(188, 42);
            this.ResourceYPosTextBox.Name = "ResourceYPosTextBox";
            this.ResourceYPosTextBox.ReadOnly = true;
            this.ResourceYPosTextBox.Size = new System.Drawing.Size(100, 20);
            this.ResourceYPosTextBox.TabIndex = 38;
            // 
            // ResourceXPosTextBox
            // 
            this.ResourceXPosTextBox.Location = new System.Drawing.Point(72, 42);
            this.ResourceXPosTextBox.Name = "ResourceXPosTextBox";
            this.ResourceXPosTextBox.ReadOnly = true;
            this.ResourceXPosTextBox.Size = new System.Drawing.Size(100, 20);
            this.ResourceXPosTextBox.TabIndex = 37;
            // 
            // ResourceNameComboBox
            // 
            this.ResourceNameComboBox.FormattingEnabled = true;
            this.ResourceNameComboBox.Location = new System.Drawing.Point(72, 14);
            this.ResourceNameComboBox.Name = "ResourceNameComboBox";
            this.ResourceNameComboBox.Size = new System.Drawing.Size(216, 21);
            this.ResourceNameComboBox.TabIndex = 36;
            // 
            // AddResourceBtn
            // 
            this.AddResourceBtn.Enabled = false;
            this.AddResourceBtn.Location = new System.Drawing.Point(188, 108);
            this.AddResourceBtn.Name = "AddResourceBtn";
            this.AddResourceBtn.Size = new System.Drawing.Size(101, 26);
            this.AddResourceBtn.TabIndex = 34;
            this.AddResourceBtn.Text = "Ajouter";
            this.AddResourceBtn.UseVisualStyleBackColor = true;
            this.AddResourceBtn.Click += new System.EventHandler(this.AddResourceBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Couleur :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Position :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Nom :";
            // 
            // ResourceColorImagePictureBox
            // 
            this.ResourceColorImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ResourceColorImagePictureBox.Location = new System.Drawing.Point(72, 68);
            this.ResourceColorImagePictureBox.Name = "ResourceColorImagePictureBox";
            this.ResourceColorImagePictureBox.Size = new System.Drawing.Size(20, 20);
            this.ResourceColorImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ResourceColorImagePictureBox.TabIndex = 39;
            this.ResourceColorImagePictureBox.TabStop = false;
            // 
            // SelectResourceBtn
            // 
            this.SelectResourceBtn.Image = global::Pixus.Properties.Resources.color_picker;
            this.SelectResourceBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectResourceBtn.Location = new System.Drawing.Point(12, 108);
            this.SelectResourceBtn.Name = "SelectResourceBtn";
            this.SelectResourceBtn.Size = new System.Drawing.Size(160, 26);
            this.SelectResourceBtn.TabIndex = 35;
            this.SelectResourceBtn.Text = "Choisir la ressource";
            this.SelectResourceBtn.UseVisualStyleBackColor = true;
            this.SelectResourceBtn.Click += new System.EventHandler(this.SelectResourceBtn_Click);
            // 
            // AddResourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 148);
            this.Controls.Add(this.NoResourceColorCheckBox);
            this.Controls.Add(this.ResourceColorImagePictureBox);
            this.Controls.Add(this.ResourceYPosTextBox);
            this.Controls.Add(this.ResourceXPosTextBox);
            this.Controls.Add(this.ResourceNameComboBox);
            this.Controls.Add(this.SelectResourceBtn);
            this.Controls.Add(this.AddResourceBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddResourceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ajouter une ressource";
            this.Load += new System.EventHandler(this.AddResourceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResourceColorImagePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox NoResourceColorCheckBox;
        private System.Windows.Forms.PictureBox ResourceColorImagePictureBox;
        private System.Windows.Forms.TextBox ResourceYPosTextBox;
        private System.Windows.Forms.TextBox ResourceXPosTextBox;
        private System.Windows.Forms.ComboBox ResourceNameComboBox;
        private System.Windows.Forms.Button SelectResourceBtn;
        private System.Windows.Forms.Button AddResourceBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}