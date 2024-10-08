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
using System.Configuration;

namespace StudentPortal
{
    public partial class StudentLogin : Form
    {
        SqlConnection Conn;
        string Connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        string validationMessage;
        public StudentLogin()
        {
            InitializeComponent();
            Conn = new SqlConnection(Connection);
        }

        private bool Validation()
        {
            bool result = true;
            validationMessage = string.Empty;
            if (String.IsNullOrEmpty(txtUserName.Text))
            {
                validationMessage = "Please enter user name";
            }
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                validationMessage += "\nPlease enter password";
            }
            if (!string.IsNullOrEmpty(validationMessage))
            {
                result = false;
            }
            return result;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            StudentRegistration studentObj = new StudentRegistration();
            studentObj.Show();
            this.Hide();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            bool pass = Validation();
            try
            {
                if (pass)
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("Select UserName,Password from StudentInfo where UserName =@UserName and Password =@Password", Conn);
                    cmd.Parameters.AddWithValue("UserName", txtUserName.Text);
                    cmd.Parameters.AddWithValue("Password", txtPassword.Text);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        StudentDashboard studentObj = new StudentDashboard();
                        studentObj.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Fill the correct info");
                    }

                }
                else
                {
                    MessageBox.Show(validationMessage);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Conn.Close();
            }
            

        }
    }
}

