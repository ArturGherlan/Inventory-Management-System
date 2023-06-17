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
    public partial class ManageOrders : Form
    {
        public ManageOrders()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\gherl\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
        void populate()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from CustomerTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                CustomersGV.DataSource = ds.Tables[0];
                Con.Close();
            }
            catch
            {

            }
        }
        void populateproducts()
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
                //CatCombo.ValueMember = "CatName";
                //CatCombo.DataSource = dt;
                SearchCombo.ValueMember = "CatName";
                SearchCombo.DataSource = dt;
                Con.Close();
            }
            catch
            {

            }
        }
        void updateproduct()
        {
            
            int id = Convert.ToInt32(selectedRow.Cells[0].Value.ToString());
            int newQty = stock - Convert.ToInt32(QtyTb.Text);
            if (newQty < 0)
                MessageBox.Show("Operation Failed");
            else
            {
                Con.Open();
                string query = "update ProductTbl set ProdQty = " + newQty + "where ProdId=" + id + "";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                Con.Close();
                populateproducts();
            }
        }
        int num = 0;
        int uprice, totprice, qty;
        string product;
        DataTable table = new DataTable();

        private void ManageOrders_Load(object sender, EventArgs e)
        {
            populate();
            populateproducts();
            fillcategory();

            table.Columns.Add("Num", typeof(int));
            table.Columns.Add("Porduct", typeof(string));
            table.Columns.Add("Qty", typeof(int));
            table.Columns.Add("Unit Price", typeof(int));
            table.Columns.Add("Total Price", typeof(int));

            OrderGV.DataSource = table;
        }
        int flag = 0;
        int stock;
        DataGridViewRow selectedRow;
        private void ProductsGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string prod;
            //int quantity;
            int uPrice;
            

            DataGridViewRow selectRow = ProductsGV.Rows[e.RowIndex];
            selectedRow = selectRow;

            prod = selectRow.Cells[1].Value.ToString();
            //quantity = Convert.ToInt32(selectRow.Cells[2].Value.ToString());
            uPrice = Convert.ToInt32(selectRow.Cells[3].Value.ToString());
            

            product = prod;
            //qty = Convert.ToInt32(QtyTb.Text);
            stock = Convert.ToInt32(selectRow.Cells[2].Value.ToString());
            uprice = uPrice;
            //totprice = qty * uprice;
            flag = 1;

        }

        int sum = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text == "")
                MessageBox.Show("Enter The Quantity of Products");
            else if (flag == 0)
                MessageBox.Show("Select The Product");
            else if (Convert.ToInt32(QtyTb.Text) > stock)
                MessageBox.Show("Not Enough Stock Available");
            else
            {
                num = num + 1;
                qty = Convert.ToInt32(QtyTb.Text);
                totprice = qty * uprice;
                table.Rows.Add(num, product, qty, uprice, totprice);
                OrderGV.DataSource = table;
                flag = 0;
            }
            sum = sum + totprice;
            TotAmount.Text = sum.ToString();
            updateproduct();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (OrderIdTb.Text == "" || CustId.Text == "" || CustName.Text == ""||TotAmount.Text=="")
            {
                MessageBox.Show("Fill The data Correctly");
            }
            else
            {
               
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into OrderTbl values(" + OrderIdTb.Text + "," + CustId.Text + ",'" + CustName.Text + "','" + orderdate.Text + "'," + TotAmount.Text + ")", Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Order Added Successfully");
                    Con.Close();
                    //populate();
                try
                {

                }
                catch
                {

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewOrders view = new ViewOrders();
            view.Show();
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

        private void CustomersGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string customerId;
            string customerName;

            DataGridViewRow selectRow = CustomersGV.Rows[e.RowIndex];

            customerId = selectRow.Cells[0].Value.ToString();
            customerName = selectRow.Cells[1].Value.ToString();

            CustId.Text = customerId;
            CustName.Text = customerName;
        }

        private void SearchCombo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string Myquery = "select * from ProductTbl where ProdCat='" + SearchCombo.SelectedValue.ToString() + "'";
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
    }
}
