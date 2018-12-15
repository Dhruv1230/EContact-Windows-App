using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EContact
{
    public partial class Register : Form
    {
        public static string passingvalue_Register;

        public Register()
        {
            InitializeComponent();
        }

        private bool Validation()
        {
            bool ans = false;

            if (string.IsNullOrEmpty(txtboxUsername.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxUsername, "Username is Required");
            }
            else if (string.IsNullOrEmpty(txtboxPassword.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxPassword, "Password is Required");
            }
            else if (string.IsNullOrEmpty(txtboxConfPassword.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxConfPassword, "Re-enter Password to Verify");
            }
            else if (txtboxPassword.Text != txtboxConfPassword.Text)
            {
                MessageBox.Show("Password Do not Match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtboxPassword.Clear();
                txtboxConfPassword.Clear();
                txtboxPassword.Focus();
            }
            else
            {
                ans = true;
                errorProvider1.Clear();
            }
            return ans;
        }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //Database Connection
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                DataSet ds = new DataSet();

                if (Validation())
                {
                    string check = @"SELECT count(*) from Login where UserName='" + txtboxUsername.Text + "'";
                    SqlCommand cmd = new SqlCommand("UserAdd", conn);
                    conn.Open();
                    SqlCommand cmda = new SqlCommand(check, conn);

                    int count = (int)cmda.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("User already Exist!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtboxUsername.Clear();
                        txtboxPassword.Clear();
                        txtboxConfPassword.Clear();
                        txtboxUsername.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Registered Successfully! Now You can Login!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        passingvalue_Register = txtboxUsername.Text;
                        Login login = new Login();
                        login.Show();
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", txtboxUsername.Text);
                    cmd.Parameters.AddWithValue("@Password", txtboxPassword.Text);
                }
            }
        }

        private void pictureBoxBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }
    }
}