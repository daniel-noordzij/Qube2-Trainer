namespace QubeTrainer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.valPosX = new System.Windows.Forms.TextBox();
            this.lblPosX = new System.Windows.Forms.Label();
            this.lblPosY = new System.Windows.Forms.Label();
            this.lblPosZ = new System.Windows.Forms.Label();
            this.valPosY = new System.Windows.Forms.TextBox();
            this.valPosZ = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.valAngleX = new System.Windows.Forms.TextBox();
            this.lblAngleX = new System.Windows.Forms.Label();
            this.btnMoonjump = new System.Windows.Forms.Button();
            this.btnSuperSpeed = new System.Windows.Forms.Button();
            this.valMarkZ = new System.Windows.Forms.TextBox();
            this.valMarkY = new System.Windows.Forms.TextBox();
            this.lblMarkZ = new System.Windows.Forms.Label();
            this.lblMarkY = new System.Windows.Forms.Label();
            this.lblMarkX = new System.Windows.Forms.Label();
            this.valMarkX = new System.Windows.Forms.TextBox();
            this.btnStore = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnVaultSave = new System.Windows.Forms.Button();
            this.btnTeleportToMarker = new System.Windows.Forms.Button();
            this.valSpeed = new System.Windows.Forms.TextBox();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblPositions = new System.Windows.Forms.Label();
            this.lblCheats = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.SLock1 = new System.Windows.Forms.Button();
            this.SLock2 = new System.Windows.Forms.Button();
            this.SLock3 = new System.Windows.Forms.Button();
            this.btnDarkMode = new System.Windows.Forms.Button();
            this.lblHelpers = new System.Windows.Forms.Label();
            this.valAngleY = new System.Windows.Forms.TextBox();
            this.lblAngleY = new System.Windows.Forms.Label();
            this.btnLowGravity = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // valPosX
            // 
            this.valPosX.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.valPosX.Location = new System.Drawing.Point(112, 98);
            this.valPosX.Name = "valPosX";
            this.valPosX.ReadOnly = true;
            this.valPosX.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.valPosX.Size = new System.Drawing.Size(97, 20);
            this.valPosX.TabIndex = 0;
            this.valPosX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.valPosX_MouseDown);
            // 
            // lblPosX
            // 
            this.lblPosX.AutoSize = true;
            this.lblPosX.BackColor = System.Drawing.Color.Transparent;
            this.lblPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosX.ForeColor = System.Drawing.Color.Black;
            this.lblPosX.Location = new System.Drawing.Point(37, 99);
            this.lblPosX.Name = "lblPosX";
            this.lblPosX.Size = new System.Drawing.Size(46, 16);
            this.lblPosX.TabIndex = 1;
            this.lblPosX.Text = "Pos X:";
            // 
            // lblPosY
            // 
            this.lblPosY.AutoSize = true;
            this.lblPosY.BackColor = System.Drawing.Color.Transparent;
            this.lblPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosY.ForeColor = System.Drawing.Color.Black;
            this.lblPosY.Location = new System.Drawing.Point(37, 124);
            this.lblPosY.Name = "lblPosY";
            this.lblPosY.Size = new System.Drawing.Size(47, 16);
            this.lblPosY.TabIndex = 15;
            this.lblPosY.Text = "Pos Y:";
            // 
            // lblPosZ
            // 
            this.lblPosZ.AutoSize = true;
            this.lblPosZ.BackColor = System.Drawing.Color.Transparent;
            this.lblPosZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPosZ.ForeColor = System.Drawing.Color.Black;
            this.lblPosZ.Location = new System.Drawing.Point(37, 150);
            this.lblPosZ.Name = "lblPosZ";
            this.lblPosZ.Size = new System.Drawing.Size(46, 16);
            this.lblPosZ.TabIndex = 16;
            this.lblPosZ.Text = "Pos Z:";
            // 
            // valPosY
            // 
            this.valPosY.Location = new System.Drawing.Point(112, 123);
            this.valPosY.Name = "valPosY";
            this.valPosY.ReadOnly = true;
            this.valPosY.Size = new System.Drawing.Size(97, 20);
            this.valPosY.TabIndex = 17;
            this.valPosY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.valPosY_MouseDown);
            // 
            // valPosZ
            // 
            this.valPosZ.Location = new System.Drawing.Point(112, 149);
            this.valPosZ.Name = "valPosZ";
            this.valPosZ.ReadOnly = true;
            this.valPosZ.Size = new System.Drawing.Size(97, 20);
            this.valPosZ.TabIndex = 18;
            this.valPosZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.valPosZ_MouseDown);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(37, 28);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(138, 23);
            this.btnConnect.TabIndex = 19;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // valAngleX
            // 
            this.valAngleX.Location = new System.Drawing.Point(112, 208);
            this.valAngleX.Name = "valAngleX";
            this.valAngleX.ReadOnly = true;
            this.valAngleX.Size = new System.Drawing.Size(97, 20);
            this.valAngleX.TabIndex = 21;
            // 
            // lblAngleX
            // 
            this.lblAngleX.AutoSize = true;
            this.lblAngleX.BackColor = System.Drawing.Color.Transparent;
            this.lblAngleX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngleX.ForeColor = System.Drawing.Color.Black;
            this.lblAngleX.Location = new System.Drawing.Point(37, 207);
            this.lblAngleX.Name = "lblAngleX";
            this.lblAngleX.Size = new System.Drawing.Size(57, 16);
            this.lblAngleX.TabIndex = 20;
            this.lblAngleX.Text = "Angle X:";
            // 
            // btnMoonjump
            // 
            this.btnMoonjump.Enabled = false;
            this.btnMoonjump.Location = new System.Drawing.Point(287, 97);
            this.btnMoonjump.Name = "btnMoonjump";
            this.btnMoonjump.Size = new System.Drawing.Size(121, 22);
            this.btnMoonjump.TabIndex = 22;
            this.btnMoonjump.Text = "Moonjump (Off)";
            this.btnMoonjump.UseVisualStyleBackColor = true;
            this.btnMoonjump.Click += new System.EventHandler(this.btnMoonjump_Click);
            // 
            // btnSuperSpeed
            // 
            this.btnSuperSpeed.Enabled = false;
            this.btnSuperSpeed.Location = new System.Drawing.Point(287, 125);
            this.btnSuperSpeed.Name = "btnSuperSpeed";
            this.btnSuperSpeed.Size = new System.Drawing.Size(121, 22);
            this.btnSuperSpeed.TabIndex = 23;
            this.btnSuperSpeed.Text = "Super Speed (Off)";
            this.btnSuperSpeed.UseVisualStyleBackColor = true;
            this.btnSuperSpeed.Click += new System.EventHandler(this.btnSuperSpeed_Click);
            // 
            // valMarkZ
            // 
            this.valMarkZ.Location = new System.Drawing.Point(112, 318);
            this.valMarkZ.Name = "valMarkZ";
            this.valMarkZ.ReadOnly = true;
            this.valMarkZ.Size = new System.Drawing.Size(97, 20);
            this.valMarkZ.TabIndex = 29;
            this.valMarkZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.valMarkZ_MouseDown);
            // 
            // valMarkY
            // 
            this.valMarkY.Location = new System.Drawing.Point(112, 292);
            this.valMarkY.Name = "valMarkY";
            this.valMarkY.ReadOnly = true;
            this.valMarkY.Size = new System.Drawing.Size(97, 20);
            this.valMarkY.TabIndex = 28;
            this.valMarkY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.valMarkY_MouseDown);
            // 
            // lblMarkZ
            // 
            this.lblMarkZ.AutoSize = true;
            this.lblMarkZ.BackColor = System.Drawing.Color.Transparent;
            this.lblMarkZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarkZ.ForeColor = System.Drawing.Color.Black;
            this.lblMarkZ.Location = new System.Drawing.Point(36, 319);
            this.lblMarkZ.Name = "lblMarkZ";
            this.lblMarkZ.Size = new System.Drawing.Size(64, 16);
            this.lblMarkZ.TabIndex = 27;
            this.lblMarkZ.Text = "Marker Z:";
            // 
            // lblMarkY
            // 
            this.lblMarkY.AutoSize = true;
            this.lblMarkY.BackColor = System.Drawing.Color.Transparent;
            this.lblMarkY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarkY.ForeColor = System.Drawing.Color.Black;
            this.lblMarkY.Location = new System.Drawing.Point(36, 293);
            this.lblMarkY.Name = "lblMarkY";
            this.lblMarkY.Size = new System.Drawing.Size(65, 16);
            this.lblMarkY.TabIndex = 26;
            this.lblMarkY.Text = "Marker Y:";
            // 
            // lblMarkX
            // 
            this.lblMarkX.AutoSize = true;
            this.lblMarkX.BackColor = System.Drawing.Color.Transparent;
            this.lblMarkX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMarkX.ForeColor = System.Drawing.Color.Black;
            this.lblMarkX.Location = new System.Drawing.Point(36, 268);
            this.lblMarkX.Name = "lblMarkX";
            this.lblMarkX.Size = new System.Drawing.Size(64, 16);
            this.lblMarkX.TabIndex = 25;
            this.lblMarkX.Text = "Marker X:";
            // 
            // valMarkX
            // 
            this.valMarkX.Location = new System.Drawing.Point(112, 267);
            this.valMarkX.Name = "valMarkX";
            this.valMarkX.ReadOnly = true;
            this.valMarkX.Size = new System.Drawing.Size(97, 20);
            this.valMarkX.TabIndex = 24;
            this.valMarkX.MouseDown += new System.Windows.Forms.MouseEventHandler(this.valMarkX_MouseDown);
            // 
            // btnStore
            // 
            this.btnStore.Enabled = false;
            this.btnStore.Location = new System.Drawing.Point(287, 181);
            this.btnStore.Name = "btnStore";
            this.btnStore.Size = new System.Drawing.Size(121, 22);
            this.btnStore.TabIndex = 30;
            this.btnStore.Text = "Store Position";
            this.btnStore.UseVisualStyleBackColor = true;
            this.btnStore.Click += new System.EventHandler(this.btnStore_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Enabled = false;
            this.btnRestore.Location = new System.Drawing.Point(287, 209);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(121, 22);
            this.btnRestore.TabIndex = 32;
            this.btnRestore.Text = "Restore Position";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnVaultSave
            // 
            this.btnVaultSave.Enabled = false;
            this.btnVaultSave.Location = new System.Drawing.Point(287, 265);
            this.btnVaultSave.Name = "btnVaultSave";
            this.btnVaultSave.Size = new System.Drawing.Size(121, 22);
            this.btnVaultSave.TabIndex = 33;
            this.btnVaultSave.Text = "Set Vault Save";
            this.btnVaultSave.UseVisualStyleBackColor = true;
            this.btnVaultSave.Click += new System.EventHandler(this.btnVaultSave_Click);
            // 
            // btnTeleportToMarker
            // 
            this.btnTeleportToMarker.Enabled = false;
            this.btnTeleportToMarker.Location = new System.Drawing.Point(287, 237);
            this.btnTeleportToMarker.Name = "btnTeleportToMarker";
            this.btnTeleportToMarker.Size = new System.Drawing.Size(121, 22);
            this.btnTeleportToMarker.TabIndex = 34;
            this.btnTeleportToMarker.Text = "Teleport To Marker";
            this.btnTeleportToMarker.UseVisualStyleBackColor = true;
            this.btnTeleportToMarker.Click += new System.EventHandler(this.btnTeleportToMarker_Click);
            // 
            // valSpeed
            // 
            this.valSpeed.Location = new System.Drawing.Point(112, 183);
            this.valSpeed.Name = "valSpeed";
            this.valSpeed.ReadOnly = true;
            this.valSpeed.Size = new System.Drawing.Size(97, 20);
            this.valSpeed.TabIndex = 36;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeed.ForeColor = System.Drawing.Color.Black;
            this.lblSpeed.Location = new System.Drawing.Point(37, 182);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(52, 16);
            this.lblSpeed.TabIndex = 35;
            this.lblSpeed.Text = "Speed:";
            // 
            // lblPositions
            // 
            this.lblPositions.AutoSize = true;
            this.lblPositions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPositions.ForeColor = System.Drawing.Color.Black;
            this.lblPositions.Location = new System.Drawing.Point(36, 66);
            this.lblPositions.Name = "lblPositions";
            this.lblPositions.Size = new System.Drawing.Size(84, 20);
            this.lblPositions.TabIndex = 37;
            this.lblPositions.Text = "Player Info";
            // 
            // lblCheats
            // 
            this.lblCheats.AutoSize = true;
            this.lblCheats.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheats.ForeColor = System.Drawing.Color.Black;
            this.lblCheats.Location = new System.Drawing.Point(283, 67);
            this.lblCheats.Name = "lblCheats";
            this.lblCheats.Size = new System.Drawing.Size(60, 20);
            this.lblCheats.TabIndex = 38;
            this.lblCheats.Text = "Cheats";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(151, 391);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(283, 13);
            this.lblVersion.TabIndex = 40;
            this.lblVersion.Text = "Trainer Version 1.0  for  Q.U.B.E. 2 Version Windows 1.0.9";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(320, 358);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(114, 13);
            this.lblAuthor.TabIndex = 41;
            this.lblAuthor.Text = "Made by SmokeyRuns";
            // 
            // SLock1
            // 
            this.SLock1.Enabled = false;
            this.SLock1.Location = new System.Drawing.Point(212, 97);
            this.SLock1.Name = "SLock1";
            this.SLock1.Size = new System.Drawing.Size(49, 22);
            this.SLock1.TabIndex = 42;
            this.SLock1.Text = "Lock";
            this.SLock1.UseVisualStyleBackColor = true;
            this.SLock1.Click += new System.EventHandler(this.SLock1_Click);
            // 
            // SLock2
            // 
            this.SLock2.Enabled = false;
            this.SLock2.Location = new System.Drawing.Point(212, 122);
            this.SLock2.Name = "SLock2";
            this.SLock2.Size = new System.Drawing.Size(49, 22);
            this.SLock2.TabIndex = 43;
            this.SLock2.Text = "Lock";
            this.SLock2.UseVisualStyleBackColor = true;
            this.SLock2.Click += new System.EventHandler(this.SLock2_Click);
            // 
            // SLock3
            // 
            this.SLock3.Enabled = false;
            this.SLock3.Location = new System.Drawing.Point(212, 148);
            this.SLock3.Name = "SLock3";
            this.SLock3.Size = new System.Drawing.Size(49, 22);
            this.SLock3.TabIndex = 44;
            this.SLock3.Text = "Lock";
            this.SLock3.UseVisualStyleBackColor = true;
            this.SLock3.Click += new System.EventHandler(this.SLock3_Click);
            // 
            // btnDarkMode
            // 
            this.btnDarkMode.Location = new System.Drawing.Point(318, 29);
            this.btnDarkMode.Name = "btnDarkMode";
            this.btnDarkMode.Size = new System.Drawing.Size(90, 22);
            this.btnDarkMode.TabIndex = 46;
            this.btnDarkMode.Text = "Dark Mode";
            this.btnDarkMode.UseVisualStyleBackColor = true;
            this.btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);
            // 
            // lblHelpers
            // 
            this.lblHelpers.AutoSize = true;
            this.lblHelpers.Location = new System.Drawing.Point(195, 374);
            this.lblHelpers.Name = "lblHelpers";
            this.lblHelpers.Size = new System.Drawing.Size(239, 13);
            this.lblHelpers.TabIndex = 48;
            this.lblHelpers.Text = "Thanks to CalMc and MonsterDruide1 for helping";
            // 
            // valAngleY
            // 
            this.valAngleY.Location = new System.Drawing.Point(112, 234);
            this.valAngleY.Name = "valAngleY";
            this.valAngleY.ReadOnly = true;
            this.valAngleY.Size = new System.Drawing.Size(97, 20);
            this.valAngleY.TabIndex = 51;
            // 
            // lblAngleY
            // 
            this.lblAngleY.AutoSize = true;
            this.lblAngleY.BackColor = System.Drawing.Color.Transparent;
            this.lblAngleY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngleY.ForeColor = System.Drawing.Color.Black;
            this.lblAngleY.Location = new System.Drawing.Point(37, 233);
            this.lblAngleY.Name = "lblAngleY";
            this.lblAngleY.Size = new System.Drawing.Size(58, 16);
            this.lblAngleY.TabIndex = 50;
            this.lblAngleY.Text = "Angle Y:";
            // 
            // btnLowGravity
            // 
            this.btnLowGravity.Enabled = false;
            this.btnLowGravity.Location = new System.Drawing.Point(287, 153);
            this.btnLowGravity.Name = "btnLowGravity";
            this.btnLowGravity.Size = new System.Drawing.Size(121, 22);
            this.btnLowGravity.TabIndex = 52;
            this.btnLowGravity.Text = "Low Gravity (Off)";
            this.btnLowGravity.UseVisualStyleBackColor = true;
            this.btnLowGravity.Click += new System.EventHandler(this.btnLowGravity_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "Not Working?";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(446, 409);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLowGravity);
            this.Controls.Add(this.valAngleY);
            this.Controls.Add(this.lblAngleY);
            this.Controls.Add(this.lblHelpers);
            this.Controls.Add(this.btnDarkMode);
            this.Controls.Add(this.SLock3);
            this.Controls.Add(this.SLock2);
            this.Controls.Add(this.SLock1);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblCheats);
            this.Controls.Add(this.lblPositions);
            this.Controls.Add(this.valSpeed);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.btnTeleportToMarker);
            this.Controls.Add(this.btnVaultSave);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnStore);
            this.Controls.Add(this.valMarkZ);
            this.Controls.Add(this.valMarkY);
            this.Controls.Add(this.lblMarkZ);
            this.Controls.Add(this.lblMarkY);
            this.Controls.Add(this.lblMarkX);
            this.Controls.Add(this.valMarkX);
            this.Controls.Add(this.btnSuperSpeed);
            this.Controls.Add(this.btnMoonjump);
            this.Controls.Add(this.valAngleX);
            this.Controls.Add(this.lblAngleX);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.valPosZ);
            this.Controls.Add(this.valPosY);
            this.Controls.Add(this.lblPosZ);
            this.Controls.Add(this.lblPosY);
            this.Controls.Add(this.lblPosX);
            this.Controls.Add(this.valPosX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Qube 2 Trainer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox valPosX;
        private System.Windows.Forms.Label lblPosX;
        private System.Windows.Forms.Label lblPosY;
        private System.Windows.Forms.Label lblPosZ;
        private System.Windows.Forms.TextBox valPosY;
        private System.Windows.Forms.TextBox valPosZ;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox valAngleX;
        private System.Windows.Forms.Label lblAngleX;
        private System.Windows.Forms.Button btnMoonjump;
        private System.Windows.Forms.Button btnSuperSpeed;
        private System.Windows.Forms.TextBox valMarkZ;
        private System.Windows.Forms.TextBox valMarkY;
        private System.Windows.Forms.Label lblMarkZ;
        private System.Windows.Forms.Label lblMarkY;
        private System.Windows.Forms.Label lblMarkX;
        private System.Windows.Forms.TextBox valMarkX;
        private System.Windows.Forms.Button btnStore;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnVaultSave;
        private System.Windows.Forms.Button btnTeleportToMarker;
        private System.Windows.Forms.TextBox valSpeed;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblPositions;
        private System.Windows.Forms.Label lblCheats;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Button SLock1;
        private System.Windows.Forms.Button SLock2;
        private System.Windows.Forms.Button SLock3;
        private System.Windows.Forms.Button btnDarkMode;
        private System.Windows.Forms.Label lblHelpers;
        private System.Windows.Forms.TextBox valAngleY;
        private System.Windows.Forms.Label lblAngleY;
        private System.Windows.Forms.Button btnLowGravity;
        private System.Windows.Forms.Label label1;
    }
}

