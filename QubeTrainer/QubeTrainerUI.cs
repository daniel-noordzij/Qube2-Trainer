using System;
using System.Drawing;
using System.Windows.Forms;
using Virtua_Cop_2Trainer;
using System.Runtime.InteropServices;
using System.IO;

using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using System.Collections.Generic;

using QubeTrainerNamespace;
using System.Linq;
using System.Media;

namespace QubeTrainerUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        QubeTrainer trainer;

        string[] fileNames = { "MainSaveGame.sav", "MainStatsSaveGame.sav", "MainUnlockedLevels.sav" };
        string sourceVS = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\VaultSave");
        string source;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QUBE\\Saved\\SaveGames\\");

        bool levelsOpen = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            trainer = new QubeTrainer(this);

            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 10;
            toolTip1.ReshowDelay = 10;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.label1, "If the marker positions aren't right, go to the main menu and continue the game to fix them!");
        }

        public void clearAll()
        {
            SetText(valPosX, "");
            SetText(valPosY, "");
            SetText(valPosZ, "");

            SetText(valAngleX, "");
            SetText(valAngleY, "");

            SetText(valMarkX, "");
            SetText(valMarkY, "");
            SetText(valMarkZ, "");

            SetText(valSpeedX, "");
            SetText(valSpeedY, "");
            SetText(valSpeedZ, "");
        }

        public void setAllEnabled(bool enabled)
        {
            foreach (var button in this.Controls.OfType<Button>().Where(btn => (string)btn.Tag != "tagNoDisable"))
            {
                button.Enabled = enabled;
            }
        }

        public void showConnectionError()
        {
            MessageBox.Show("Could not find an open QUBE 2 process!", "Error Finding Process", MessageBoxButtons.OK);
        }

        public void updateAllValues()
        {
            SetText(valPosX,trainer.valX.ToString());
            SetText(valPosY, trainer.valY.ToString());
            SetText(valPosZ, trainer.valZ.ToString());

            SetText(valMarkX, trainer.valMX.ToString());
            SetText(valMarkY, trainer.valMY.ToString());
            SetText(valMarkZ, trainer.valMZ.ToString());

            SetText(valAngleX, trainer.valAX.ToString());
            SetText(valAngleY, trainer.valAY.ToString());

            SetText(valSpeedX,trainer.valSX.ToString());
            SetText(valSpeedY,trainer.valSY.ToString());
            SetText(valSpeedZ,trainer.valSZ.ToString());
        }

        private void btnLevels_Click(object sender, EventArgs e)
        {
            levelsOpen = !levelsOpen;
            if (levelsOpen)
            {
                this.Width = 600;
                lblAuthor.Location = new Point(458, 417);
                lblHelpers.Location = new Point(333, 431);
                lblVersion.Location = new Point(289, 445);
            }
            else
            {
                this.Width = 450;
                lblAuthor.Location = new Point(308, 417);
                lblHelpers.Location = new Point(183, 431);
                lblVersion.Location = new Point(139, 445);
            }
            btnLevels.Text = levelsOpen ? "Levels <" : "Levels >";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!trainer.connected)
            {
                trainer.connect();
            } else
            {
                trainer.disconnect();
            }
        }

        public void SetText(Control obj, string text)
        {
            obj.Invoke(new Action(() =>
            {
                obj.Text = text;
            }));
        }

        private float showNumberDialog(float oldValue)
        {
            using (Form2 form2 = new Form2(oldValue))
            {
                DialogResult dr = form2.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    return float.Parse(form2.getNewVal());
                }
                form2.Close();
            }
            return 0;
        }

        private void chapterClicks(object sender, EventArgs e)
        {
            switch ((sender as Button).Text)
            {
                case "Chapter 1":
                    setLevelButtons(1); break;
                case "Chapter 2":
                    setLevelButtons(2); break;
                case "Chapter 3":
                    setLevelButtons(3); break;
                case "Chapter 4":
                    setLevelButtons(4); break;
                case "Chapter 5":
                    setLevelButtons(5); break;
                case "Chapter 6":
                    setLevelButtons(6); break;
                case "Chapter 7":
                    setLevelButtons(7); break;
                case "Chapter 8":
                    setLevelButtons(8); break;
                case "Chapter 9":
                    setLevelButtons(9); break;
                case "Chapter 10":
                    setLevelButtons(10); break;
                case "Chapter 11":
                    setLevelButtons(11); break;
                default:
                    MessageBox.Show("Something went wrong, please ask for help in the discord!", "Error!", MessageBoxButtons.OK);
                    break;
            }
        }

        private void setLevelButtons(int chapterNum)
        {
            foreach (var button in flowLayoutPanel1.Controls.OfType<Button>().Where(btn => (string)btn.Tag == "tagMain"))
                button.Visible = false;
            foreach (var button in flowLayoutPanel1.Controls.OfType<Button>().Where(btn => (string)btn.Tag == "tagCh" + chapterNum))
                button.Visible = true;
            btnBack.Enabled = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            foreach (var button in flowLayoutPanel1.Controls.OfType<Button>())
                button.Visible = false;
            foreach (var button in flowLayoutPanel1.Controls.OfType<Button>().Where(btn => (string)btn.Tag == "tagMain"))
                button.Visible = true;
            btnBack.Enabled = false;
        }

        private void btnReloadSave_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
            {
                using (StreamReader sr = File.OpenText(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
                {
                    string s = "";
                    if ((s = sr.ReadLine()) != null)
                    {
                        source = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\" + s);
                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            string sourceFile = System.IO.Path.Combine(source, fileNames[i]);
                            string targetFile = System.IO.Path.Combine(target, fileNames[i]);

                            System.IO.File.Copy(sourceFile, targetFile, true);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong, please ask for help in the discord!", "Error!", MessageBoxButtons.OK);
                    }
                }
            }
            else
            {
                MessageBox.Show("No existing previous save!", "Error!", MessageBoxButtons.OK);
            }
        }

        private void setNewSave(object sender, EventArgs e)
        {
            source = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\" + (sender as Button).Text);
            for (int i = 0; i < fileNames.Length; i++)
            {
                string sourceFile = System.IO.Path.Combine(source, fileNames[i]);
                string targetFile = System.IO.Path.Combine(target, fileNames[i]);

                System.IO.File.Copy(sourceFile, targetFile, true);
            }

            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
                {
                    sw.WriteLine((sender as Button).Text);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
                {
                    sw.WriteLine((sender as Button).Text);
                }
            }

            SystemSounds.Beep.Play();
        }

        private void btnVaultSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fileNames.Length; i++)
            {
                string sourceFile = System.IO.Path.Combine(sourceVS, fileNames[i]);
                string targetFile = System.IO.Path.Combine(target, fileNames[i]);

                System.IO.File.Copy(sourceFile, targetFile, true);
            }
        }

        private void SLock4_Click(object sender, EventArgs e)
        {
            trainer.lockSX();
        }

        private void SLock5_Click(object sender, EventArgs e)
        {
            trainer.lockSY();
        }

        private void SLock6_Click(object sender, EventArgs e)
        {
            trainer.lockSZ();
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            toggleDarkMode();
        }

        private void valMarkX_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                trainer.writeFloat(trainer.BaseMX, showNumberDialog(trainer.valMX));
            }
        }

        private void valMarkY_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                trainer.writeFloat(trainer.BaseMY, showNumberDialog(trainer.valMY));
            }
        }

        private void valMarkZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                trainer.writeFloat(trainer.BaseMZ, showNumberDialog(trainer.valMZ));
            }
        }

        private void valPosX_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                trainer.writeFloat(trainer.BaseX, showNumberDialog(trainer.valX));
            }
        }

        private void valPosY_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                trainer.writeFloat(trainer.BaseY, showNumberDialog(trainer.valY));
            }
        }

        private void valPosZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                trainer.writeFloat(trainer.BaseZ, showNumberDialog(trainer.valZ));
            }
        }

        private void btnMoonjump_Click(object sender, EventArgs e)
        {
            trainer.toggleMoonjump();
        }

        private void btnSuperSpeed_Click(object sender, EventArgs e)
        {
            trainer.toggleSuperspeed();
        }

        private void btnLowGravity_Click(object sender, EventArgs e)
        {
            trainer.toggleLowGravity();
        }

        private void btnTeleportToMarker_Click(object sender, EventArgs e)
        {
            trainer.teleportToMarker();
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            trainer.storePosition();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            trainer.restorePosition();
        }

        private void SLock2_Click(object sender, EventArgs e)
        {
            trainer.lockY();
        }

        private void SLock1_Click(object sender, EventArgs e)
        {
            trainer.lockX();
        }

        private void SLock3_Click(object sender, EventArgs e)
        {
            trainer.lockZ();
        }

        private void btnFlyMode_Click(object sender, EventArgs e)
        {
            trainer.toggleFlyMode();
        }

        private void btnHideArms_Click(object sender, EventArgs e)
        {
            trainer.toggleArmsVisible();
        }

        private void toggleDarkMode()
        {
            if (this.BackColor == Color.WhiteSmoke)
            {
                btnDarkMode.Text = "Light Mode";
                setAllForegroundColors(Color.WhiteSmoke);
                this.BackColor = Color.FromArgb(64, 64, 64);
            }
            else
            {
                btnDarkMode.Text = "Dark Mode";
                setAllForegroundColors(Color.Black);
                this.BackColor = Color.WhiteSmoke;
            }
        }

        public void setAllForegroundColors(Color color)
        {
            foreach (var label in this.Controls.OfType<Label>())
            {
                label.ForeColor = color;
            }
        }
    }
}
