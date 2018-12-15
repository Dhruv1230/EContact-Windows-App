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
    public partial class Login : Form
    {
        public static string passingvalue;
        public Login()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //Clear theData and Focus
            this.Hide();
            Register register = new Register();
            register.Show();
        }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
      
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                //Database Connectivity
                SqlConnection conn = new SqlConnection(myconnstrng);

                //sql query
                SqlDataAdapter sda = new SqlDataAdapter(@"SELECT * FROM[EConact].[dbo].[Login] WHERE UserName = '" + txtboxUsername.Text + "' and Password = '" + txtboxPassword.Text + "'", conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                //Validation
                if (dt.Rows.Count == 1)
                {
                    this.Hide();
                    passingvalue = txtboxUsername.Text;
                    EContact eContact = new EContact();
                    eContact.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtboxUsername.Clear();
                    txtboxPassword.Clear();
                    txtboxUsername.Focus();
                }
            }
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
            else
            {
                ans = true;
                errorProvider1.Clear();
            }
            return ans;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            passingvalue = txtboxUsername.Text;
            Reset reset = new Reset();
            reset.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Register register = new Register();
            this.txtboxUsername.Text = Register.passingvalue_Register;
        }
    }
}
