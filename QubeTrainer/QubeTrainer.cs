using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Media;
using System.Timers;

using Virtua_Cop_2Trainer;

namespace QubeTrainerNamespace
{
    class QubeTrainer
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

        bool lockedX, lockedY, lockedZ, lockedSX, lockedSY, lockedSZ, moonjump, singleJump, lowGravity, superSpeed, flyMode, armsHidden = false;

        string[] fileNames = { "MainSaveGame.sav", "MainStatsSaveGame.sav", "MainUnlockedLevels.sav" };
        string sourceVS = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\VaultSave");
        string source;
        string target = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QUBE\\Saved\\SaveGames\\");

        public bool connected = false;

        private static Timer aInterval;

        private int checkCounter = 0;

        List<KeyboardHook.VK> currentKeys = new List<KeyboardHook.VK>();

        public float valX, valY, valZ, valMX, valMY, valMZ, valSX, valSY, valSZ, valAX, valAY, valLockX, valLockY, valLockZ, valLockSX, valLockSY, valLockSZ, valStoreX, valStoreY, valStoreZ;
        public float hashOfValues;
        QubeTrainerUI.Form1 ui;

        public QubeTrainer(QubeTrainerUI.Form1 ui)
        {
            this.ui = ui;
            KeyboardHook.CreateHook(KeyReader);
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

        public void setSave(String name)
        {
            source = Path.Combine(Directory.GetCurrentDirectory(), "Saves\\" + name);
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
                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            string sourceFile = System.IO.Path.Combine(source, fileNames[i]);
                            string targetFile = System.IO.Path.Combine(target, fileNames[i]);

                            System.IO.File.Copy(sourceFile, targetFile, true);
                        }
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
            for (int i = 0; i < fileNames.Length; i++)
            {
                string sourceFile = System.IO.Path.Combine(sourceVS, fileNames[i]);
                string targetFile = System.IO.Path.Combine(target, fileNames[i]);

                System.IO.File.Copy(sourceFile, targetFile, true);
            }
        }

        public void toggleArmsVisible()
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
            ui.SetText(ui.btnHideArms,armsHidden ? "Hide Arms (On)" : "Hide Arms (Off)");
        }

        public void toggleFlyMode()
        {
            flyMode = !flyMode;
            valLockY = valY;
            valLockX = valX;
            valLockZ = valZ;

            ui.SetText(ui.btnFlyMode,flyMode ? "Fly Mode (On)" : "Fly Mode (Off)");
        }

        public void lockX()
        {
            if (lockedX)
            {
                lockedX = false;
                ui.SetText(ui.SLock1,"Lock");
            }
            else
            {
                valLockX = valX;
                valLockSX = 0f;

                lockedX = true;
                ui.SetText(ui.SLock1,"Unlock");
            }
        }

        public void lockY()
        {
            if (lockedY)
            {
                lockedY = false;
                ui.SetText(ui.SLock2,"Lock");
            }
            else
            {
                valLockY = valY;
                valLockSY = 0f;

                lockedY = true;
                ui.SetText(ui.SLock2,"Unlock");
            }
        }

        public void lockZ()
        {
            if (lockedZ)
            {
                lockedZ = false;
                ui.SetText(ui.SLock3,"Lock");
            }
            else
            {
                valLockZ = valZ;
                valLockSZ = 0f;

                lockedZ = true;
                ui.SetText(ui.SLock3,"Unlock");
            }
        }

        public void lockSX()
        {
            if (lockedSX)
            {
                lockedSX = false;
                ui.SetText(ui.SLock4,"Lock");
            }
            else
            {
                valLockSX = valSX;

                lockedSX = true;
                ui.SetText(ui.SLock4,"Unlock");
            }
        }

        public void lockSY()
        {
            if (lockedSY)
            {
                lockedSY = false;
                ui.SetText(ui.SLock5,"Lock");
            }
            else
            {
                valLockSY = valSY;

                lockedSY = true;
                ui.SetText(ui.SLock5,"Unlock");
            }
        }

        public void lockSZ()
        {
            if (lockedSZ)
            {
                lockedSZ = false;
                ui.SetText(ui.SLock6,"Lock");
            }
            else
            {
                valLockSZ = valSZ;

                lockedSZ = true;
                ui.SetText(ui.SLock6,"Unlock");
            }
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
            if (lowGravity)
            {
                ui.SetText(ui.btnLowGravity,"Low Gravity (Off)");
                lowGravity = false;
            }
            else
            {
                ui.SetText(ui.btnLowGravity,"Low Gravity (On)");
                lowGravity = true;
            }
        }

        public void toggleSuperspeed()
        {
            if (superSpeed)
            {
                ui.SetText(ui.btnSuperSpeed,"Super Speed (Off)");
                superSpeed = false;

                vam.WriteFloat(BaseSX, 0f);
                vam.WriteFloat(BaseSZ, 0f);
            }
            else
            {
                ui.SetText(ui.btnSuperSpeed,"Super Speed (On)");
                superSpeed = true;
            }
        }

        public void toggleMoonjump()
        {
            if (moonjump)
            {
                ui.SetText(ui.btnMoonjump,"Moonjump (Off)");
                moonjump = false;
            }
            else
            {
                ui.SetText(ui.btnMoonjump,"Moonjump (On)");
                moonjump = true;
            }
        }

        public void connect()
        {
            processes = Process.GetProcessesByName("QUBE-Win64-Shipping");

            if (processes.Length > 0)
            {
                ui.SetText(ui.btnConnect,"Disconnect");
                ui.setAllEnabled(true);

                GameProcess = processes[0];
                vam = new VAMemory("QUBE-Win64-Shipping");

                setupAddresses();

                SetInterval();
                connected = true;
            }
            else
            {
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
            ui.SetText(ui.btnConnect,"Connect");
            ui.setAllEnabled(false);

            aInterval.Stop();
            aInterval.Dispose();
            for (int i = 0; i < 10; i++)
            {
                ui.clearAll();
            }
            connected = false;
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
    }
}
