using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QubeTrainer
{
    public partial class HotkeyForm : Form
    {
        string newHKNum, newHKName;

        public HotkeyForm(string oldFKey)
        {
            InitializeComponent();
            lblHKTitle.Text = "Select new " + oldFKey + " hotkey...";
        }

        private void setNewHotkey(object sender, EventArgs e)
        {
            newHKNum = (sender as Label).Name;
            newHKName = (sender as Label).Text;
            DialogResult = DialogResult.OK;
        }

        public string[] getNewHK()
        {
            string[] newHKInfo = { newHKNum.Substring(5), newHKName};
            return newHKInfo;
        }

        private void lblHKCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
