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
    public partial class Books : Form
    {
        string awsAccessKey = "admin";
        string awsSecretKey = "r7568436";
        string awsRdsEndpoint = "database-1.clqo6emvk7i4.ap-northeast-1.rds.amazonaws.com"; // 資料庫伺服器的端點
        string awsDatabaseName = "LeafBookShopDb";
        private SqlConnection Con;
        public Books()
        {
            InitializeComponent();
            string connectionString = $"Data Source={awsRdsEndpoint};Initial Catalog={awsDatabaseName};User ID={awsAccessKey};Password={awsSecretKey};";
            Con = new SqlConnection(connectionString);
            populate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void populate()
        {


            // 建立 AWS RDS 連線字串
            string connectionString = $"Data Source={awsRdsEndpoint};Initial Catalog={awsDatabaseName};User ID={awsAccessKey};Password={awsSecretKey};";

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM BookTb1";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                var ds = new DataSet();
                sda.Fill(ds);
                BookDGV.DataSource = ds.Tables[0];
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (BTitleTb.Text == "" || BAutTb.Text == "" || BCatCb.SelectedIndex == -1 || QtyTb.Text == "" || PriceTb.Text == "")
            {
                MessageBox.Show("The form submission failed due to unfilled fields.");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "update BookTb1 set BTitle = '" + BTitleTb.Text + "', BAuthor= '" + BAutTb.Text + "', BCat= '" + BCatCb.SelectedItem.ToString() + "', BQty =" + QtyTb.Text + ", BPrice=" + PriceTb.Text + " where BId = " + key + "";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Edit success!");
                    Con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (BTitleTb.Text == "" || BAutTb.Text == "" || BCatCb.SelectedIndex == -1 || QtyTb.Text == "" || PriceTb.Text == "")
            {
                MessageBox.Show("The form submission failed due to unfilled fields.");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "insert into BookTb1 values('" + BTitleTb.Text + "','" + BAutTb.Text + "','" + BCatCb.SelectedItem.ToString() + "'," + QtyTb.Text + "," + PriceTb.Text + ")";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Save success!");
                    Con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void Filter()
        {
            Con.Open();
            string query = "select * from BookTb1 where BCat ='" + CatCbSearch.SelectedItem.ToString() + "' ";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void CatCbSearchCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            populate();
            CatCbSearch.SelectedIndex = -1;
        }

        private void Reset()
        {
            BTitleTb.Text = "";
            BAutTb.Text = "";
            BCatCb.SelectedIndex = -1;
            QtyTb.Text = "";
            PriceTb.Text = "";
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
            CatCbSearch.SelectedIndex = -1;
        }
        int key = 0;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BTitleTb.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();
            BAutTb.Text = BookDGV.SelectedRows[0].Cells[2].Value.ToString();
            BCatCb.SelectedItem = BookDGV.SelectedRows[0].Cells[3].Value.ToString();
            QtyTb.Text = BookDGV.SelectedRows[0].Cells[4].Value.ToString();
            PriceTb.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
            if (BTitleTb.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("You don't insert anything!");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from BookTb1 where BId =" + key + "";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Delete success!");
                    Con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Users obj = new Users();
            obj.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            DashBoard obj = new DashBoard();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
