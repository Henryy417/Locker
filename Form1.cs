/*  Copyright (C) 2020 Henry Chen

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace Locker
{
    public partial class Form1 : Form
    {
        public string otext = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                k.SetValue("DisableRegistryTools", 0, RegistryValueKind.DWord);
                MessageBox.Show("Registry editor has been unlocked.", "Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                k.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
                MessageBox.Show("Registry editor has been locked.", "Locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                k.SetValue("NoControlPanel", 1, RegistryValueKind.DWord);
                MessageBox.Show("Control Panel and System Setting has been locked.", "Locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                k.SetValue("NoControlPanel", 0, RegistryValueKind.DWord);
                MessageBox.Show("Control Panel and System Setting has been unlocked.", "Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"Software\Policies\Microsoft\Windows\System");
                if (checkBox1.Checked)
                {
                    k.SetValue("DisableCMD", 2, RegistryValueKind.DWord);
                }
                else
                {
                    k.SetValue("DisableCMD", 1, RegistryValueKind.DWord);
                }
                MessageBox.Show("CMD has been locked.", "Locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"Software\Policies\Microsoft\Windows\System");
                k.SetValue("DisableCMD", 0, RegistryValueKind.DWord);
                MessageBox.Show("CMD has been unlocked.", "Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string deskTop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
            if (System.IO.File.Exists(deskTop + "Locker.lnk"))
            {
                System.IO.File.Delete(deskTop + "Locker.lnk");
            }
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(deskTop + "Locker.lnk");
            shortcut.TargetPath = Application.StartupPath + "Locker.exe";
            shortcut.WorkingDirectory = Environment.CurrentDirectory;
            shortcut.WindowStyle = 1; 
            shortcut.Description = "Run Locker";
            shortcut.IconLocation = Application.StartupPath + "\\icon.ico";
            shortcut.Arguments = "";
            shortcut.Hotkey = "CTRL+ALT+L";
            shortcut.Save();
            comboBox1.SelectedItem = "Shutdown";
            otext = textBox1.Text;
            int name = 1;
            RegistryKey k = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun");
            for (; ; )
            {
                if (k.GetValue(name.ToString(), null) == null)
                {
                    break;
                }
                textBox1.AppendText(Convert.ToString(k.GetValue(name.ToString())) + "\r\n");
                ++name;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool skip = false;
            string filePath = "";
            int name = 1;
            File.Filter = "Application (*.exe)|*.exe";
            try
            {
                if (File.ShowDialog() == DialogResult.OK)
                {
                    filePath = File.FileName.Substring(File.FileName.LastIndexOf("\\") + 1,
                        File.FileName.Length - File.FileName.LastIndexOf("\\") - 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!skip)
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                    k.SetValue("DisallowRun", 1, RegistryValueKind.DWord);
                    RegistryKey k2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun");
                    for (; ; )
                    {
                        if (k2.GetValue(name.ToString(), null) == null)
                        {
                            k2.SetValue(name.ToString(), filePath, RegistryValueKind.String);
                            break;

                        }
                        else
                        {
                            ++name;
                        }
                    }
                    textBox1.AppendText(filePath + "\r\n");
                    MessageBox.Show(textBox1.Text + " has been locked.", "Locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    k.Close();
                    k2.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int name = 1;
            RegistryKey k2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun");


            object text = k2.GetValue(name.ToString());
            textBox1.AppendText(Convert.ToString(text));
            ++name;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun");
                textBox1.Text = "";
            }
            catch (ArgumentException)
            {
                MessageBox.Show("You have cleared all already.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int name = 1;
                int no = 0;
                textBox1.Text += "\r\n";
                string[] lines = textBox1.Text.Split("\r\n");
                try
                {
                    Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun");
                }
                catch (ArgumentException)
                {

                }
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer\DisallowRun");
                for (; ; )
                {
                    string value = lines[no];
                    if (value == "" || value == null)
                    {
                        MessageBox.Show("All .exe files in your input has been locked. (if there are empty line in your input, we will not locked the app(s) after it, please delete all empty line(s)!)", "Locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                    if (!value.Contains(".exe"))
                    {
                        MessageBox.Show("Please make sure you have added \".exe\" in the end of your applications name \"" + value + "\", and it is an .exe file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    if (value.Contains("?") || value.Contains("/") || value.Contains("\\") || value.Contains("<") || value.Contains(">") || value.Contains("|") || value.Contains(":") || value.Contains(" "))
                    {
                        MessageBox.Show("There are illegal character(s) in \"" + value + "\", please make sure it is a .exe file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    k.SetValue(name.ToString(), value, RegistryValueKind.String);
                    MessageBox.Show(lines[no] + "\r\nhas been locked");
                    ++name;
                    ++no;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button11.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                k.SetValue("NoClose", 1, RegistryValueKind.DWord);
                MessageBox.Show("Computer is now unable to shutdown.", "Locked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey k = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                k.SetValue("NoClose", 0, RegistryValueKind.DWord);
                MessageBox.Show("Computer is now able to shutdown.", "Unlocked", MessageBoxButtons.OK, MessageBoxIcon.Information);
                k.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("We run into a problem, give the below error code to support staff:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            textBox2.Text = "";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Contains(" "))
            {
                string text = textBox2.Text.Replace(" ", "");
                if (text == "")
                {
                    MessageBox.Show("You must add message to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("You must add message to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string totaltime = Convert.ToString((numericUpDown1.Value * 3600) + (numericUpDown2.Value * 60) + (numericUpDown3.Value));
            if (Convert.ToInt32(totaltime) > 315360000)
            {
                MessageBox.Show("Total time cannot be greater than 10 years.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            switch (comboBox1.SelectedItem)
            {
                case "Shutdown":
                    cmd("shutdown /s /t " + totaltime + " /c " + textBox2.Text);
                    break;
                case "Restart":
                    cmd("shutdown /r /t " + totaltime + " /c " + textBox2.Text);
                    break;
                case "Logoff":
                    cmd("shutdown /l /t " + totaltime + " /c " + textBox2.Text);
                    break;

            }
        }
        private void cmd(string str)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.StandardInput.WriteLine(str + "&exit");
                    process.StandardInput.AutoFlush = true;
                    process.WaitForExit();
                    process.Close();
                }
            }
            catch
            {
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            cmd("shutdown /a");
        }

        private void menuStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MessageBox.Show("Locker (v1.0)\nCopyright (C) 2020 Henry Chen\nAll rights reserved.\nThis work is licensed under GNU GENERAL PUBLIC LICENSE (Version 3).\nThis program comes with ABSOLUTELY NO WARRANTY.\nThis is free software, and you are welcome to redistribute itunder certain conditions.", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
