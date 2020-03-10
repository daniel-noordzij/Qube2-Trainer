using System;
using System.Timers;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Virtua_Cop_2Trainer;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;
using System.IO;

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
        IntPtr BaseX, BaseY, BaseZ, BaseMX, BaseMY, BaseMZ, BaseSX, BaseSY, BaseSZ, BaseAX, BaseAY;
        Int32[] valXOff = {0x58, 0x340, 0x18, 0x100, 0x190};
        Int32[] valYOff = {0x58, 0x340, 0x18, 0x100, 0x198};
        Int32[] valZOff = {0x58, 0x340, 0x18, 0x100, 0x194};
        Int32[] valMXOff = {0x58, 0x340, 0x18, 0x528, 0x670};
        Int32[] valMYOff = {0x58, 0x340, 0x18, 0x528, 0x678};
        Int32[] valMZOff = {0x58, 0x340, 0x18, 0x528, 0x674};
        Int32[] valSXOff = {0x58, 0x2A0, 0x10, 0x80, 0x104};
        Int32[] valSYOff = {0x58, 0x2A0, 0x10, 0x80, 0x10C};
        Int32[] valSZOff = {0x58, 0x2A0, 0x10, 0x80, 0x108};
        Int32[] valAXOff = {0x58, 0x340, 0x18, 0x100, 0x16C};
        Int32[] valAYOff = {0x58, 0x3A0, 0x708, 0x100, 0x168};

        float valX, valY, valZ, valMX, valMY, valMZ, valSX, valSY, valSZ, valAX, valAY, valLockX, valLockY, valLockZ, valLockSX, valLockSY, valLockSZ, valStoreX, valStoreY, valStoreZ;
        float valXOld, valYOld, valZOld, valMXOld, valMYOld, valMZOld, valSXOld, valSYOld, valSZOld, valAXOld, valAYOld;

        int checkCounter = 0;

        bool lockedX = false;
        bool lockedY = false;
        bool lockedZ = false;
        bool moonjump = false;
        bool singleJump = false;
        bool lowGravity = false;
        bool superSpeed = false;

        private static System.Timers.Timer aInterval;

        string[] fileNames = { "MainSaveGame.sav", "MainStatsSaveGame.sav", "MainUnlockedLevels.sav" };
        string source = Path.Combine(Directory.GetCurrentDirectory(), "VaultSave");
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
            int key = Marshal.ReadInt32(lParam);
            KeyboardHook.VK vk = (KeyboardHook.VK)key;
            String temp = "";

            switch (vk)
            {
                case KeyboardHook.VK.VK_F1: temp = "<-F1->";
                    break;
                case KeyboardHook.VK.VK_F2: temp = "<-F2->";
                    break;
                case KeyboardHook.VK.VK_F3: temp = "<-F3->";
                    break;
                case KeyboardHook.VK.VK_F4: temp = "<-F4->";
                    break;
                case KeyboardHook.VK.VK_F5: temp = "<-F5->";
                    break;
                case KeyboardHook.VK.VK_F6: temp = "<-F6->";
                    break;
                case KeyboardHook.VK.VK_F7: temp = "<-F7->";
                    break;
                case KeyboardHook.VK.VK_F8: temp = "<-F8->";
                    break;
                case KeyboardHook.VK.VK_F9: temp = "<-F9->";
                    break;
                case KeyboardHook.VK.VK_F10: temp = "<-F10->";
                    break;
                case KeyboardHook.VK.VK_F11: temp = "<-F11->";
                    break;
                case KeyboardHook.VK.VK_F12: temp = "<-F12->";
                    break;
                case KeyboardHook.VK.VK_NUMLOCK: temp = "<-numlock->";
                    break;
                case KeyboardHook.VK.VK_SCROLL: temp = "<-scroll>";
                    break;
                case KeyboardHook.VK.VK_LSHIFT: temp = "<-left shift->";
                    break;
                case KeyboardHook.VK.VK_RSHIFT: temp = "<-right shift->";
                    break;
                case KeyboardHook.VK.VK_LCONTROL: temp = "<-left control->";
                    break;
                case KeyboardHook.VK.VK_RCONTROL: temp = "<-right control->";
                    break;
                case KeyboardHook.VK.VK_SEPERATOR: temp = "|";
                    break;
                case KeyboardHook.VK.VK_SUBTRACT: temp = "-";
                    break;
                case KeyboardHook.VK.VK_ADD: temp = "+";
                    break;
                case KeyboardHook.VK.VK_DECIMAL: temp = ".";
                    break;
                case KeyboardHook.VK.VK_DIVIDE: temp = "/";
                    break;
                case KeyboardHook.VK.VK_NUMPAD0: temp = "0";
                    break;
                case KeyboardHook.VK.VK_NUMPAD1: temp = "1";
                    break;
                case KeyboardHook.VK.VK_NUMPAD2: temp = "2";
                    break;
                case KeyboardHook.VK.VK_NUMPAD3: temp = "3";
                    break;
                case KeyboardHook.VK.VK_NUMPAD4: temp = "4";
                    break;
                case KeyboardHook.VK.VK_NUMPAD5: temp = "5";
                    break;
                case KeyboardHook.VK.VK_NUMPAD6: temp = "6";
                    break;
                case KeyboardHook.VK.VK_NUMPAD7: temp = "7";
                    break;
                case KeyboardHook.VK.VK_NUMPAD8: temp = "8";
                    break;
                case KeyboardHook.VK.VK_NUMPAD9: temp = "9";
                    break;
                case KeyboardHook.VK.VK_Q: temp = "q";
                    break;
                case KeyboardHook.VK.VK_W: temp = "w";
                    break;
                case KeyboardHook.VK.VK_E: temp = "e";
                    break;
                case KeyboardHook.VK.VK_R: temp = "r";
                    break;
                case KeyboardHook.VK.VK_T: temp = "t";
                    break;
                case KeyboardHook.VK.VK_Y: temp = "y";
                    break;
                case KeyboardHook.VK.VK_U: temp = "u";
                    break;
                case KeyboardHook.VK.VK_I: temp = "i";
                    break;
                case KeyboardHook.VK.VK_O: temp = "o";
                    break;
                case KeyboardHook.VK.VK_P: temp = "p";
                    break;
                case KeyboardHook.VK.VK_A: temp = "a";
                    break;
                case KeyboardHook.VK.VK_S: temp = "s";
                    break;
                case KeyboardHook.VK.VK_D: temp = "d";
                    break;
                case KeyboardHook.VK.VK_F: temp = "f";
                    break;
                case KeyboardHook.VK.VK_G: temp = "g";
                    break;
                case KeyboardHook.VK.VK_H: temp = "h";
                    break;
                case KeyboardHook.VK.VK_J: temp = "j";
                    break;
                case KeyboardHook.VK.VK_K: temp = "k";
                    break;
                case KeyboardHook.VK.VK_L: temp = "l";
                    break;
                case KeyboardHook.VK.VK_Z: temp = "z";
                    break;
                case KeyboardHook.VK.VK_X: temp = "x";
                    break;
                case KeyboardHook.VK.VK_C: temp = "c";
                    break;
                case KeyboardHook.VK.VK_V: temp = "v";
                    break;
                case KeyboardHook.VK.VK_B: temp = "b";
                    break;
                case KeyboardHook.VK.VK_N: temp = "n";
                    break;
                case KeyboardHook.VK.VK_M: temp = "m";
                    break;
                case KeyboardHook.VK.VK_0: temp = "0";
                    break;
                case KeyboardHook.VK.VK_1: temp = "1";
                    break;
                case KeyboardHook.VK.VK_2: temp = "2";
                    break;
                case KeyboardHook.VK.VK_3: temp = "3";
                    break;
                case KeyboardHook.VK.VK_4: temp = "4";
                    break;
                case KeyboardHook.VK.VK_5: temp = "5";
                    break;
                case KeyboardHook.VK.VK_6: temp = "6";
                    break;
                case KeyboardHook.VK.VK_7: temp = "7";
                    break;
                case KeyboardHook.VK.VK_8: temp = "8";
                    break;
                case KeyboardHook.VK.VK_9: temp = "9";
                    break;
                case KeyboardHook.VK.VK_SNAPSHOT: temp = "<-print screen->";
                    break;
                case KeyboardHook.VK.VK_INSERT: temp = "<-insert->";
                    break;
                case KeyboardHook.VK.VK_DELETE: temp = "<-delete->";
                    break;
                case KeyboardHook.VK.VK_BACK: temp = "<-backspace->";
                    break;
                case KeyboardHook.VK.VK_TAB: temp = "<-tab->";
                    break;
                case KeyboardHook.VK.VK_RETURN: temp = "<-enter->";
                    break;
                case KeyboardHook.VK.VK_PAUSE: temp = "<-pause->";
                    break;
                case KeyboardHook.VK.VK_CAPITAL: temp = "<-caps lock->";
                    break;
                case KeyboardHook.VK.VK_ESCAPE: temp = "<-esc->";
                    break;
                case KeyboardHook.VK.VK_SPACE: temp = " "; //was <-space->
                    break;
                case KeyboardHook.VK.VK_PRIOR: temp = "<-page up->";
                    break;
                case KeyboardHook.VK.VK_NEXT: temp = "<-page down->";
                    break;
                case KeyboardHook.VK.VK_END: temp = "<-end->";
                    break;
                case KeyboardHook.VK.VK_HOME: temp = "<-home->";
                    break;
                case KeyboardHook.VK.VK_LEFT: temp = "<-arrow left->";
                    break;
                case KeyboardHook.VK.VK_UP: temp = "<-arrow up->";
                    break;
                case KeyboardHook.VK.VK_RIGHT: temp = "<-arrow right->";
                    break;
                case KeyboardHook.VK.VK_DOWN: temp = "<-arrow down->";
                    break;
                default: break;
            }

            #region Key triggers
            if (temp == "<-F1->" && btnConnect.Text == "Disconnect")
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

            if (temp == "<-F2->" && btnConnect.Text == "Disconnect")
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

            if (temp == "<-F3->" && btnConnect.Text == "Disconnect")
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

            if (temp == "<-F4->" && btnConnect.Text == "Disconnect")
            {
                valStoreX = valX;
                valStoreY = valY;
                valStoreZ = valZ;
            }

            if (temp == "<-F5->" && btnConnect.Text == "Disconnect")
            {
                vam.WriteFloat(BaseX, valStoreX);
                vam.WriteFloat(BaseY, valStoreY);
                vam.WriteFloat(BaseZ, valStoreZ);
            }

            if (temp == "<-F6->" && btnConnect.Text == "Disconnect")
            {
                vam.WriteFloat(BaseX, valMX);
                vam.WriteFloat(BaseY, valMY);
                vam.WriteFloat(BaseZ, valMZ);
            }

            if (temp == "<-F7->" && btnConnect.Text == "Disconnect")
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

            if (temp == "<-F8->" && btnConnect.Text == "Disconnect")
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

            if (temp == "<-F9->" && btnConnect.Text == "Disconnect")
            {
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

            #endregion
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
            {
                processes = Process.GetProcessesByName("QUBE-Win64-Shipping");

                if (processes.Length > 0)
                {
                    btnConnect.Text = "Disconnect";

                    SLock1.Enabled = true;
                    SLock2.Enabled = true;
                    SLock3.Enabled = true;
                    btnLowGravity.Enabled = true;
                    btnMoonjump.Enabled = true;
                    btnStore.Enabled = true;
                    btnRestore.Enabled = true;
                    btnSuperSpeed.Enabled = true;
                    btnTeleportToMarker.Enabled = true;
                    btnVaultSave.Enabled = true;

                    GameProcess = processes[0];
                    vam = new VAMemory("QUBE-Win64-Shipping");

                    BaseX = BaseY = BaseZ = BaseMX = BaseMY = BaseMZ = BaseSX = BaseSY = BaseSZ = BaseAX = BaseAY = GameProcess.MainModule.BaseAddress + 0x0290B008;

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
                    }

                    SetInterval();
                }
                else
                {
                    MessageBox.Show("Could not find an open QUBE 2 process!", "Error Finding Process", MessageBoxButtons.OK);

                }
            } else
            {
                btnConnect.Text = "Connect";

                SLock1.Enabled = false;
                SLock2.Enabled = false;
                SLock3.Enabled = false;
                btnLowGravity.Enabled = false;
                btnMoonjump.Enabled = false;
                btnStore.Enabled = false;
                btnRestore.Enabled = false;
                btnSuperSpeed.Enabled = false;
                btnTeleportToMarker.Enabled = false;
                btnVaultSave.Enabled = false;

                aInterval.Stop();
                aInterval.Dispose();
                for (int i = 0; i < 10; i++)
                {
                    SetValue("", i);
                }
            }
        }

        delegate void SetTextCallback(string text, int type);
        private void SetValue(string text, int type)
        {
            switch (type)
            {
                case 1:
                    if (this.valPosX.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valPosX.Text = text;
                    }
                    break;
                case 2:
                    if (this.valPosY.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valPosY.Text = text;
                    }
                    break;
                case 3:
                    if (this.valPosZ.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valPosZ.Text = text;
                    }
                    break;
                case 4:
                    if (this.valMarkX.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valMarkX.Text = text;
                    }
                    break;
                case 5:
                    if (this.valMarkY.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valMarkY.Text = text;
                    }
                    break;
                case 6:
                    if (this.valMarkZ.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valMarkZ.Text = text;
                    }
                    break;
                case 7:
                    if (this.valSpeed.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valSpeed.Text = text;
                    }
                    break;
                case 8:
                    if (this.valAngleX.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valAngleX.Text = text;
                    }
                    break;
                case 9:
                    if (this.valAngleY.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetValue);
                        this.Invoke(d, new object[] { text, type });
                    }
                    else
                    {
                        this.valAngleY.Text = text;
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
            valSZ = vam.ReadFloat(BaseSZ);

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

            valSY = vam.ReadFloat(BaseSY);

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
                switch (valAX)
                {
                    case float n when n <= 10f && n >= -10f:
                        vam.WriteFloat(BaseSX, 5000f);
                        break;
                    case float n when n < -170f || n > 170f:
                        vam.WriteFloat(BaseSX, -5000f);
                        break;
                    case float n when n > 80f && n <= 100f:
                        vam.WriteFloat(BaseSZ, 5000f);
                        break;
                    case float n when n < -80f && n >= -100f:
                        vam.WriteFloat(BaseSZ, -5000f);
                        break;
                    case float n when n > 10f && n <= 15f:
                        vam.WriteFloat(BaseSX, 4583f);
                        vam.WriteFloat(BaseSZ, 416f);
                        break;
                    case float n when n > 15f && n <= 30f:
                        vam.WriteFloat(BaseSX, 3750f);
                        vam.WriteFloat(BaseSZ, 1250f);
                        break;
                    case float n when n > 30f && n <= 45f:
                        vam.WriteFloat(BaseSX, 2916f);
                        vam.WriteFloat(BaseSZ, 2083f);
                        break;
                    case float n when n > 45f && n <= 60f:
                        vam.WriteFloat(BaseSX, 2083f);
                        vam.WriteFloat(BaseSZ, 2916f);
                        break;
                    case float n when n > 60f && n <= 75f:
                        vam.WriteFloat(BaseSX, 1250f);
                        vam.WriteFloat(BaseSZ, 3750f);
                        break;
                    case float n when n > 75f && n <= 80f:
                        vam.WriteFloat(BaseSX, 416f);
                        vam.WriteFloat(BaseSZ, 4583f);
                        break;
                    case float n when n > 100f && n <= 105f:
                        vam.WriteFloat(BaseSX, -416f);
                        vam.WriteFloat(BaseSZ, 4583f);
                        break;
                    case float n when n > 105f && n <= 120f:
                        vam.WriteFloat(BaseSX, -1250f);
                        vam.WriteFloat(BaseSZ, 3750f);
                        break;
                    case float n when n > 120f && n <= 135f:
                        vam.WriteFloat(BaseSX, -2083f);
                        vam.WriteFloat(BaseSZ, 2916f);
                        break;
                    case float n when n > 135f && n <= 150f:
                        vam.WriteFloat(BaseSX, -2916f);
                        vam.WriteFloat(BaseSZ, 2083f);
                        break;
                    case float n when n > 150f && n <= 165f:
                        vam.WriteFloat(BaseSX, -3750f);
                        vam.WriteFloat(BaseSZ, 1250f);
                        break;
                    case float n when n > 165f && n <= 170f:
                        vam.WriteFloat(BaseSX, -4583f);
                        vam.WriteFloat(BaseSZ, 416f);
                        break;
                    case float n when n < -10f && n >= -15f:
                        vam.WriteFloat(BaseSX, 4583f);
                        vam.WriteFloat(BaseSZ, -416f);
                        break;
                    case float n when n < -15f && n >= -30f:
                        vam.WriteFloat(BaseSX, 3750f);
                        vam.WriteFloat(BaseSZ, -1250f);
                        break;
                    case float n when n < -30f && n >= -45f:
                        vam.WriteFloat(BaseSX, 2916f);
                        vam.WriteFloat(BaseSZ, -2083f);
                        break;
                    case float n when n < -45f && n >= -60f:
                        vam.WriteFloat(BaseSX, 2083f);
                        vam.WriteFloat(BaseSZ, -2916f);
                        break;
                    case float n when n < -60f && n >= -75f:
                        vam.WriteFloat(BaseSX, 1250f);
                        vam.WriteFloat(BaseSZ, -3750f);
                        break;
                    case float n when n < -75f && n >= -80f:
                        vam.WriteFloat(BaseSX, 416f);
                        vam.WriteFloat(BaseSZ, -4583f);
                        break;
                    case float n when n < -100f && n >= -105f:
                        vam.WriteFloat(BaseSX, -416f);
                        vam.WriteFloat(BaseSZ, -4583f);
                        break;
                    case float n when n < -105f && n >= -120f:
                        vam.WriteFloat(BaseSX, -1250f);
                        vam.WriteFloat(BaseSZ, -3750f);
                        break;
                    case float n when n < -120f && n >= -135f:
                        vam.WriteFloat(BaseSX, -2083f);
                        vam.WriteFloat(BaseSZ, -2916f);
                        break;
                    case float n when n < -135f && n >= -150f:
                        vam.WriteFloat(BaseSX, -2916f);
                        vam.WriteFloat(BaseSZ, -2083f);
                        break;
                    case float n when n < -150f && n >= -165f:
                        vam.WriteFloat(BaseSX, -3750f);
                        vam.WriteFloat(BaseSZ, -1250f);
                        break;
                    case float n when n < -165f && n >= -170f:
                        vam.WriteFloat(BaseSX, -4583f);
                        vam.WriteFloat(BaseSZ, -416f);
                        break;
                }
            }

            if (valSY <= -500 && lowGravity == true)
            {
                vam.WriteFloat(BaseSY, -400);
            }

            if (valSX > valSZ)
            {
                SetValue(Math.Abs(valSX).ToString(), 7);
            }
            else
            {
                SetValue(Math.Abs(valSZ).ToString(), 7);
            }

            if (valX == valXOld && valY == valYOld && valZ == valZOld && valMX == valMXOld && valMY == valMYOld && valMZ == valMZOld && valSX == valSXOld && valSY == valSYOld && valSZ == valSZOld && valAX == valAXOld && valAY == valAYOld)
            {
                checkCounter++;
            }

            if (checkCounter >= 150)
            {
                checkCounter = 0;

                BaseX = BaseY = BaseZ = BaseMX = BaseMY = BaseMZ = BaseSX = BaseSY = BaseSZ = BaseAX = BaseAY = GameProcess.MainModule.BaseAddress + 0x0290B008;

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
                }
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
            if (btnConnect.Text == "Disconnect")
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
            if (btnConnect.Text == "Disconnect")
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
            if (btnConnect.Text == "Disconnect")
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
            if (btnConnect.Text == "Disconnect")
            {
                using (Form2 form2 = new Form2(valZ))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        vam.WriteFloat(BaseZ, float.Parse(form2.getNewVal()));
                    }
                    form2.Close();
                }
            }
        }

        private void valPosY_MouseDown(object sender, MouseEventArgs e)
        {
            if (btnConnect.Text == "Disconnect")
            {
                using (Form2 form2 = new Form2(valY))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        vam.WriteFloat(BaseY, float.Parse(form2.getNewVal()));
                    }
                    form2.Close();
                }
            }
        }

        private void valPosX_MouseDown(object sender, MouseEventArgs e)
        {
            if (btnConnect.Text == "Disconnect")
            {
                using (Form2 form2 = new Form2(valX))
                {
                    DialogResult dr = form2.ShowDialog(this);
                    if (dr == DialogResult.OK)
                    {
                        vam.WriteFloat(BaseX, float.Parse(form2.getNewVal()));
                    }
                    form2.Close();
                }
            }
        }

        private void btnMoonjump_Click(object sender, EventArgs e)
        {
            if (moonjump)
            {
                btnMoonjump.Text = "Moonjump (Off)";
                moonjump = false;
            } else
            {
                btnMoonjump.Text = "Moonjump (On)";
                moonjump = true;
            }
        }

        private void btnSuperSpeed_Click(object sender, EventArgs e)
        {
            if (superSpeed)
            {
                btnSuperSpeed.Text = "Super Speed (Off)";
                superSpeed = false;
            }
            else
            {
                btnSuperSpeed.Text = "Super Speed (On)";
                superSpeed = true;
            }
        }

        private void btnLowGravity_Click(object sender, EventArgs e)
        {
            if (lowGravity)
            {
                btnLowGravity.Text = "Low Gravity (Off)";
                lowGravity = false;
            } else
            {
                btnLowGravity.Text = "Low Gravity (On)";
                lowGravity = true;
            }
        }

        private void btnTeleportToMarker_Click(object sender, EventArgs e)
        {
            vam.WriteFloat(BaseX, valMX);
            vam.WriteFloat(BaseY, valMY);
            vam.WriteFloat(BaseZ, valMZ);
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            valStoreX = valX;
            valStoreY = valY;
            valStoreZ = valZ;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            vam.WriteFloat(BaseX, valStoreX);
            vam.WriteFloat(BaseY, valStoreY);
            vam.WriteFloat(BaseZ, valStoreZ);
        }

        private void btnVaultSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fileNames.Length; i++)
            {
                string sourceFile = System.IO.Path.Combine(source, fileNames[i]);
                string targetFile = System.IO.Path.Combine(target, fileNames[i]);

                System.IO.File.Copy(sourceFile, targetFile, true);
            }
        }

        private void SLock2_Click(object sender, EventArgs e)
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

        private void SLock1_Click(object sender, EventArgs e)
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

        private void SLock3_Click(object sender, EventArgs e)
        {
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

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            if (this.BackColor == Color.WhiteSmoke)
            {
                btnDarkMode.Text = "Light Mode";
                lblAngleX.ForeColor = Color.WhiteSmoke;
                lblAngleY.ForeColor = Color.WhiteSmoke;
                lblPosX.ForeColor = Color.WhiteSmoke;
                lblPosY.ForeColor = Color.WhiteSmoke;
                lblPosZ.ForeColor = Color.WhiteSmoke;
                lblMarkX.ForeColor = Color.WhiteSmoke;
                lblMarkY.ForeColor = Color.WhiteSmoke;
                lblMarkZ.ForeColor = Color.WhiteSmoke;
                lblSpeed.ForeColor = Color.WhiteSmoke;
                lblAuthor.ForeColor = Color.WhiteSmoke;
                lblHelpers.ForeColor = Color.WhiteSmoke;
                lblVersion.ForeColor = Color.WhiteSmoke;
                lblPositions.ForeColor = Color.WhiteSmoke;
                lblCheats.ForeColor = Color.WhiteSmoke;
                label1.ForeColor = Color.WhiteSmoke;
                this.BackColor = Color.FromArgb(64, 64, 64);
            } else
            {
                btnDarkMode.Text = "Dark Mode";
                lblAngleX.ForeColor = Color.Black;
                lblAngleY.ForeColor = Color.Black;
                lblPosX.ForeColor = Color.Black;
                lblPosY.ForeColor = Color.Black;
                lblPosZ.ForeColor = Color.Black;
                lblMarkX.ForeColor = Color.Black;
                lblMarkY.ForeColor = Color.Black;
                lblMarkZ.ForeColor = Color.Black;
                lblSpeed.ForeColor = Color.Black;
                lblAuthor.ForeColor = Color.Black;
                lblHelpers.ForeColor = Color.Black;
                lblVersion.ForeColor = Color.Black;
                lblPositions.ForeColor = Color.Black;
                lblCheats.ForeColor = Color.Black;
                label1.ForeColor = Color.Black;
                this.BackColor = Color.WhiteSmoke;
            }
        }
    }
}
