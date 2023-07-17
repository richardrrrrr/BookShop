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
    public partial class Billing : Form
    {
        string awsAccessKey = "admin";
        string awsSecretKey = "r7568436";
        string awsRdsEndpoint = "database-1.clqo6emvk7i4.ap-northeast-1.rds.amazonaws.com"; // 資料庫伺服器的端點
        string awsDatabaseName = "LeafBookShopDb";
        private SqlConnection Con;
        public Billing()
        {
            InitializeComponent();
            string connectionString = $"Data Source={awsRdsEndpoint};Initial Catalog={awsDatabaseName};User ID={awsAccessKey};Password={awsSecretKey};";
            Con = new SqlConnection(connectionString);
            populate();
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
        int key = 0, stock = 0;
        private void BookDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BTitleTb.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();
            // BAutTb.Text = BookDGV.SelectedRows[0].Cells[2].Value.ToString();
            //BCatCb.SelectedItem = BookDGV.SelectedRows[0].Cells[3].Value.ToString();
            //QtyTb.Text = BookDGV.SelectedRows[0].Cells[4].Value.ToString();
            PriceTb.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
            QtyTb.Text = "";
            if (BTitleTb.Text == "")
            {
                key = 0;
                stock = 0;
            }
            else
            {
                key = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value.ToString());
                stock = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[4].Value.ToString());
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Reset()
        {
            BTitleTb.Text = "";

            QtyTb.Text = "";
            PriceTb.Text = "";
        }

        private void UpdateBook()
        {
            int newQty = stock - Convert.ToInt32(QtyTb.Text);
            try
            {
                Con.Open();
                string query = "update BookTb1 set  BQty =" + newQty + " where BId = " + key + "";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Edit success!");
                Con.Close();
                populate();
                Reset();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

      

        private void PrintBtn_Click(object sender, EventArgs e)
        {

            if (BillDGV.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Haven't chosen a product yet");
            }
            else
            {

                try
                {
                    Con.Open();
                    string query = "insert into BillTb1 values('" + UserNameLb1.Text + "', " + GrdTotal + ")";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    
                    Con.Close();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprmn", 285, 600);
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
        }
       
        int prodid, prodqty, proprice, tottal, pos = 60;

        private void Billing_Load(object sender, EventArgs e)
        {
            UserNameLb1.Text = Login.UserName;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        string prodname;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("LeafBookShop", new Font("Source Code Pro", 12, FontStyle.Bold), Brushes.Red, new Point(80));
            e.Graphics.DrawString("OredrNum BookName UnitPrice Quantity Amount", new Font("Source Code Pro", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["Column7"].Value);
                prodname = "" + row.Cells["Column8"].Value;
                proprice = Convert.ToInt32(row.Cells["Column9"].Value);
                prodqty = Convert.ToInt32(row.Cells["Column10"].Value);
                tottal = Convert.ToInt32(row.Cells["Column11"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Source Code Pro", 8, FontStyle.Bold), Brushes.Blue, new Point(26, pos));
                e.Graphics.DrawString("" + prodname, new Font("Source Code Pro", 8, FontStyle.Bold), Brushes.Blue, new Point(45, pos));
                e.Graphics.DrawString("" + proprice, new Font("Source Code Pro", 8, FontStyle.Bold), Brushes.Blue, new Point(120, pos));
                e.Graphics.DrawString("" + prodqty, new Font("Source Code Pro", 8, FontStyle.Bold), Brushes.Blue, new Point(170, pos));
                e.Graphics.DrawString("" + tottal, new Font("Source Code Pro", 8, FontStyle.Bold), Brushes.Blue, new Point(235, pos));
                pos = pos + 20;
            }
            e.Graphics.DrawString("Total order amount" + GrdTotal, new Font("Source Code Pro", 12, FontStyle.Bold), Brushes.Crimson, new Point(60, pos + 50));
            e.Graphics.DrawString("********LeafBookShop********" + GrdTotal, new Font("Source Code Pro", 10, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 85));
            BillDGV.Rows.Clear();
            BillDGV.Refresh();
            pos = 100;
            GrdTotal = 0;
        }
        int n = 0, GrdTotal = 0;
        private void AddtoBillBtn_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > stock)
            {
                MessageBox.Show("Inventory shortage");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = BTitleTb.Text;
                newRow.Cells[2].Value = PriceTb.Text;
                newRow.Cells[3].Value = QtyTb.Text;
                newRow.Cells[4].Value = total;
                BillDGV.Rows.Add(newRow);
                n++;
                UpdateBook();
                GrdTotal = GrdTotal + total;
                TotalLb1.Text = "$" + GrdTotal;
            }
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
