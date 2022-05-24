using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Media;
using System.Timers;
using System.Drawing;
using System.Windows.Input;
using System.Net;
using System.Windows.Forms;

using Virtua_Cop_2Trainer;

namespace QubeTrainer
{
    class QubeTrainerClass
    {

        VAMemory vam;
        Process GameProcess;
        Process[] processes;
        public IntPtr BaseX, BaseY, BaseZ, BaseMX, BaseMY, BaseMZ, BaseSX, BaseSY, BaseSZ, BaseAX, BaseAY, BaseArmsRotY;
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

        int turnValue;
        int flyModeSpeed = 100;

        bool lockedX, lockedY, lockedZ, lockedSX, lockedSY, lockedSZ, moonjump, singleJump, lowGravity, superSpeed, flyMode, armsHidden;

        string fileName = "MainSaveGame.sav";
        string sourceVS = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\VaultSave");
        string source;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QUBE\\Saved\\SaveGames\\");

        public bool connected = false, autoDisconnectToggle = true, recheckPtrToggle = true, lockAllFkeys = false;

        private static System.Timers.Timer aInterval;
        private static System.Timers.Timer clearDelay;
        private static System.Timers.Timer appliedTimer;

        private int checkCounter = 0;
        private string currentVersion = "2.0";

        List<KeyboardHook.VK> currentKeys = new List<KeyboardHook.VK>();
        public string f1Num = "1", f2Num = "2", f3Num = "3", f4Num = "4", f5Num = "5", f6Num = "6", f7Num = "8", f8Num = "9", f9Num ="10", f10Num = "7", f12Num = "15";

        public float valX, valY, valZ, valMX, valMY, valMZ, valSX, valSY, valSZ, valAX, valAY, valLockX, valLockY, valLockZ, valLockSX, valLockSY, valLockSZ, valStoreX, valStoreY, valStoreZ;
        public float hashOfValues;
        QubeTrainerUI.Form1 ui;

        public QubeTrainerClass(QubeTrainerUI.Form1 ui)
        {
            this.ui = ui;
            KeyboardHook.CreateHook(KeyReader);
            GetSettings();
        }

