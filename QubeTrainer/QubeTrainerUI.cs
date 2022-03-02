using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text.RegularExpressions;

using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

using QubeTrainerNamespace;

namespace QubeTrainerUI
{
    public partial class Form1 : Form
    {
        QubeTrainerClass trainer;

        private bool mouseDown;
        private Point lastLocation;
        private int currentTab = 1;
        private string currentChapterTab = "0";
        private string selectedLevel = "0";

        public bool hotkeyClicked = false;
        public Label lastHotkey;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trainer = new QubeTrainerClass(this);
            trainer.checkNewUpdate();
        }
        private void pnlTopBar_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.SizeAll;
            mouseDown = true;
            lastLocation = e.Location;
        }
        private void pnlTopBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }
        private void pnlTopBar_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            mouseDown = false;
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
            foreach (var button in pnlLevels.Controls.OfType<Button>())
            {
                button.Enabled = enabled;
            }
        }

        public void showMessageBox(String message, String title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK);
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

        private void setLevelButtons(int chapterNum)
        {
            /*foreach (var button in flowLayoutPanel1.Controls.OfType<Button>().Where(btn => (string)btn.Tag == "tagMain"))
                button.Visible = false;
            foreach (var button in flowLayoutPanel1.Controls.OfType<Button>().Where(btn => (string)btn.Tag == "tagCh" + chapterNum))
                button.Visible = true;
            
             enable again when new level selector has been implement with the proper prefix
             */
        }

        private void btnReloadSave_Click(object sender, EventArgs e)
        {
            trainer.reloadSave();
        }

        private void setNewSave(object sender, EventArgs e)
        {
            trainer.setSave((sender as Button).Text);
        }

        private void btnVaultSave_Click(object sender, EventArgs e)
        {
            trainer.loadVaultSave();
        }

        #region listeners for number fields: read new value from dialog and write it to memory
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
                float newVal = showNumberDialog(trainer.valX);
                trainer.valLockX = newVal;
                trainer.writeFloat(trainer.BaseX, newVal);
            }
        }

        private void valPosY_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                float newVal = showNumberDialog(trainer.valY);
                trainer.valLockY = newVal;
                trainer.writeFloat(trainer.BaseY, newVal);
            }
        }

        private void valPosZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (trainer.connected)
            {
                float newVal = showNumberDialog(trainer.valZ);
                trainer.valLockZ = newVal;
                trainer.writeFloat(trainer.BaseZ, newVal);
            }
        }
        #endregion

        #region single-action listeners: Just pass them on to trainer
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
        #endregion
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnConnect_MouseDown(object sender, MouseEventArgs e)
        {
            btnConnect.Size = new Size(132, 115);
            btnConnect.Location = new Point(51, 27);
        }
        private void btnConnect_MouseUp(object sender, MouseEventArgs e)
        {
            btnConnect.Size = new Size(138, 121);
            btnConnect.Location = new Point(48, 24);
        }
        private void btnConnect_MouseEnter(object sender, EventArgs e)
        {
            btnConnect.Size = new Size(142, 125);
            btnConnect.Location = new Point(46, 22);
        }
        private void btnConnect_MouseLeave(object sender, EventArgs e)
        {
            btnConnect.Size = new Size(138, 121);
            btnConnect.Location = new Point(48, 24);
        }
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.FromArgb(62, 62, 62);
        }
        private void btnMinimize_MouseEnter(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.FromArgb(62, 62, 62);
        }
        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.FromArgb(52, 52, 52);
        }
        private void btnMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.FromArgb(52, 52, 52);
        }

        private void btnGithub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/daniel-noordzij/Qube2-Trainer");
        }

        private void btnDiscord_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/Qb36QHf");
        }

        private void btnSrc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.speedrun.com/qube2");
        }
        private void linkButtonEnter(object sender, EventArgs e)
        {
            Button senderBtn = (Button)sender;
            Size currentSize = senderBtn.Size;
            Point currentLoc = senderBtn.Location;

            senderBtn.Size = new Size(currentSize.Width + 4, currentSize.Height + 4);
            senderBtn.Location = new Point(currentLoc.X - 2, currentLoc.Y - 2);
        }
        private void linkButtonLeave(object sender, EventArgs e)
        {
            Button senderBtn = (Button)sender;
            Size currentSize = senderBtn.Size;
            Point currentLoc = senderBtn.Location;

            senderBtn.Size = new Size(currentSize.Width - 4, currentSize.Height - 4);
            senderBtn.Location = new Point(currentLoc.X + 2, currentLoc.Y + 2);
        }

        private void pnlSettingsSide_MouseLeave(object sender, EventArgs e)
        {
            if (currentTab != 4)
            {
                pnlSettingsSide.BackColor = Color.FromArgb(67, 67, 67);
                pnlSettingsB.BackColor = Color.FromArgb(114, 114, 114);
            }
        }

        private void pnlSettingsSide_MouseEnter(object sender, EventArgs e)
        {
            pnlSettingsSide.BackColor = Color.FromArgb(229, 148, 77);
            pnlSettingsB.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void pnlTrainerSide_MouseLeave(object sender, EventArgs e)
        {
            if (currentTab != 1)
            {
                pnlTrainerSide.BackColor = Color.FromArgb(67, 67, 67);
                pnlTrainerB.BackColor = Color.FromArgb(114, 114, 114);
            }
        }

        private void pnlTrainerSide_MouseEnter(object sender, EventArgs e)
        {
            pnlTrainerSide.BackColor = Color.FromArgb(229, 148, 77);
            pnlTrainerB.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void pnlLevelsSide_MouseLeave(object sender, EventArgs e)
        {
            if (currentTab != 2)
            {
                pnlLevelsSide.BackColor = Color.FromArgb(67, 67, 67);
                pnlLevelsB.BackColor = Color.FromArgb(114, 114, 114);
            }
        }

        private void pnlLevelsSide_MouseEnter(object sender, EventArgs e)
        {
            pnlLevelsSide.BackColor = Color.FromArgb(229, 148, 77);
            pnlLevelsB.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void pnlInfoSide_MouseLeave(object sender, EventArgs e)
        {
            if (currentTab != 3)
            {
                pnlInfoSide.BackColor = Color.FromArgb(67, 67, 67);
                pnlInfoB.BackColor = Color.FromArgb(114, 114, 114);
            }
        }

        private void pnlInfoSide_MouseEnter(object sender, EventArgs e)
        {
            pnlInfoSide.BackColor = Color.FromArgb(229, 148, 77);
            pnlInfoB.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void packClickMain(object sender, EventArgs e)
        {
            pnlBackToPacks.Visible = true;
            pnlPacks.Visible = false;
            pnlMainLevels.Visible = true;
        }

        private void packClickLost(object sender, EventArgs e)
        {
            pnlBackToPacks.Visible = true;
            pnlPacks.Visible = false;
            pnlLostorbit.Visible = true;
        }

        private void packClickAfter(object sender, EventArgs e)
        {
            pnlBackToPacks.Visible = true;
            pnlPacks.Visible = false;
            pnlAftermath.Visible = true;
        }

        private void backToPacks(object sender, EventArgs e)
        {
            foreach (var panel in pnlLevels.Controls.OfType<Panel>().Where(pnl => (string)pnl.Tag == "tagAllPacks"))
                panel.Visible = false;
            pnlBackToPacks.Visible = false;
            pnlPacks.Visible = true;
        }

        private void chapterClick(object sender, EventArgs e)
        {
            Type senderType = sender.GetType();
            string chapterNum;

            if (senderType.Equals(typeof(Panel)))
            {
                chapterNum = Regex.Replace((sender as Panel).Name, @"[^\d]", "");
            } else
            {
                chapterNum = Regex.Replace((sender as Label).Name, @"[^\d]", "");
            }

            if (currentChapterTab == chapterNum)
                return;

            foreach (var panel in pnlMainLevels.Controls.OfType<Panel>().Where(pnl => pnl.Name.Contains("pnlChapter")))
            {

                if (panel.Name.Contains("B"))
                {
                    panel.BackColor = Color.FromArgb(114, 114, 114);
                }
                else
                {
                    panel.BackColor = Color.FromArgb(67, 67, 67);
                }

                string pnlNum = Regex.Replace(panel.Name, @"[^\d]", "");
                if (pnlNum != chapterNum)
                {
                    continue;
                }

                if (panel.Name.Contains("B"))
                {
                    panel.BackColor = Color.FromArgb(239, 239, 239);
                }
                else
                {
                    panel.BackColor = Color.FromArgb(229, 148, 77);
                }

                foreach (var panel2 in pnlMainLevels.Controls.OfType<Panel>().Where(pnl => pnl.Name.Contains("pnlMainCh")))
                {
                    string pnl2Num = Regex.Replace(panel2.Name, @"[^\d]", "");

                    if (pnl2Num != chapterNum)
                    {
                        panel2.Visible = false;
                    } else
                    {
                        panel2.Visible = true;
                    }
                }
            }
            currentChapterTab = chapterNum;
        }

        private void levelClick(object sender, EventArgs e)
        {
            Type senderType = sender.GetType();
            string chapterNum = "", chapterName = "";

            if (senderType.Equals(typeof(Panel)))
            {
                foreach (var label in (sender as Panel).Controls.OfType<Label>().Where(lbl => lbl.Name.Contains(chapterNum)))
                {
                    chapterName = label.Text;
                }

                chapterNum = Regex.Replace((sender as Panel).Name, @"[^\d]", "");
            }
            else
            {
                chapterNum = Regex.Replace((sender as Label).Name, @"[^\d]", "");
                chapterName = (sender as Label).Text;
            }

            lblSelectedChapter.Text = chapterName;

            pnlMainPreview.BackgroundImage = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "icons/Preview/" + chapterNum + ".jpg"));
            pnlMainPreview.Visible = true;
            selectedLevel = chapterName;
            pnlApplyLvl.Visible = true;
            GC.Collect();
        }

        private void pnlApplyLvl_Click(object sender, EventArgs e)
        {
            /*trainer.setSave(selectedLevel);*/
            Console.WriteLine(selectedLevel);
        }

        private void tabChangeClick(object sender, EventArgs e)
        {
            foreach (var panel in pnlSide.Controls.OfType<Panel>().Where(pnl => (string)pnl.Tag == "tagSidePanel"))
                panel.BackColor = Color.FromArgb(67, 67, 67);
            foreach (var panel in pnlSide.Controls.OfType<Panel>().Where(pnl => (string)pnl.Tag == "tagSidePanelB"))
                panel.BackColor = Color.FromArgb(114, 114, 114);

            Type senderType = sender.GetType();

            if (senderType.Equals(typeof(Panel)))
            {
                if ((sender as Panel).Name.Contains("Trainer"))
                {
                    if (currentTab != 1)
                    {
                        tabChange(1);
                    }
                }
                else if ((sender as Panel).Name.Contains("Levels"))
                {
                    if (currentTab != 2)
                    {
                        tabChange(2);
                    }
                }
                else if ((sender as Panel).Name.Contains("Info"))
                {
                    if (currentTab != 3)
                    {
                        tabChange(3);
                    }
                }
                else if ((sender as Panel).Name.Contains("Settings"))
                {
                    if (currentTab != 4)
                    {
                        tabChange(4);
                    }
                }
            }
            else
            {
                if ((sender as Label).Name.Contains("Trainer"))
                {
                    if (currentTab != 1)
                    {
                        tabChange(1);
                    }
                }
                else if ((sender as Label).Name.Contains("Levels"))
                {
                    if (currentTab != 2)
                    {
                        tabChange(2);
                    }
                }
                else if ((sender as Label).Name.Contains("Info"))
                {
                    if (currentTab != 3)
                    {
                        tabChange(3);
                    }
                }
                else if ((sender as Label).Name.Contains("Settings"))
                {
                    if (currentTab != 4)
                    {
                        tabChange(4);
                    }
                }
            }
        }

        private void tabChange(int newTabNum)
        {
            switch(newTabNum)
            {
                case 1:
                    currentTab = 1;
                    pnlTrainerSide.BackColor = Color.FromArgb(229, 148, 77);
                    pnlTrainerB.BackColor = Color.FromArgb(239, 239, 239);
                    foreach (var panel in Controls.OfType<Panel>().Where(pnl => (string)pnl.Tag == "tagMainPnls"))
                        panel.Visible = false;
                    pnlTrainer.Visible = true;
                    break;
                case 2:
                    currentTab = 2;
                    pnlLevelsSide.BackColor = Color.FromArgb(229, 148, 77);
                    pnlLevelsB.BackColor = Color.FromArgb(239, 239, 239);
                    foreach (var panel in Controls.OfType<Panel>().Where(pnl => (string)pnl.Tag == "tagMainPnls"))
                        panel.Visible = false;
                    pnlLevels.Visible = true;
                    break;
                case 3:
                    currentTab = 3;
                    pnlInfoSide.BackColor = Color.FromArgb(229, 148, 77);
                    pnlInfoB.BackColor = Color.FromArgb(239, 239, 239);
                    foreach (var panel in Controls.OfType<Panel>().Where(pnl => (string)pnl.Tag == "tagMainPnls"))
                        panel.Visible = false;
                    pnlInfo.Visible = true;
                    break;
                case 4:
                    currentTab = 4;
                    pnlSettingsSide.BackColor = Color.FromArgb(229, 148, 77);
                    pnlSettingsB.BackColor = Color.FromArgb(239, 239, 239);
                    foreach (var panel in Controls.OfType<Panel>().Where(pnl => (string)pnl.Tag == "tagMainPnls"))
                        panel.Visible = false;
                    pnlSettings.Visible = true;
                    break;
            }
        }
    }
}
