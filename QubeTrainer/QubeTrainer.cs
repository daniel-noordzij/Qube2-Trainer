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

using Virtua_Cop_2Trainer;

namespace QubeTrainerNamespace
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

        bool lockedX, lockedY, lockedZ, lockedSX, lockedSY, lockedSZ, moonjump, singleJump, lowGravity, superSpeed, flyMode, armsHidden;

        string fileName = "MainSaveGame.sav";
        string sourceVS = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\VaultSave");
        string source;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QUBE\\Saved\\SaveGames\\");

        public bool connected = false, autoDisconnectToggle = true, recheckPtrToggle = true;

        private static Timer aInterval;
        private static Timer clearDelay;

        private int checkCounter = 0;
        private string currentVersion = "2.0.0";

        List<KeyboardHook.VK> currentKeys = new List<KeyboardHook.VK>();

        public float valX, valY, valZ, valMX, valMY, valMZ, valSX, valSY, valSZ, valAX, valAY, valLockX, valLockY, valLockZ, valLockSX, valLockSY, valLockSZ, valStoreX, valStoreY, valStoreZ;
        public float hashOfValues;
        QubeTrainerUI.Form1 ui;

        public QubeTrainerClass(QubeTrainerUI.Form1 ui)
        {
            this.ui = ui;
            KeyboardHook.CreateHook(KeyReader);
        }

        public void KeyReader(IntPtr wParam, IntPtr lParam)
        {
            if (connected && wParam.ToInt32() == 0x100/* && !ui.hotkeyClicked --  for when custom hotkeys are implemented again */) //WM_KEYDOWN
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
            else if (connected && wParam.ToInt32() == 0x101/* && !ui.hotkeyClicked --  for when custom hotkeys are implemented again */) //WM_KEYUP
            {
                KeyboardHook.VK key = (KeyboardHook.VK)Marshal.ReadInt32(lParam);
                switch (key) //Global Hotkeys
                {
                    default:
                        currentKeys.Remove(key);
                        break;
                }
            }

            /*if (ui.hotkeyClicked) --  for when custom hotkeys are implemented again
            {
                if (wParam.ToInt32() == 0x100)
                {
                    KeyboardHook.VK key = (KeyboardHook.VK)Marshal.ReadInt32(lParam);
                    string keyCode = key.ToString().Replace("VK_", "");
                    ui.updateHotkeyText(keyCode);
                }
            }*/
        }

        public void setSave(String name)
        {
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
            SystemSounds.Beep.Play();
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
            armsHidden = !armsHidden;
            vam.WriteFloat(BaseArmsRotY, armsHidden ? -180 : 0);
            ui.pnlHideArmsF.BackColor = armsHidden ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0 ,0, 0);
        }

        public void toggleFlyMode()
        {
            flyMode = !flyMode;
            valLockY = valY;
            valLockX = valX;
            valLockZ = valZ;
            ui.pnlFlyModeF.BackColor = flyMode ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockX()
        {
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
            lockedSX = !lockedSX;
            if (lockedSX)
                valLockSX = valSX;

            ui.SLock4Check.BackColor = lockedSX ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockSY()
        {
            lockedSY = !lockedSY;
            if (lockedSY)
                valLockSY = valSY;

            ui.SLock5Check.BackColor = lockedSY ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void lockSZ()
        {
            lockedSZ = !lockedSZ;
            if (lockedSZ)
                valLockSZ = valSZ;

            ui.SLock6Check.BackColor = lockedSZ ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void teleportToMarker()
        {
            vam.WriteFloat(BaseX, valMX);
            vam.WriteFloat(BaseY, valMY);
            vam.WriteFloat(BaseZ, valMZ);
            valLockX = valMX;
            valLockY = valMY;
            valLockZ = valMZ;
        }

        public void restorePosition()
        {
            vam.WriteFloat(BaseX, valStoreX);
            vam.WriteFloat(BaseY, valStoreY);
            vam.WriteFloat(BaseZ, valStoreZ);
            valLockX = valStoreX;
            valLockY = valStoreY;
            valLockZ = valStoreZ;
        }

        public void storePosition()
        {
            valStoreX = valX;
            valStoreY = valY;
            valStoreZ = valZ;
        }

        public void toggleLowGravity()
        {
            lowGravity = !lowGravity;
            ui.pnlLowGravityF.BackColor = lowGravity ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void toggleSuperspeed()
        {
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
            moonjump = !moonjump;
            ui.pnlMoonjumpF.BackColor = moonjump ? Color.FromArgb(228, 149, 78) : Color.FromArgb(0, 0, 0);
        }

        public void connect()
        {
            processes = Process.GetProcessesByName("QUBE-Win64-Shipping");
            ui.btnConnect.BackgroundImage = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "icons/stop_btn256.png"));

            if (processes.Length > 0)
            {
                /*ui.setAllEnabled(true);*/

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
            ui.setAllEnabled(false);

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
            WebRequest wr = WebRequest.Create(new Uri("https://raw.githubusercontent.com/LagoLunatic/wwrando/master/version.txt"));
            WebResponse ws = wr.GetResponse();
            StreamReader sr = new StreamReader(ws.GetResponseStream());

            string newVersion = sr.ReadToEnd();

            if (!currentVersion.Contains(newVersion)) //15, 13
            {
                ui.lblNewUpdate.Visible = true;
            }
        }
    }
}
