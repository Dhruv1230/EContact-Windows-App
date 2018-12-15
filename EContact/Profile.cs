using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EContact
{
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();
        }

        string imgLocation = "";
        SqlCommand cmd;
        
        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Reset reset = new Reset();
            Thread.Sleep(500);
            reset.Show();
        }

        private void pictureBoxBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            EContact eContact = new EContact();
            Thread.Sleep(200);
            eContact.Show();
        }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        SqlConnection conn = new SqlConnection(myconnstrng);

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                byte[] images = null;
                FileStream fileStream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(fileStream);
                images = brs.ReadBytes((int)fileStream.Length);
                conn.Open();
                string sql = "INSERT INTO Profile(Name,Email,JobTitle,ContactNo,Image) VALUES ('" + txtboxName.Text + "','" + txtboxEmail.Text + "','" + txtboxJobTitle.Text + "','" + txtboxContactNumber.Text + "',@images)";
                cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add(new SqlParameter("@images", images));
                int N = cmd.ExecuteNonQuery();
                conn.Close();
                Thread.Sleep(1000);
                MessageBox.Show("Data Saved Successfully!", "Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                EContact eContact = new EContact();
                eContact.Show();
            }
        }

        private bool Validation()
        {
            bool ans = false;

            if (string.IsNullOrEmpty(txtboxEmail.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxEmail, "Email is Required");
            }
            else if (string.IsNullOrEmpty(txtboxJobTitle.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxJobTitle, "Position is Required");
            }
            else if (string.IsNullOrEmpty(txtboxContactNumber.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxContactNumber, "Contact Number is Required");
            }
            else
            {
                ans = true;
                errorProvider1.Clear();
            }
            return ans;
        }

        private void lnkChangeProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|All files(*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = openFileDialog.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;
            }
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            Login login = new Login();
            this.txtboxName.Text = Login.passingvalue;
        }

        private void btnview_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "SELECT Email,JobTitle,ContactNo,Image FROM Profile WHERE Name='" + txtboxName.Text + "'";
            cmd = new SqlCommand(sql, conn);
            SqlDataReader DataRead = cmd.ExecuteReader();
            DataRead.Read();

            if (DataRead.HasRows)
            {
                txtboxEmail.Text = DataRead[0].ToString();
                txtboxJobTitle.Text = DataRead[1].ToString();
                txtboxContactNumber.Text = DataRead[2].ToString();
                byte[] images = ((byte[])DataRead[3]);

                if (images == null)
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    MemoryStream memoryStream = new MemoryStream(images);
                    pictureBox1.Image = Image.FromStream(memoryStream);
                }
            }
            else
            {
                MessageBox.Show("Data not available!","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            conn.Close();
        }

        private void txtboxContactNumber_TextChanged(object sender, EventArgs e)
        {
            this.btnSave.Text = "Update";
        }
    }
}
        
    