        public void GetSettings()
        {
            f1Num = Properties.Settings.Default.F1HK;
            f2Num = Properties.Settings.Default.F2HK;
            f3Num = Properties.Settings.Default.F3HK;
            f4Num = Properties.Settings.Default.F4HK;
            f5Num = Properties.Settings.Default.F5HK;
            f6Num = Properties.Settings.Default.F6HK;
            f7Num = Properties.Settings.Default.F7HK;
            f8Num = Properties.Settings.Default.F8HK;
            f9Num = Properties.Settings.Default.F9HK;
            f10Num = Properties.Settings.Default.F10HK;
            f12Num = Properties.Settings.Default.F12HK;
            autoDisconnectToggle = Properties.Settings.Default.AutoDisc;
            recheckPtrToggle = Properties.Settings.Default.PtrCheck;

            ui.lblHotF1.Text = GetHKNameFromNum(Properties.Settings.Default.F1HK);
            ui.lblHotF2.Text = GetHKNameFromNum(Properties.Settings.Default.F2HK);
            ui.lblHotF3.Text = GetHKNameFromNum(Properties.Settings.Default.F3HK);
            ui.lblHotF4.Text = GetHKNameFromNum(Properties.Settings.Default.F4HK);
            ui.lblHotF5.Text = GetHKNameFromNum(Properties.Settings.Default.F5HK);
            ui.lblHotF6.Text = GetHKNameFromNum(Properties.Settings.Default.F6HK);
            ui.lblHotF7.Text = GetHKNameFromNum(Properties.Settings.Default.F7HK);
            ui.lblHotF8.Text = GetHKNameFromNum(Properties.Settings.Default.F8HK);
            ui.lblHotF9.Text = GetHKNameFromNum(Properties.Settings.Default.F9HK);
            ui.lblHotF10.Text = GetHKNameFromNum(Properties.Settings.Default.F10HK);
            ui.lblHotF12.Text = GetHKNameFromNum(Properties.Settings.Default.F12HK);

            ui.lblHkLvlF1.Text = GetHKNameFromNum(Properties.Settings.Default.F1HK);
            ui.lblHkLvlF2.Text = GetHKNameFromNum(Properties.Settings.Default.F2HK);
            ui.lblHkLvlF3.Text = GetHKNameFromNum(Properties.Settings.Default.F3HK);
            ui.lblHkLvlF4.Text = GetHKNameFromNum(Properties.Settings.Default.F4HK);
            ui.lblHkLvlF5.Text = GetHKNameFromNum(Properties.Settings.Default.F5HK);
            ui.lblHkLvlF6.Text = GetHKNameFromNum(Properties.Settings.Default.F6HK);
            ui.lblHkLvlF7.Text = GetHKNameFromNum(Properties.Settings.Default.F7HK);
            ui.lblHkLvlF8.Text = GetHKNameFromNum(Properties.Settings.Default.F8HK);
            ui.lblHkLvlF9.Text = GetHKNameFromNum(Properties.Settings.Default.F9HK);
            ui.lblHkLvlF10.Text = GetHKNameFromNum(Properties.Settings.Default.F10HK);
            ui.lblHkLvlF12.Text = GetHKNameFromNum(Properties.Settings.Default.F12HK);

            ui.pnlAutoDiscF.BackColor = autoDisconnectToggle ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
            ui.pnlRecheckF.BackColor = recheckPtrToggle ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.F1HK = f1Num;
            Properties.Settings.Default.F2HK = f2Num;
            Properties.Settings.Default.F3HK = f3Num;
            Properties.Settings.Default.F4HK = f4Num;
            Properties.Settings.Default.F5HK = f5Num;
            Properties.Settings.Default.F6HK = f6Num;
            Properties.Settings.Default.F7HK = f7Num;
            Properties.Settings.Default.F8HK = f8Num;
            Properties.Settings.Default.F9HK = f9Num;
            Properties.Settings.Default.F10HK = f10Num;
            Properties.Settings.Default.F12HK = f12Num;
            Properties.Settings.Default.AutoDisc = autoDisconnectToggle;
            Properties.Settings.Default.PtrCheck = recheckPtrToggle;

            Properties.Settings.Default.Save();

            SoundPlayer appliedPlayer = new SoundPlayer(Path.Combine(Directory.GetCurrentDirectory(), "icons/appliedsound.wav"));
            appliedPlayer.Play();

            ui.pnlSettingApplied.Visible = true;
            ui.lblSettingApplied.Visible = true;

            appliedTimer = new System.Timers.Timer(1000);
            appliedTimer.Elapsed += applyEvent;
            appliedTimer.AutoReset = false;
            appliedTimer.Enabled = true;
        }

        public void saveLevelSettings(string lvlFKey, string lvlNum)
        {
            switch (lvlFKey)
            {
                case "F1":
                    Properties.Settings.Default.F1HK = "18-" + lvlNum;
                    f1Num = "18-" + lvlNum;
                    ui.lblHotF1.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF1.Text = "Set Level " + lvlNum;
                    break;
                case "F2":
                    Properties.Settings.Default.F2HK = "18-" + lvlNum;
                    f2Num = "18-" + lvlNum;
                    ui.lblHotF2.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF2.Text = "Set Level " + lvlNum;
                    break;
                case "F3":
                    Properties.Settings.Default.F3HK = "18-" + lvlNum;
                    f3Num = "18-" + lvlNum;
                    ui.lblHotF3.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF3.Text = "Set Level " + lvlNum;
                    break;
                case "F4":
                    Properties.Settings.Default.F4HK = "18-" + lvlNum;
                    f4Num = "18-" + lvlNum;
                    ui.lblHotF4.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF4.Text = "Set Level " + lvlNum;
                    break;
                case "F5":
                    Properties.Settings.Default.F5HK = "18-" + lvlNum;
                    f5Num = "18-" + lvlNum;
                    ui.lblHotF5.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF5.Text = "Set Level " + lvlNum;
                    break;
                case "F6":
                    Properties.Settings.Default.F6HK = "18-" + lvlNum;
                    f6Num = "18-" + lvlNum;
                    ui.lblHotF6.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF6.Text = "Set Level " + lvlNum;
                    break;
                case "F7":
                    Properties.Settings.Default.F7HK = "18-" + lvlNum;
                    f7Num = "18-" + lvlNum;
                    ui.lblHotF7.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF7.Text = "Set Level " + lvlNum;
                    break;
                case "F8":
                    Properties.Settings.Default.F8HK = "18-" + lvlNum;
                    f8Num = "18-" + lvlNum;
                    ui.lblHotF8.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF8.Text = "Set Level " + lvlNum;
                    break;
                case "F9":
                    Properties.Settings.Default.F9HK = "18-" + lvlNum;
                    f9Num = "18-" + lvlNum;
                    ui.lblHotF9.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF9.Text = "Set Level " + lvlNum;
                    break;
                case "F10":
                    Properties.Settings.Default.F10HK = "18-" + lvlNum;
                    f10Num = "18-" + lvlNum;
                    ui.lblHotF10.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF10.Text = "Set Level " + lvlNum;
                    break;
                case "F12":
                    Properties.Settings.Default.F12HK = "18-" + lvlNum;
                    f12Num = "18-" + lvlNum;
                    ui.lblHotF12.Text = "Set Level " + lvlNum;
                    ui.lblHkLvlF12.Text = "Set Level " + lvlNum;
                    break;
            }

            SoundPlayer appliedPlayer = new SoundPlayer(Path.Combine(Directory.GetCurrentDirectory(), "icons/appliedsound.wav"));
            appliedPlayer.Play();

            Properties.Settings.Default.Save();
        }

