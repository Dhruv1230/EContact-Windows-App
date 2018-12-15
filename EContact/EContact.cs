using EContact.EContactClasses;
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
using System.Threading;

namespace EContact
{
    public partial class EContact : Form
    {
        public EContact()
        {
            InitializeComponent();
        }
        ContactClass c = new ContactClass();

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Validation())
            {

                //Get the value from the input fields
                c.FirstName = txtboxFirstName.Text;
                c.LastName = txtboxLastName.Text;
                c.ContactNo = txtboxContactNumber.Text;
                c.Address = txtboxAddress.Text;
                c.Gender = cmbGender.Text;

                //Inserting Data into DAtabase uing the method we created in previous episode
                bool success = c.Insert(c);
                if (success == true)
                {
                    //Successfully Inserted
                    MessageBox.Show("New Contact Successfully Inserted","EContact",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    //Call the Clear Method Here
                    Clear();
                }
                else
                {
                    //FAiled to Add Contact
                    MessageBox.Show("Failed to add New Contact. Duplicated Contact Value.","EContact",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    Clear();
                }
                //Load Data on Data GRidview
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
            }
        }

        private bool Validation()
        {
            bool ans = false;

            if (string.IsNullOrEmpty(txtboxFirstName.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxFirstName, "First Name is Required");
            }
            else if (string.IsNullOrEmpty(txtboxLastName.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxLastName, "Last Name is Required");
            }
            else if (string.IsNullOrEmpty(txtboxContactNumber.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxContactNumber, "Contact Number is Required");
            }
            else if (string.IsNullOrEmpty(txtboxAddress.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtboxAddress, "Address is Required");
            }
            else if (cmbGender.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(cmbGender, "Select Gender");
            }
            else
            {
                ans = true;
                errorProvider1.Clear();
            }
            return ans;
        }

        //Method to Clear Fields
        public void Clear()
        {
            txtboxFirstName.Text = "";
            txtboxLastName.Text = "";
            txtboxContactNumber.Text = "";
            txtboxAddress.Text = "";
            cmbGender.Text = "";
            txtboxContactID.Text = "";
        }

        private void dgvContactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the DAta From DAta Grid View and Load it to the textboxes respectively
            //identify the row on which mouse is clicked
            int rowIndex = e.RowIndex;
            txtboxContactID.Text = dgvContactList.Rows[rowIndex].Cells[0].Value.ToString();
            txtboxFirstName.Text = dgvContactList.Rows[rowIndex].Cells[1].Value.ToString();
            txtboxLastName.Text = dgvContactList.Rows[rowIndex].Cells[2].Value.ToString();
            txtboxContactNumber.Text = dgvContactList.Rows[rowIndex].Cells[3].Value.ToString();
            txtboxAddress.Text = dgvContactList.Rows[rowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dgvContactList.Rows[rowIndex].Cells[5].Value.ToString();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            //Get the Data from textboxes
            c.ContactID = int.Parse(txtboxContactID.Text);
            c.FirstName = txtboxFirstName.Text;
            c.LastName = txtboxLastName.Text;
            c.ContactNo = txtboxContactNumber.Text;
            c.Address = txtboxAddress.Text;
            c.Gender = cmbGender.Text;
            //Update DAta in Database
            bool success = c.Update(c);
            if (success == true)
            {
                //Updated Successfully
                MessageBox.Show("Contact has been successfully Updated.","EContact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Load Data on Data GRidview
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
                //Call Clear Method
                Clear();
            }
            else
            {
                //Failed to Update
                MessageBox.Show("Failed to Update Contact.Try Again.","EContact", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            //Call Clear Method Here
            Clear();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            //Confirmation
            DialogResult dialogResult = MessageBox.Show("Are You Sure You Want To Delete?","EContact",MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                //Get the Contact ID fromt eh Application
                c.ContactID = Convert.ToInt32(txtboxContactID.Text);
                bool success = c.Delete(c);
                if (success == true)
                {
                    //Successfully Deleted
                    MessageBox.Show("Contact successfully deleted.", "EContact", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Refresh Data GridView
                    //Load Data on Data GRidview
                    DataTable dt = c.Select();
                    dgvContactList.DataSource = dt;
                    //CAll the Clear Method Here
                    Clear();
                }
                else
                {
                    //FAiled to dElte
                    MessageBox.Show("Failed to Delete Contact. Try Again.", "EContact", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        static string myconnstr = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {
            //Get teh value from text box
            string keyword = txtboxSearch.Text;

            SqlConnection conn = new SqlConnection(myconnstr);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_contact WHERE FirstName LIKE '%" + keyword + "%' OR LastName LIKE '%" + keyword + "%' OR Address LIKE '%" + keyword + "%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvContactList.DataSource = dt;
        }

        private void EContact_Load(object sender, EventArgs e)
        {
            lblName.Text = Login.passingvalue;
            //Load Data on Data GRidview
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;
        }
        
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        private void txtboxSearch_TextChanged_1(object sender, EventArgs e)
        {
            //Get the value from text box
            string keyword = txtboxSearch.Text;
            SqlConnection conn = new SqlConnection(myconnstrng);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_contact WHERE FirstName LIKE '%" + keyword + "%' OR LastName LIKE '%" + keyword + "%' OR Address LIKE '%" + keyword + "%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvContactList.DataSource = dt;
        }

        //Confirmation to exit Application
        bool close = true;

        private void EContact_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
            {
                DialogResult dialogResult = MessageBox.Show("Are You Sure You Want To Log Out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    close = false;
                    Thread.Sleep(1100);
                    this.Hide();
                    Login login = new Login();
                    login.Show();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void txtboxSearch_Enter(object sender, EventArgs e)
        {
            if (txtboxSearch.Text == "Search Here...")
            {
                txtboxSearch.Clear();
                txtboxSearch.ForeColor = Color.Black;
            }
        }

        private void txtboxSearch_Leave(object sender, EventArgs e)
        {
            if (txtboxSearch.Text == "")
            {
                txtboxSearch.Text = "Search Here...";
                txtboxSearch.ForeColor = Color.Silver;
            }
        }

        private void pictureboxUser_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.pictureboxUser,Login.passingvalue);
        }

        private void pictureboxUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            Profile profile = new Profile();
            profile.Show();
        }
    }
}