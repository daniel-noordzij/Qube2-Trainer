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
    public partial class Form2 : Form
    {
        public Form2(float boxValue)
        {
            InitializeComponent();
            valBox.Text = boxValue.ToString();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void btnSetNewPos_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void valBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) || ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1)))
            {
                e.Handled = true;
            }
        }

        public string getNewVal()
        {
            return valBox.Text;
        }

        private void valBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
