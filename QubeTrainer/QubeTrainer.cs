using System;
using System.Timers;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Virtua_Cop_2Trainer;
using System.Runtime.InteropServices;
using System.IO;


using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Media;

namespace QubeTrainer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        VAMemory vam;
        Process GameProcess;
        Process[] processes;
        IntPtr BaseX, BaseY, BaseZ, BaseMX, BaseMY, BaseMZ, BaseSX, BaseSY, BaseSZ, BaseAX, BaseAY, BaseArmsRotY;
        Int32[] valXOff = { 0x58, 0x340, 0x18, 0x100, 0x190 };
        Int32[] valYOff = { 0x58, 0x340, 0x18, 0x100, 0x198 };
        Int32[] valZOff = { 0x58, 0x340, 0x18, 0x100, 0x194 };
        Int32[] valMXOff = { 0x58, 0x340, 0x18, 0x528, 0x670 };
        Int32[] valMYOff = { 0x58, 0x340, 0x18, 0x528, 0x678 };
        Int32[] valMZOff = { 0x58, 0x340, 0x18, 0x528, 0x674 };
        Int32[] valSXOff = { 0x58, 0x2A0, 0x10, 0x80, 0x104 };
        Int32[] valSYOff = { 0x58, 0x2A0, 0x10, 0x80, 0x10C };
        Int32[] valSZOff = { 0x58, 0x2A0, 0x10, 0x80, 0x108 };
        Int32[] valAXOff = { 0x58, 0x340, 0x18, 0x100, 0x16C };
        Int32[] valAYOff = { 0x58, 0x3A0, 0x708, 0x100, 0x168 };
        Int32[] valArmsRotYOff = { 0x58, 0x340, 0x10, 0x100, 0x170 };

        float valX, valY, valZ, valMX, valMY, valMZ, valSX, valSY, valSZ, valAX, valAY, valLockX, valLockY, valLockZ, valLockSX, valLockSY, valLockSZ, valStoreX, valStoreY, valStoreZ;
        float valXOld, valYOld, valZOld, valMXOld, valMYOld, valMZOld, valSXOld, valSYOld, valSZOld, valAXOld, valAYOld;

        int checkCounter = 0;
        int turnValue;

        bool lockedX = false;
        bool lockedY = false;
        bool lockedZ = false;
        bool lockedSX = false;
        bool lockedSY = false;
        bool lockedSZ = false;
        bool moonjump = false;
        bool singleJump = false;
        bool lowGravity = false;
        bool superSpeed = false;
        bool flyMode = false;
        bool armsHidden = false;
        bool levelsOpen = false;

        List<KeyboardHook.VK> currentKeys = new List<KeyboardHook.VK>();

        bool connected = false;

        private static System.Timers.Timer aInterval;

        string[] fileNames = { "MainSaveGame.sav", "MainStatsSaveGame.sav", "MainUnlockedLevels.sav" };
        string sourceVS = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\VaultSave");
        string source;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QUBE\\Saved\\SaveGames\\");

        private void Form1_Load(object sender, EventArgs e)
        {
            KeyboardHook.CreateHook(KeyReader);

            ToolTip toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 10;
            toolTip1.ReshowDelay = 10;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.label1, "If the marker positions aren't right, go to the main menu and continue the game to fix them!");
        }

        public void KeyReader(IntPtr wParam, IntPtr lParam)
        {
            if (connected && wParam.ToInt32() == 0x100) //WM_KEYDOWN
            {
                KeyboardHook.VK key = (KeyboardHook.VK)Marshal.ReadInt32(lParam);
                switch (key) //Global Hotkeys
                {
                    case KeyboardHook.VK.VK_F1:
                        toggleMoonjump();
                        break;
                    case KeyboardHook.VK.VK_F2:
                        toggleSuperspeed();
                        break;
                    case KeyboardHook.VK.VK_F3:
                        toggleLowGravity();
                        break;
                    case KeyboardHook.VK.VK_F4:
                        storePosition();
                        break;
                    case KeyboardHook.VK.VK_F5:
                        restorePosition();
                        break;
                    case KeyboardHook.VK.VK_F6:
                        teleportToMarker();
                        break;
                    case KeyboardHook.VK.VK_F7:
                        lockX();
                        break;
                    case KeyboardHook.VK.VK_F8:
                        lockY();
                        break;
                    case KeyboardHook.VK.VK_F9:
                        lockZ();
                        break;
                    case KeyboardHook.VK.VK_F10:
                        toggleFlyMode();
                        break;
                        //F11 reserved for QUBE fullscreen toggling
                    case KeyboardHook.VK.VK_F12:
                        toggleArmsVisible();
                        break;
                    default:
                        if (!currentKeys.Contains(key))
                        {
                            currentKeys.Add(key);
                        }
                        break;
                }
            }
            else if (connected && wParam.ToInt32() == 0x101) //WM_KEYUP
            {
                KeyboardHook.VK key = (KeyboardHook.VK)Marshal.ReadInt32(lParam);
                switch (key) //Global Hotkeys
                {
                    default:
                        currentKeys.Remove(key);
                        break;
                }
            }
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

        private void toggleArmsVisible()
        {
            armsHidden = !armsHidden;
            if (armsHidden)
            {
                vam.WriteFloat(BaseArmsRotY, -180);
            }
            if (!armsHidden)
            {
                vam.WriteFloat(BaseArmsRotY, 0);
            }
            btnHideArms.Text = armsHidden ? "Hide Arms (On)" : "Hide Arms (Off)";
        }

        private void toggleFlyMode()
        {
            flyMode = !flyMode;
            valLockY = valY;
            valLockX = valX;
            valLockZ = valZ;

            btnFlyMode.Text = flyMode ? "Fly Mode (On)" : "Fly Mode (Off)";
        }

        private void lockX()
        {
            if (lockedX)
            {
                lockedX = false;
                SLock1.Text = "Lock";
            }
            else
            {
                valLockX = valX;
                valLockSX = 0f;

                lockedX = true;
                SLock1.Text = "Unlock";
            }
        }

        private void lockY()
        {
            if (lockedY)
            {
                lockedY = false;
                SLock2.Text = "Lock";
            }
            else
            {
                valLockY = valY;
                valLockSY = 0f;

                lockedY = true;
                SLock2.Text = "Unlock";
            }
        }

        private void lockZ() {
            if (lockedZ)
            {
                lockedZ = false;
                SLock3.Text = "Lock";
            }
            else
            {
                valLockZ = valZ;
                valLockSZ = 0f;

                lockedZ = true;
                SLock3.Text = "Unlock";
            }
        }

        private void lockSX()
        {
            if (lockedSX)
            {
                lockedSX = false;
                SLock4.Text = "Lock";
            }
            else
            {
                valLockSX = valSX;

                lockedSX = true;
                SLock4.Text = "Unlock";
            }
        }

        private void lockSY()
        {
            if (lockedSY)
            {
                lockedSY = false;
                SLock5.Text = "Lock";
            }
            else
            {
                valLockSY = valSY;

                lockedSY = true;
                SLock5.Text = "Unlock";
            }
        }

        private void lockSZ()
        {
            if (lockedSZ)
            {
                lockedSZ = false;
                SLock6.Text = "Lock";
            }
            else
            {
                valLockSZ = valSZ;

                lockedSZ = true;
                SLock6.Text = "Unlock";
            }
        }

        private void teleportToMarker()
        {
            vam.WriteFloat(BaseX, valMX);
            vam.WriteFloat(BaseY, valMY);
            vam.WriteFloat(BaseZ, valMZ);
        }

        private void restorePosition()
        {
            vam.WriteFloat(BaseX, valStoreX);
            vam.WriteFloat(BaseY, valStoreY);
            vam.WriteFloat(BaseZ, valStoreZ);
        }

        private void storePosition()
        {
            valStoreX = valX;
            valStoreY = valY;
            valStoreZ = valZ;
        }

        private void toggleLowGravity()
        {
            if (lowGravity)
            {
                btnLowGravity.Text = "Low Gravity (Off)";
                lowGravity = false;
            }
            else
            {
                btnLowGravity.Text = "Low Gravity (On)";
                lowGravity = true;
            }
        }

        private void toggleSuperspeed()
        {
            if (superSpeed)
            {
                btnSuperSpeed.Text = "Super Speed (Off)";
                superSpeed = false;

                vam.WriteFloat(BaseSX, 0f);
                vam.WriteFloat(BaseSZ, 0f);
            }
            else
            {
                btnSuperSpeed.Text = "Super Speed (On)";
                superSpeed = true;
            }
        }

        private void toggleMoonjump()
        {
            if (moonjump)
            {
                btnMoonjump.Text = "Moonjump (Off)";
                moonjump = false;
            }
            else
            {
                btnMoonjump.Text = "Moonjump (On)";
                moonjump = true;
            }
        }

        private void connect()
        {
            processes = Process.GetProcessesByName("QUBE-Win64-Shipping");

            if (processes.Length > 0)
            {
                btnConnect.Text = "Disconnect";

                foreach (var button in this.Controls.OfType<Button>().Where(btn => (string)btn.Tag != "tagNoEnable"))
                {
                    button.Enabled = true;
                }

                GameProcess = processes[0];
                vam = new VAMemory("QUBE-Win64-Shipping");

                setupAddresses();

                SetInterval();
                connected = true;
            }
            else
            {
                MessageBox.Show("Could not find an open QUBE 2 process!", "Error Finding Process", MessageBoxButtons.OK);
            }
        }

        private void setupAddresses()
        {
            BaseX = BaseY = BaseZ = BaseMX = BaseMY = BaseMZ = BaseSX = BaseSY = BaseSZ = BaseAX = BaseAY = BaseArmsRotY = GameProcess.MainModule.BaseAddress + 0x0290B008;

            for (int i = 0; i < 5; i++)
            {
                BaseX = IntPtr.Add((IntPtr)vam.ReadInt64(BaseX), valXOff[i]);
                BaseY = IntPtr.Add((IntPtr)vam.ReadInt64(BaseY), valYOff[i]);
                BaseZ = IntPtr.Add((IntPtr)vam.ReadInt64(BaseZ), valZOff[i]);
                BaseMX = IntPtr.Add((IntPtr)vam.ReadInt64(BaseMX), valMXOff[i]);
                BaseMY = IntPtr.Add((IntPtr)vam.ReadInt64(BaseMY), valMYOff[i]);
                BaseMZ = IntPtr.Add((IntPtr)vam.ReadInt64(BaseMZ), valMZOff[i]);
                BaseSX = IntPtr.Add((IntPtr)vam.ReadInt64(BaseSX), valSXOff[i]);
                BaseSY = IntPtr.Add((IntPtr)vam.ReadInt64(BaseSY), valSYOff[i]);
                BaseSZ = IntPtr.Add((IntPtr)vam.ReadInt64(BaseSZ), valSZOff[i]);
                BaseAX = IntPtr.Add((IntPtr)vam.ReadInt64(BaseAX), valAXOff[i]);
                BaseAY = IntPtr.Add((IntPtr)vam.ReadInt64(BaseAY), valAYOff[i]);
                BaseArmsRotY = IntPtr.Add((IntPtr)vam.ReadInt64(BaseArmsRotY), valArmsRotYOff[i]);
            }
        }

        private void disconnect()
        {
            btnConnect.Text = "Connect";

            foreach (var button in this.Controls.OfType<Button>().Where(btn => (string)btn.Tag != "tagNoDisable"))
            {
                button.Enabled = false;
            }

            aInterval.Stop();
            aInterval.Dispose();
            for (int i = 0; i < 10; i++)
            {
                SetValue("", i);
            }
            connected = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                connect();
            } else
            {
                disconnect();
            }
        }
        
        delegate void SetTextCallback(string text, int type);
        private void SetValue(string text, int type)
        {
            switch (type)
            {
                case 1:
                    valPosX.Invoke(new Action(() => {
                        valPosX.Text = text;
                    }));
                    break;
                case 2:
                    valPosY.Invoke(new Action(() => {
                        valPosY.Text = text;
                    }));
                    break;
                case 3:
                    valPosZ.Invoke(new Action(() => {
                        valPosZ.Text = text;
                    }));
                    break;
                case 4:
                    valMarkX.Invoke(new Action(() => {
                        valMarkX.Text = text;
                    }));
                    break;
                case 5:
                    valMarkY.Invoke(new Action(() => {
                        valMarkY.Text = text;
                    }));
                    break;
                case 6:
                    valMarkZ.Invoke(new Action(() => {
                        valMarkZ.Text = text;
                    }));
                    break;
                case 7:
                    valSpeedX.Invoke(new Action(() => {
                        valSpeedX.Text = text;
                    }));
                    break;
                case 8:
                    valAngleX.Invoke(new Action(() => {
                        valAngleX.Text = text;
                    }));
                    break;
                case 9:
                    valAngleY.Invoke(new Action(() => {
                        valAngleY.Text = text;
                    }));
                    break;
                case 10:
                    if (this.valSpeedY.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valSpeedY.Text = text;
                    }
                    break;
                case 11:
                    if (this.valSpeedZ.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valSpeedZ.Text = text;
                    }
                    break;
            }
        }

        private void SetInterval()
        {
            aInterval = new System.Timers.Timer(20);
            aInterval.Elapsed += IntervalEvent;
            aInterval.AutoReset = true;
            aInterval.Enabled = true;
        }
        
        private void IntervalEvent(Object source, ElapsedEventArgs e)
        {
            if (!connected)
            {
                return;
            }

            valX = vam.ReadFloat(BaseX);
            SetValue(valX.ToString(), 1);
            valY = vam.ReadFloat(BaseY);
            SetValue(valY.ToString(), 2);
            valZ = vam.ReadFloat(BaseZ);
            SetValue(valZ.ToString(), 3);

            valMX = vam.ReadFloat(BaseMX);
            SetValue(valMX.ToString(), 4);
            valMY = vam.ReadFloat(BaseMY);
            SetValue(valMY.ToString(), 5);
            valMZ = vam.ReadFloat(BaseMZ);
            SetValue(valMZ.ToString(), 6);

            valSX = vam.ReadFloat(BaseSX);
            SetValue(valSX.ToString(), 7);
            valSY = vam.ReadFloat(BaseSY);
            SetValue(valSY.ToString(), 10);
            valSZ = vam.ReadFloat(BaseSZ);
            SetValue(valSZ.ToString(), 11);

            valAX = vam.ReadFloat(BaseAX);
            SetValue(valAX.ToString(), 8);
            valAY = vam.ReadFloat(BaseAY);
            SetValue(valAY.ToString(), 9);

            if (lockedX)
            {
                vam.WriteFloat(BaseX, valLockX);
                vam.WriteFloat(BaseSX, valLockSX);
            }

            if (lockedY)
            {
                vam.WriteFloat(BaseY, valLockY);
                vam.WriteFloat(BaseSY, valLockSY);
            }

            if (lockedZ)
            {
                vam.WriteFloat(BaseZ, valLockZ);
                vam.WriteFloat(BaseSZ, valLockSZ);
            }

            if (lockedSX)
            {
                vam.WriteFloat(BaseSX, valLockSX);
            }

            if (lockedSY)
            {
                vam.WriteFloat(BaseSY, valLockSY);
            }

            if (lockedSZ)
            {
                vam.WriteFloat(BaseSZ, valLockSZ);
            }

            if (moonjump && !singleJump)
            {
                if (valSY > 0)
                {
                    vam.WriteFloat(BaseSY, 5000f);
                    singleJump = true;
                }
            }

            if (valSY == 0)
            {
                singleJump = false;
            }

            if (singleJump)
            {
                if (valSY < -2880)
                {
                    vam.WriteFloat(BaseSY, -2830);
                }
            }

            if (superSpeed)
            {
                if (currentKeys.Contains(KeyboardHook.VK.VK_W) && currentKeys.Contains(KeyboardHook.VK.VK_D))
                {
                    turnValue = 45;
                }
                else if (currentKeys.Contains(KeyboardHook.VK.VK_W) && currentKeys.Contains(KeyboardHook.VK.VK_A))
                {
                    turnValue = 315;
                }
                else if (currentKeys.Contains(KeyboardHook.VK.VK_S) && currentKeys.Contains(KeyboardHook.VK.VK_D))
                {
                    turnValue = 135;
                }
                else if (currentKeys.Contains(KeyboardHook.VK.VK_S) && currentKeys.Contains(KeyboardHook.VK.VK_A))
                {
                    turnValue = 225;
                }
                else if (currentKeys.Contains(KeyboardHook.VK.VK_W))
                {
                    turnValue = 0;
                }
                else if (currentKeys.Contains(KeyboardHook.VK.VK_A))
                {
                    turnValue = 270;
                }
                else if (currentKeys.Contains(KeyboardHook.VK.VK_S))
                {
                    turnValue = 180;
                }
                else if (currentKeys.Contains(KeyboardHook.VK.VK_D))
                {
                    turnValue = 90;
                }

                if (currentKeys.Contains(KeyboardHook.VK.VK_W) || currentKeys.Contains(KeyboardHook.VK.VK_A) || currentKeys.Contains(KeyboardHook.VK.VK_S) || currentKeys.Contains(KeyboardHook.VK.VK_D))
                {
                    vam.WriteFloat(BaseSX, (float)Math.Cos((Math.PI / 180) * (valAX + turnValue)) * 5000);
                    vam.WriteFloat(BaseSZ, (float)Math.Sin((Math.PI / 180) * (valAX + turnValue)) * 5000);
                }
                else
                {
                    if (!lockedSX)
                    {
                        vam.WriteFloat(BaseSX, 0);
                    }
                    if (!lockedSZ)
                    {
                        vam.WriteFloat(BaseSZ, 0);
                    }
                }
            }

            if (flyMode)
            {
                if (currentKeys.Contains(KeyboardHook.VK.VK_SPACE))
                {
                    valLockY += 100;
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_LSHIFT))
                {
                    valLockY -= 100;
                }
                vam.WriteFloat(BaseSX, 0);
                vam.WriteFloat(BaseSY, 0);
                vam.WriteFloat(BaseSZ, 0);

                if (currentKeys.Contains(KeyboardHook.VK.VK_W))
                {
                    valX = (((float)Math.Cos((Math.PI / 180) * valAX) * 100) + valX);
                    valZ = (((float)Math.Sin((Math.PI / 180) * valAX) * 100) + valZ);
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_A))
                {
                    valX = (((float)Math.Sin((Math.PI / 180) * valAX) * 100) + valX);
                    valZ = (((float)Math.Cos((Math.PI / 180) * valAX) * -100) + valZ);
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_S))
                {
                    valX = (((float)Math.Cos((Math.PI / 180) * valAX) * -100) + valX);
                    valZ = (((float)Math.Sin((Math.PI / 180) * valAX) * -100) + valZ);
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_D))
                {
                    valX = (((float)Math.Sin((Math.PI / 180) * valAX) * -100) + valX);
                    valZ = (((float)Math.Cos((Math.PI / 180) * valAX) * 100) + valZ);
                }

                vam.WriteFloat(BaseX, valX);
                vam.WriteFloat(BaseZ, valZ);

                vam.WriteFloat(BaseY, valLockY);
            }

            if (lowGravity && valSY <= -500)
            {
                vam.WriteFloat(BaseSY, -400);
            }

            if (valX == valXOld && valY == valYOld && valZ == valZOld && valMX == valMXOld && valMY == valMYOld && valMZ == valMZOld && valSX == valSXOld && valSY == valSYOld && valSZ == valSZOld && valAX == valAXOld && valAY == valAYOld)
            {
                checkCounter++;
            }

            if (checkCounter >= 150)
            {
                checkCounter = 0;

                setupAddresses();
            }

            valXOld = valX;
            valYOld = valY;
            valZOld = valZ;
            valMXOld = valMX;
            valMYOld = valMY;
            valMZOld = valMZ;
            valSXOld = valSX;
            valSYOld = valSY;
            valSZOld = valSZ;
            valAXOld = valAX;
            valAYOld = valAY;
        }

        private void valMarkY_MouseDown(object sender, MouseEventArgs e)
        {
            if (connected)
            {
                using (Form2 form2 = new Form2(valMY))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        vam.WriteFloat(BaseMY, float.Parse(form2.getNewVal()));
                    }
                    form2.Close();
                }
            }
        }

        private void valMarkZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (connected)
            {
                using (Form2 form2 = new Form2(valMZ))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        vam.WriteFloat(BaseMZ, float.Parse(form2.getNewVal()));
                    }
                    form2.Close();
                }
            }
        }

        private void valMarkX_MouseDown(object sender, MouseEventArgs e)
        {
            if (connected)
            {
                using (Form2 form2 = new Form2(valMX))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        vam.WriteFloat(BaseMX, float.Parse(form2.getNewVal()));
                    }
                    form2.Close();
                }
            }
        }

        private void valPosZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (connected)
            {
                using (Form2 form2 = new Form2(valZ))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        valLockZ = float.Parse(form2.getNewVal());
                        vam.WriteFloat(BaseZ, float.Parse(form2.getNewVal()));
                        valLockZ = float.Parse(form2.getNewVal());
                    }
                    form2.Close();
                }
            }
        }

        private void valPosY_MouseDown(object sender, MouseEventArgs e)
        {
            if (connected)
            {
                using (Form2 form2 = new Form2(valY))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        valLockY = float.Parse(form2.getNewVal());
                        vam.WriteFloat(BaseY, float.Parse(form2.getNewVal()));
                        valLockY = float.Parse(form2.getNewVal());
                    }
                    form2.Close();
                }
            }
        }

        private void valPosX_MouseDown(object sender, MouseEventArgs e)
        {
            if (connected)
            {
                using (Form2 form2 = new Form2(valX))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        valLockX = float.Parse(form2.getNewVal());
                        vam.WriteFloat(BaseX, float.Parse(form2.getNewVal()));
                        valLockX = float.Parse(form2.getNewVal());
                    }
                    form2.Close();
                }
            }
        }

        private void btnMoonjump_Click(object sender, EventArgs e)
        {
            toggleMoonjump();
        }

        private void btnSuperSpeed_Click(object sender, EventArgs e)
        {
            toggleSuperspeed();
        }

        private void btnLowGravity_Click(object sender, EventArgs e)
        {
            toggleLowGravity();
        }

        private void btnTeleportToMarker_Click(object sender, EventArgs e)
        {
            teleportToMarker();
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            storePosition();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            restorePosition();
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

        private void SLock2_Click(object sender, EventArgs e)
        {
            lockY();
        }

        private void SLock1_Click(object sender, EventArgs e)
        {
            lockX();
        }

        private void SLock3_Click(object sender, EventArgs e)
        {
            lockZ();
        }

        private void SLock4_Click(object sender, EventArgs e)
        {
            lockSX();
        }

        private void SLock5_Click(object sender, EventArgs e)
        {
            lockSY();
        }

        private void SLock6_Click(object sender, EventArgs e)
        {
            lockSZ();
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            toggleDarkMode();
        }

        private void btnFlyMode_Click(object sender, EventArgs e)
        {
            toggleFlyMode();
        }

        private void btnHideArms_Click(object sender, EventArgs e)
        {
            toggleArmsVisible();
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
    }
}
