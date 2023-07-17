using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Amazon;
using Amazon.RDS;
using Amazon.RDS.Model;

namespace BookShop
{
    public partial class Login : Form
    {
        string awsAccessKey = "admin";
        string awsSecretKey = "r7568436";
        string awsRdsEndpoint = "database-1.clqo6emvk7i4.ap-northeast-1.rds.amazonaws.com"; // 資料庫伺服器的端點
        string awsDatabaseName = "LeafBookShopDb";
        private SqlConnection Con;
        public Login()
        {
            InitializeComponent();
            string connectionString = $"Data Source={awsRdsEndpoint};Initial Catalog={awsDatabaseName};User ID={awsAccessKey};Password={awsSecretKey};";
            Con = new SqlConnection(connectionString);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public static string UserName = "";
        private void button1_Click(object sender, EventArgs e)
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select count(*) from UserTb1 where UName='" + UNameTb.Text + "' and UPassword = '" + UPassTb.Text + "'", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                UserName = UNameTb.Text;
                Billing obj = new Billing();
                obj.Show();
                this.Hide();
                Con.Close();
            }
            else
            {
                MessageBox.Show("Invalid account or password");
            }
            Con.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            AddminLogin obj = new AddminLogin();
            obj.Show();
            this.Hide();
        }
    }
}
