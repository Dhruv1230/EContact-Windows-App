using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EContact.EContactClasses
{
    class ContactClass
    {
        //Get and Set Properties
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        //Select data Method

        public DataTable Select()
        {
            //Database Connection
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();

            try
            {
                //sql query
                string sql = "SELECT * from tbl_contact";
                
                //creating cmd using sql and conn
                SqlCommand cmd = new SqlCommand(sql,conn);
                
                //creating sql dataadapter using cmd
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        //Insert Data Method

        public bool Insert (ContactClass c)
        {
            //default bool value
            bool isSuccess = false;

            //Connect Database
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                string check = @"SELECT count(*) from Login where ContactNo='@ContactNo'";
                //sql query
                string sql = "INSERT INTO tbl_contact (FirstName, LastName, ContactNo, Address, Gender) VALUES (@FirstName, @LastName, @ContactNo, @Address, @Gender)";
                
                //creating cmd using sql and conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlCommand cmda = new SqlCommand(check, conn);

                int count = (int)cmda.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Contact Number Duplicated!", "EContact", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                //Create params to add data
                cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
                cmd.Parameters.AddWithValue("@LastName", c.LastName);
                cmd.Parameters.AddWithValue("@ContactNo", c.ContactNo);
                cmd.Parameters.AddWithValue("@Address", c.Address);
                cmd.Parameters.AddWithValue("@Gender", c.Gender);

                //Connection open
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //Condition check
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false; 
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Update Data Method

        public bool Update(ContactClass c)
        {
            //default bool value
            bool isSuccess = false;
            
            //Connect Database
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //sql query
                string sql = "UPDATE tbl_contact SET FirstName = @FirstName, LastName = @LastName, ContactNo = @ContactNo, Address = @Address, Gender = @Gender WHERE ContactID = @ContactID";

                //creating cmd using sql and conn
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Create params to add data
                cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
                cmd.Parameters.AddWithValue("@LastName", c.LastName);
                cmd.Parameters.AddWithValue("@ContactNo", c.ContactNo);
                cmd.Parameters.AddWithValue("@Address", c.Address);
                cmd.Parameters.AddWithValue("@Gender", c.Gender);
                cmd.Parameters.AddWithValue("ContactID", c.ContactID);

                //Connection open
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //Condition check
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Delete Data Method

        public bool Delete(ContactClass c)
        {
            //defult bool value
            bool isSuccess = false;

            //Connect Database
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                //sql query
                string sql = "DELETE FROM tbl_contact where ContactID = @ContactID";

                //sql command
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("ContactID", c.ContactID);

                //Open Connection
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //Condition check
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }
    }
}