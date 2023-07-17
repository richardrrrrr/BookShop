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
    public partial class DashBoard : Form
    {
        string awsAccessKey = "admin";
        string awsSecretKey = "r7568436";
        string awsRdsEndpoint = "database-1.clqo6emvk7i4.ap-northeast-1.rds.amazonaws.com"; // 資料庫伺服器的端點
        string awsDatabaseName = "LeafBookShopDb";
        private SqlConnection Con;
        public DashBoard()
        {
            InitializeComponent();
            string connectionString = $"Data Source={awsRdsEndpoint};Initial Catalog={awsDatabaseName};User ID={awsAccessKey};Password={awsSecretKey};";
            Con = new SqlConnection(connectionString);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Books obj = new Books();
            obj.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select sum(BQty) from BookTb1", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            BookStockLb1.Text = dt.Rows[0][0].ToString();

            SqlDataAdapter sda1 = new SqlDataAdapter("select sum(Amount) from BillTb1", Con);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            AmountLb1.Text = dt1.Rows[0][0].ToString();
            Con.Close();

            SqlDataAdapter sda2 = new SqlDataAdapter("select Count(*) from UserTb1", Con);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            UserTotalLb1.Text = dt2.Rows[0][0].ToString();
            Con.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