        private void applyEvent(Object source, ElapsedEventArgs e)
        {
            if (ui.pnlSettingApplied.InvokeRequired)
                ui.pnlSettingApplied.Invoke(new MethodInvoker(delegate
                {
                    ui.pnlSettingApplied.Visible = false;
                    ui.lblSettingApplied.Visible = false;
                }));
            else
            {
                ui.pnlSettingApplied.Visible = false;
                ui.lblSettingApplied.Visible = false;
            }
        }

        public string GetHKNameFromNum(string hkNum)
        {
            string hkName = "Error!";
            switch (hkNum.Split('-')[0])
            {
                case "1":
                    hkName = "Moon Jump";
                    break;
                case "2":
                    hkName = "Super Speed";
                    break;
                case "3":
                    hkName = "Low Gravity";
                    break;
                case "4":
                    hkName = "Store Position";
                    break;
                case "5":
                    hkName = "Restore Position";
                    break;
                case "6":
                    hkName = "TP to Marker";
                    break;
                case "7":
                    hkName = "Fly Mode";
                    break;
                case "8":
                    hkName = "Lock X Position";
                    break;
                case "9":
                    hkName = "Lock Y Position";
                    break;
                case "10":
                    hkName = "Lock Z Position";
                    break;
                case "11":
                    hkName = "Lock X Speed";
                    break;
                case "12":
                    hkName = "Lock Y Speed";
                    break;
                case "13":
                    hkName = "Lock Z Speed";
                    break;
                case "14":
                    hkName = "Toggle Connect";
                    break;
                case "15":
                    hkName = "Hide Arms";
                    break;
                case "16":
                    hkName = "Set Vault Save";
                    break;
                case "17":
                    hkName = "Set Previous Save";
                    break;
                case "18":
                    hkName = "Level " + hkNum.Split('-')[1];
                    break;
            }
            return hkName;
        }

