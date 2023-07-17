using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookShop
{
    public partial class AddminLogin : Form
    {
        public AddminLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (UPassTb.Text == "password")
            {
                Books obj = new Books();
                obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Password incorrect");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
    }
}
