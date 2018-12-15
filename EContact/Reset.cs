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
    public partial class Reset : Form
    {
        public Reset()
        {
            InitializeComponent();
        }

        private void pictureBoxBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
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
            else if (string.IsNullOrEmpty(txtboxSecretCode.Text))
            {
                errorProvider1.Clear();
                MessageBox.Show("Secret Code is Require to Reset Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (txtboxSecretCode.Text != "Secret123")
            {
                MessageBox.Show("Secret Code does not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtboxSecretCode.Clear();
                txtboxSecretCode.Focus();
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
            //default bool value
            bool isSuccess = false;

            //Connect Database
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                if (Validation())
                {
                    string sql = "UPDATE Login SET Password = @Password WHERE UserName = @UserName";

                    //creating cmd using sql and conn
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    //Create params to add data
                    cmd.Parameters.AddWithValue("@UserName", txtboxUsername.Text);
                    cmd.Parameters.AddWithValue("@Password", txtboxPassword.Text);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    //Condition check
                    if (rows > 0)
                    {
                        isSuccess = true;
                        MessageBox.Show("Password Changed Successfully!", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Login login = new Login();
                        login.Show();
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }
            }   
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }
    }
}