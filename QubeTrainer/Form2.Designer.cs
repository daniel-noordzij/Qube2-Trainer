namespace QubeTrainerNamespace
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.valBox = new System.Windows.Forms.TextBox();
            this.btnSetNewPos = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // valBox
            // 
            this.valBox.Location = new System.Drawing.Point(30, 23);
            this.valBox.Name = "valBox";
            this.valBox.Size = new System.Drawing.Size(100, 20);
            this.valBox.TabIndex = 7;
            this.valBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.valBox_KeyPress);
            this.valBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.valBox_KeyUp);
            // 
            // btnSetNewPos
            // 
            this.btnSetNewPos.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSetNewPos.Location = new System.Drawing.Point(137, 22);
            this.btnSetNewPos.Name = "btnSetNewPos";
            this.btnSetNewPos.Size = new System.Drawing.Size(61, 22);
            this.btnSetNewPos.TabIndex = 6;
            this.btnSetNewPos.Text = "Set!";
            this.btnSetNewPos.UseVisualStyleBackColor = true;
            this.btnSetNewPos.Click += new System.EventHandler(this.btnSetNewPos_Click);
            // 
            // Form2
            // 
            this.AcceptButton = this.btnSetNewPos;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 62);
            this.Controls.Add(this.btnSetNewPos);
            this.Controls.Add(this.valBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set New Position";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox valBox;
        private System.Windows.Forms.Button btnSetNewPos;
    }
}