        public void KeyReader(IntPtr wParam, IntPtr lParam)
        {
            if (!lockAllFkeys)
            {
                if (wParam.ToInt32() == 0x100) //WM_KEYDOWN
                {
                    KeyboardHook.VK key = (KeyboardHook.VK)Marshal.ReadInt32(lParam);
                    switch (key) //Global Hotkeys
                    {
                        case KeyboardHook.VK.VK_F1:
                            hotkeyHub(f1Num);
                            break;
                        case KeyboardHook.VK.VK_F2:
                            hotkeyHub(f2Num);
                            break;
                        case KeyboardHook.VK.VK_F3:
                            hotkeyHub(f3Num);
                            break;
                        case KeyboardHook.VK.VK_F4:
                            hotkeyHub(f4Num);
                            break;
                        case KeyboardHook.VK.VK_F5:
                            hotkeyHub(f5Num);
                            break;
                        case KeyboardHook.VK.VK_F6:
                            hotkeyHub(f6Num);
                            break;
                        case KeyboardHook.VK.VK_F7:
                            hotkeyHub(f7Num);
                            break;
                        case KeyboardHook.VK.VK_F8:
                            hotkeyHub(f8Num);
                            break;
                        case KeyboardHook.VK.VK_F9:
                            hotkeyHub(f9Num);
                            break;
                        case KeyboardHook.VK.VK_F10:
                            hotkeyHub(f10Num);
                            break;
                        //F11 reserved for QUBE fullscreen toggling
                        case KeyboardHook.VK.VK_F12:
                            hotkeyHub(f12Num);
                            break;
                        //page up & down for flymode speed adjustment
                        case KeyboardHook.VK.VK_PRIOR:
                            changeFlySpeed(1);
                            break;
                        case KeyboardHook.VK.VK_NEXT:
                            changeFlySpeed(2);
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
        }

        public void changeFlySpeed(int directionSpeed)
        {
            if (directionSpeed == 1)
            {
                if (flyMode && flyModeSpeed < 300)
                {
                    flyModeSpeed = flyModeSpeed + 20;
                    ui.lblFlySpeed.Text = flyModeSpeed.ToString();
                }
            } else
            {
                if (flyMode && flyModeSpeed > 20)
                {
                    flyModeSpeed = flyModeSpeed - 20;
                    ui.lblFlySpeed.Text = flyModeSpeed.ToString();
                }
            }
        }

        public void setSave(String name)
        {
            Console.WriteLine(name);
            source = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\" + name);
            string sourceFile = System.IO.Path.Combine(source, fileName);
            string targetFile = System.IO.Path.Combine(target, fileName);

            System.IO.File.Copy(sourceFile, targetFile, true);

            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
                {
                    sw.WriteLine(name);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
                {
                    sw.WriteLine(name);
                }
            }

            SoundPlayer appliedPlayer = new SoundPlayer(Path.Combine(Directory.GetCurrentDirectory(), "icons/appliedsound.wav"));
            appliedPlayer.Play();
        }

        public void reloadSave()
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
            {
                using (StreamReader sr = File.OpenText(Path.Combine(Directory.GetCurrentDirectory(), "last_save.txt")))
                {
                    string s = "";
                    if ((s = sr.ReadLine()) != null)
                    {
                        source = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\" + s);
                        string sourceFile = System.IO.Path.Combine(source, fileName);
                        string targetFile = System.IO.Path.Combine(target, fileName);

                        File.Copy(sourceFile, targetFile, true);
                    }
                    else
                    {
                        ui.showMessageBox("Something went wrong, please ask for help in the discord!", "Error!");
                    }
                }
            }
            else
            {
                ui.showMessageBox("No existing previous save!", "Error!");
            }
        }

        public void loadVaultSave()
        {
            string sourceFile = System.IO.Path.Combine(sourceVS, fileName);
            string targetFile = System.IO.Path.Combine(target, fileName);

            System.IO.File.Copy(sourceFile, targetFile, true);
        }

        public void toggleArmsVisible()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            armsHidden = !armsHidden;
            vam.WriteFloat(BaseArmsRotY, armsHidden ? -180 : 0);
            ui.pnlHideArmsF.BackColor = armsHidden ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0 ,0, 0);
        }

        public void toggleFlyMode()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            flyMode = !flyMode;
            valLockY = valY;
            valLockX = valX;
            valLockZ = valZ;
            ui.pnlFlyModeF.BackColor = flyMode ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockX()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            lockedX = !lockedX;
            if (lockedX)
            {
                valLockX = valX;
                valLockSX = 0f;
            }
            ui.SLock1Check.BackColor = lockedX ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockY()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            lockedY = !lockedY;
            if (lockedY)
            {
                valLockY = valY;
                valLockSY = 0f;
            }
            ui.SLock2Check.BackColor = lockedY ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockZ()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            lockedZ = !lockedZ;
            if (lockedZ)
            {
                valLockZ = valZ;
                valLockSZ = 0f;
            }
            ui.SLock3Check.BackColor = lockedZ ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockSX()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            lockedSX = !lockedSX;
            if (lockedSX)
                valLockSX = valSX;

