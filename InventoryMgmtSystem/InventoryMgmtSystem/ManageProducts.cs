using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryMgmtSystem
{
    public partial class ManageProducts : Form
    {
        public ManageProducts()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\gherl\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
        void fillcategory()
        {
            string query = "select * from CategoryTbl";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader rdr;
            try
            {
                Con.Open();
                DataTable dt = new DataTable();
                dt.Columns.Add("CatName", typeof(string));
                rdr = cmd.ExecuteReader();
                dt.Load(rdr);
                CatCombo.ValueMember = "CatName";
                CatCombo.DataSource = dt;
                SearchCombo.ValueMember = "CatName";
                SearchCombo.DataSource = dt;
                Con.Close();
            }
            catch
            {

            }
        }

        //void fillSearchcombo()
        //{
        //    string query = "select * from CategoryTbl where CatName='"+SearchCombo.SelectedValue.ToString()+"'";
        //    SqlCommand cmd = new SqlCommand(query, Con);
        //    SqlDataReader rdr;
        //    try
        //    {
        //        Con.Open();
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add("CatName", typeof(string));
        //        rdr = cmd.ExecuteReader();
        //        dt.Load(rdr);
        //        CatCombo.ValueMember = "CatName";
        //        CatCombo.DataSource = dt;
        //        Con.Close();
        //    }
        //    catch
        //    {

        //    }
        //}
        private void ManageProducts_Load(object sender, EventArgs e)
        {
            fillcategory();
            populate();
        }

        void populate()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from ProductTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                ProductsGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            catch
            {

            }
        }

        void filterbycategory()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from ProductTbl where ProdCat='"+SearchCombo.SelectedValue.ToString()+"'";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                ProductsGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into ProductTbl values('" + ProdIdTb.Text + "','" + ProdNameTb.Text + "','" + QtyTb.Text + "','" + PriceTb.Text + "','" + DescriptionTb.Text + "','" + CatCombo.SelectedValue.ToString() + "')", Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product Successfully Added");
                Con.Close();
                populate();
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ProdIdTb.Text == "")
            {
                MessageBox.Show("Enter The Porduct Id");
            }
            else
            {
                Con.Open();
                string myquery = "delete from ProductTbl where ProdId =" + ProdIdTb.Text + ";";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product Successfully Deleted");
                Con.Close();
                populate();
            }
        }

        private void ProductsGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string productId;
            string productName;
            string productQty;
            string productPrice;
            string productDescription;
            string productCategory;

            DataGridViewRow selectRow = ProductsGV.Rows[e.RowIndex];

            productId = selectRow.Cells[0].Value.ToString();
            productName = selectRow.Cells[1].Value.ToString();
            productQty = selectRow.Cells[2].Value.ToString();
            productPrice = selectRow.Cells[3].Value.ToString();
            productDescription = selectRow.Cells[4].Value.ToString();
            productCategory = selectRow.Cells[5].Value.ToString();

            ProdIdTb.Text = productId;
            ProdNameTb.Text = productName;
            QtyTb.Text = productQty;
            PriceTb.Text = productPrice;
            DescriptionTb.Text = productDescription;
            CatCombo.SelectedValue = productCategory;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("update ProductTbl set ProdName = '" + ProdNameTb.Text + "',ProdQty='" + QtyTb.Text + "',ProdPrice='" + PriceTb.Text + "',ProdDesc='" + DescriptionTb.Text + "',ProdCat='" + CatCombo.SelectedValue.ToString() + "' where ProdId = '" + ProdIdTb.Text + "'", Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product Successfully Updated");
                Con.Close();
                populate();
            }
            catch
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            filterbycategory();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
    }
}
