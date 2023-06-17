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
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\gherl\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True");
        private void label3_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
        void populate()
        {
            try
            {
                Con.Open();
                string Myquery = "select * from UserTbl";
                SqlDataAdapter da = new SqlDataAdapter(Myquery, Con);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                var ds = new DataSet();
                da.Fill(ds);
                UsersGV.DataSource = ds.Tables[0];
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
                SqlCommand cmd = new SqlCommand("insert into UserTbl values('"+unameTb.Text+ "','" + FnameTb.Text + "','"+PasswordTb.Text+"','"+PhoneTb.Text+"')",Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("User Successfully Added");
                Con.Close();
                populate();
            }
            catch
            {

            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(PhoneTb.Text == "")
            {
                MessageBox.Show("Enter The Users Phone Number");
            }
            else
            {
                Con.Open();
                string myquery = "delete from UserTbl where UPhone ='" + PhoneTb.Text + "';";
                SqlCommand cmd = new SqlCommand(myquery, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("User Successfully Deleted");
                Con.Close();
                populate();
            }
        }

        private void UsersGV_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //unameTb.Text = UsersGV.SelectedRows[e.RowIndex].Cells[0].Value.ToString();
            //FnameTb.Text = UsersGV.SelectedRows[e.RowIndex].Cells[1].Value.ToString();
            //PasswordTb.Text = UsersGV.SelectedRows[e.RowIndex].Cells[2].Value.ToString();
            //PhoneTb.Text = UsersGV.SelectedRows[e.RowIndex].Cells[3].Value.ToString();

            string uname;
            string Fname;
            string Password;
            string Phone;

            DataGridViewRow selectRow = UsersGV.Rows[e.RowIndex];

            uname = selectRow.Cells[0].Value.ToString();
            Fname = selectRow.Cells[1].Value.ToString();
            Password = selectRow.Cells[2].Value.ToString();
            Phone = selectRow.Cells[3].Value.ToString();

            unameTb.Text = uname;
            FnameTb.Text = Fname;
            PasswordTb.Text = Password;
            PhoneTb.Text = Phone;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("update UserTbl set Uname = '"+unameTb.Text+"',Ufullname='"+FnameTb.Text+"',Upassword='"+PasswordTb.Text+"' where UPhone = '"+PhoneTb.Text+"'", Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("User Successfully Updated");
                Con.Close();
                populate();
            }
            catch
            {

            }
        }

        private void unameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void FnameTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void PasswordTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }
    }
}