            ui.SLock4Check.BackColor = lockedSX ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockSY()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            lockedSY = !lockedSY;
            if (lockedSY)
                valLockSY = valSY;

            ui.SLock5Check.BackColor = lockedSY ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockSZ()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            lockedSZ = !lockedSZ;
            if (lockedSZ)
                valLockSZ = valSZ;

            ui.SLock6Check.BackColor = lockedSZ ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void teleportToMarker()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            vam.WriteFloat(BaseX, valMX);
            vam.WriteFloat(BaseY, valMY);
            vam.WriteFloat(BaseZ, valMZ);
            valLockX = valMX;
            valLockY = valMY;
            valLockZ = valMZ;
        }

        public void restorePosition()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            vam.WriteFloat(BaseX, valStoreX);
            vam.WriteFloat(BaseY, valStoreY);
            vam.WriteFloat(BaseZ, valStoreZ);
            valLockX = valStoreX;
            valLockY = valStoreY;
            valLockZ = valStoreZ;
        }

        public void storePosition()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            valStoreX = valX;
            valStoreY = valY;
            valStoreZ = valZ;
        }

        public void toggleLowGravity()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            lowGravity = !lowGravity;
            ui.pnlLowGravityF.BackColor = lowGravity ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void toggleSuperspeed()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            superSpeed = !superSpeed;
            if (superSpeed)
            {
                vam.WriteFloat(BaseSX, 0f);
                vam.WriteFloat(BaseSZ, 0f);
            }
            ui.pnlSuperSpeedF.BackColor = superSpeed ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void toggleMoonjump()
        {
            if (!connected)
            {
                ui.showMessageBox("The trainer needs to be connected for cheats to be enabled!", "Error Not Connected");
                return;
            }
            moonjump = !moonjump;
            ui.pnlMoonjumpF.BackColor = moonjump ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void connect()
        {
            processes = Process.GetProcessesByName("QUBE-Win64-Shipping");
            ui.btnConnect.BackgroundImage = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "icons/stop_btn256.png"));

            if (processes.Length > 0)
            {

                GameProcess = processes[0];
                vam = new VAMemory("QUBE-Win64-Shipping");

                setupAddresses();

                SetInterval();
                connected = true;
            }
            else
            {
                ui.btnConnect.BackgroundImage = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "icons/play_btn256.png"));
                ui.showMessageBox("Could not find an open QUBE 2 process!", "Error Finding Process");
            }
        }

        public void setupAddresses()
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

        public void disconnect()
        {
            ui.btnConnect.BackgroundImage = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "icons/play_btn256.png"));

            aInterval.Stop();
            aInterval.Dispose();
                
            clearDelay = new System.Timers.Timer(40);
            clearDelay.Elapsed += clearEvent;
            clearDelay.AutoReset = false;
            clearDelay.Enabled = true;

            connected = false;
        }

        private void clearEvent(Object source, ElapsedEventArgs e)
        {
            ui.clearAll();
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

            if (autoDisconnectToggle)
            {
                processes = Process.GetProcessesByName("QUBE-Win64-Shipping");
                if (processes.Length <= 0)
                {
                    disconnect();
                }
            }

            valX = vam.ReadFloat(BaseX);
            valY = vam.ReadFloat(BaseY);
            valZ = vam.ReadFloat(BaseZ);

            valMX = vam.ReadFloat(BaseMX);
            valMY = vam.ReadFloat(BaseMY);
            valMZ = vam.ReadFloat(BaseMZ);

            valSX = vam.ReadFloat(BaseSX);
            valSY = vam.ReadFloat(BaseSY);
            valSZ = vam.ReadFloat(BaseSZ);

            valAX = vam.ReadFloat(BaseAX);
            valAY = vam.ReadFloat(BaseAY);

            ui.updateAllValues();

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
                    valLockY += flyModeSpeed;
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_LSHIFT))
                {
                    valLockY -= flyModeSpeed;
                }
                vam.WriteFloat(BaseSX, 0);
                vam.WriteFloat(BaseSY, 0);
                vam.WriteFloat(BaseSZ, 0);

                if (currentKeys.Contains(KeyboardHook.VK.VK_W))
                {
                    valX = (((float)Math.Cos((Math.PI / 180) * valAX) * flyModeSpeed) + valX);
                    valZ = (((float)Math.Sin((Math.PI / 180) * valAX) * flyModeSpeed) + valZ);
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_A))
                {
                    valX = (((float)Math.Sin((Math.PI / 180) * valAX) * flyModeSpeed) + valX);
                    valZ = (((float)Math.Cos((Math.PI / 180) * valAX) * -flyModeSpeed) + valZ);
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_S))
                {
                    valX = (((float)Math.Cos((Math.PI / 180) * valAX) * -flyModeSpeed) + valX);
                    valZ = (((float)Math.Sin((Math.PI / 180) * valAX) * -flyModeSpeed) + valZ);
                }
                if (currentKeys.Contains(KeyboardHook.VK.VK_D))
                {
                    valX = (((float)Math.Sin((Math.PI / 180) * valAX) * -flyModeSpeed) + valX);
                    valZ = (((float)Math.Cos((Math.PI / 180) * valAX) * flyModeSpeed) + valZ);
                }

                vam.WriteFloat(BaseX, valX);
                vam.WriteFloat(BaseZ, valZ);

                vam.WriteFloat(BaseY, valLockY);
            }

            if (lowGravity && valSY <= -500)
            {
                vam.WriteFloat(BaseSY, -400);
            }

            if (recheckPtrToggle)
            {
                int newHash = calculateHashOfValues();
                if (hashOfValues == newHash)
                {
                    checkCounter++;
                }
                hashOfValues = newHash;

                if (checkCounter >= 150)
                {
                    checkCounter = 0;

                    Console.WriteLine("testttttt");

                    setupAddresses();
                }
            }
        }

        public int calculateHashOfValues()
        {
            int hash = 139;
            hash = (hash * 47) + valX.GetHashCode();
            hash = (hash * 47) + valY.GetHashCode();
            hash = (hash * 47) + valZ.GetHashCode();
            hash = (hash * 47) + valMX.GetHashCode();
            hash = (hash * 47) + valMY.GetHashCode();
            hash = (hash * 47) + valMZ.GetHashCode();
            hash = (hash * 47) + valSX.GetHashCode();
            hash = (hash * 47) + valSY.GetHashCode();
            hash = (hash * 47) + valSZ.GetHashCode();
            hash = (hash * 47) + valAX.GetHashCode();
            hash = (hash * 47) + valAY.GetHashCode();
            return hash;
        }

        public void writeFloat(IntPtr address, float newValue)
        {
            vam.WriteFloat(address, newValue);
        }

        public void checkNewUpdate()
        {
            using (WebClient wc = new WebClient())
            {
                var newVersion = wc.DownloadString("https://raw.githubusercontent.com/daniel-noordzij/Qube2-Trainer/master/version.txt");

                if (currentVersion != newVersion)
                {
                    ui.lblNewUpdate.Visible = true;
                }
            }
        }

        public void hotkeyHub(string hkNum)
        {

            switch (hkNum.Split('-')[0])
            {
                case "1":
                    toggleMoonjump();
                    break;
                case "2":
                    toggleSuperspeed();
                    break;
                case "3":
                    toggleLowGravity();
                    break;
                case "4":
                    storePosition();
                    break;
                case "5":
                    restorePosition();
                    break;
                case "6":
                    teleportToMarker();
                    break;
                case "7":
                    toggleFlyMode();
                    break;
                case "8":
                    lockX();
                    break;
                case "9":
                    lockY();
                    break;
                case "10":
                    lockZ();
                    break;
                case "11":
                    lockSX();
                    break;
                case "12":
                    lockSY();
                    break;
                case "13":
                    lockSZ();
                    break;
                case "14":
                    if (!connected)
                    {
                        connect();
                    }
                    else
                    {
                        disconnect();
                    }
                    break;
                case "15":
                    toggleArmsVisible();
                    break;
                case "16":
                    loadVaultSave();
                    break;
                case "17":
                    reloadSave();
                    break;
                case "18":
                    setSave(hkNum.Split('-')[1]);
                    break;

            }
        }
    }
}
