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
    public partial class StudentRegistration : Form
    {
        SqlConnection Conn;
        string Connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        string validationMessage;
        DataTable dt;
        Common common;
        public StudentRegistration()
        {   
            InitializeComponent();
            Conn = new SqlConnection(Connection);
            common = new Common();
            FillCountry();
        }
        private void FillCountry()
        {
            try
            {
                //Conn.Open();
                //SqlCommand cmd = new SqlCommand("Select CountryId,CountryName from Country", Conn);
                //cmd.ExecuteNonQuery();
                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //dt = new DataTable();
                //adapter.Fill(dt);
                //DataRow row = dt.NewRow();
                //row[0] = "0";
                //row[1] = "Please Select";
                //dt.Rows.InsertAt(row, 0);
                //Common common = new Common();
                DataTable dtCountry = common.FillCountry();
                comboBoxCountry.DataSource = dtCountry;
                comboBoxCountry.ValueMember = "CountryId";
                comboBoxCountry.DisplayMember = "CountryName";
              //  Conn.Close();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void FillStateByCountryId(int countryId)
        {
            try
            {
                //Common common = new Common();
                DataTable dtState = common.GetStateByCountryId(countryId);
                comboBoxState.DataSource = dtState;
                comboBoxState.ValueMember = "StateId";
                comboBoxState.DisplayMember = "StateName";
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void FillCityByStateId(int stateId)
        {
            try
            {
                DataTable dtCity = common.GetCityByStateId(stateId);
                comboBoxCity.DataSource = dtCity;
                comboBoxCity.ValueMember = "CityId";
                comboBoxCity.DisplayMember = "CityName";

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
        private void ClearState()
        {
            dt = new DataTable();
            DataColumn firstColumn, secondColumn;
            firstColumn = new DataColumn();
            firstColumn.DataType = typeof(int);
            firstColumn.ColumnName = "StateId";
            secondColumn = new DataColumn();
            secondColumn.DataType = typeof(string);
            secondColumn.ColumnName = "StateName";
            dt.Columns.Add(firstColumn);
            dt.Columns.Add(secondColumn);
            DataRow row = dt.NewRow();
            row[0] = "0";
            row[1] = "Please Select";
            dt.Rows.Add(row);
            comboBoxState.DataSource = dt;
        }
        private void ClearCity()
        {
            dt = new DataTable();
            DataColumn firstColumn, secondColumn;
            firstColumn = new DataColumn();
            firstColumn.DataType = typeof(int);
            firstColumn.ColumnName = "CityId";
            secondColumn = new DataColumn();
            secondColumn.DataType = typeof(string);
            secondColumn.ColumnName = "CityName";
            dt.Columns.Add(firstColumn);
            dt.Columns.Add(secondColumn);
            DataRow row = dt.NewRow();
            row[0] = "0";
            row[1] = "Please Select";
            dt.Rows.Add(row);
            comboBoxCity.DataSource = dt;
        }
        private bool Validation()
        {
            bool result = true;
            validationMessage = string.Empty;

            if ((string.IsNullOrEmpty(txtName.Text)))
            {
                validationMessage = "Please enter name. ";
            }
            if ((string.IsNullOrEmpty(txtEmail.Text)))
            {
                validationMessage += "\nPlease enter email. ";
            }
            if ((string.IsNullOrEmpty(txtUserName.Text)))
            {
                validationMessage += "\nPlease enter the username. ";
            }
            if ((string.IsNullOrEmpty(txtPassword.Text)))
            {
                validationMessage += "\nPlease enter the password. ";
            }
            if (!string.IsNullOrEmpty(validationMessage))
            {
                result = false;
            }


            return result;
        }
        private string SelectGender()
        {
            string gender;

            if (rbtnMale.Checked)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            return gender;
        }
        private string SelectLanguage()
        {
            string language = string.Empty;
            if (checkBoxEnglish.Checked)
            {

                language = "English,";
            }
            if (checkBoxHindi.Checked)
            {
                language += "Hindi,";
            }
            if (checkBoxFrench.Checked)
            {
                language += "French,";
            }
            if (checkBoxSanskrit.Checked)
            {
                language += "Sanskrit";
            }

            return language;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("Select UserName from StudentInfo where UserName=@UserName", Conn);
                cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    bool result = Validation();
                    string name = Convert.ToString(txtName.Text);
                    string email = Convert.ToString(txtEmail.Text);
                    string username = Convert.ToString(txtUserName.Text);
                    string password = Convert.ToString(txtPassword.Text);
                    string gender = SelectGender();
                    string address = Convert.ToString(txtAddress.Text);
                    string countryId = Convert.ToString(comboBoxCountry.SelectedValue.ToString());
                    string stateId = Convert.ToString(comboBoxState.SelectedValue.ToString());
                    string cityId = Convert.ToString(comboBoxCity.SelectedValue.ToString());
                    string language = SelectLanguage();
                    string date = Convert.ToString(dateTimePicker1.Text);
                    string description = Convert.ToString(richTextBoxDesBox.Text);
                    if (result)
                    {
                        try
                        {
                            string sqlQuery = "insert into StudentInfo (Name,Email,UserName,Password,Gender," +
                                 "Address,CountryId,StateId,CityId,Language,Date,Description) " +
                                 "values('" + name + "','" + email + "','" + username + "','" + password + "','"
                                 + gender + "','" + address + "'," +
                                 "'" + countryId + "','" + stateId + "','" + cityId + "','" + language + "','"
                                 + date + "','" + description + "')";
                            SqlCommand command = new SqlCommand(sqlQuery, Conn);
                            command.ExecuteNonQuery();
                            Clear();
                            MessageBox.Show("Form submitted successfully");

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
                    else
                    {
                        MessageBox.Show(validationMessage);
                    }
                }
                else
                {
                    MessageBox.Show("User Already exists");
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
        private void Clear()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            rbtnMale.Checked = false;
            rbtnFemale.Checked = false;
            txtAddress.Clear();
            comboBoxCountry.SelectedIndex = 0;
            ClearState();
            ClearCity();
            //comboBoxState.Enabled = false;
            //comboBoxCity.Enabled = false;
            checkBoxHindi.Checked = false;
            checkBoxSanskrit.Checked = false;
            checkBoxEnglish.Checked = false;
            checkBoxFrench.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
            richTextBoxDesBox.Clear();
            validationMessage = string.Empty;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void comboBoxCountry_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCountry.SelectedValue.ToString() != "0")
                {
                    int selectedCountryId = (int)comboBoxCountry.SelectedValue;
                    FillStateByCountryId(selectedCountryId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void comboBoxState_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxState.SelectedValue.ToString() != "0")
                {
                    int selectedStateId = (int)comboBoxState.SelectedValue;
                    FillCityByStateId(selectedStateId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void logInFormLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StudentLogin studentLogin = new StudentLogin();
            studentLogin.Show();
            this.Hide();
        }
    }
}
