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
using System.Globalization;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StudentPortal
{
    public partial class StudentDashboard : Form
    {
        SqlConnection Conn;
        string Connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        int Id;
        string Message;
        Common Common;
        public StudentDashboard()
        {
            InitializeComponent();
            Common = new Common();
            Conn = new SqlConnection(Connection);
            FillCountry();
            Display();
        }
        private void Display()
        {
            try
            {
                Conn.Open();
                //string sqlQuery = "Select Id,Name,Email,UserName,Password,Gender,Address," +
                //    "CountryId,StateId,CityId,Language,Date,Description from StudentInfo";
                string sqlQuery = "select st.Id,st.Name as FullName,st.Email,st.UserName,st.Password," +
                    "st.Gender,st.Address,st.CountryId,st.StateId,st.CityId,st.Language," +
                    "st.Date,st.Description,co.CountryName,sta.StateName," +
                    "ci.cityName from StudentInfo st " +
                    "left outer join Country co on st.CountryId = co.CountryId" +
                    " left outer join State sta on st.StateId = sta.StateId " +
                    "left outer join City ci on st.CityId = ci.CityId";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gridViewStudentDashBord.DataSource = dt;
                Conn.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillCountry()
        {
            try
            {
                //Conn.Open();
                //SqlCommand cmd = new SqlCommand("Select CountryId,CountryName from Country", Conn);
                //cmd.ExecuteNonQuery();
                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //adapter.Fill(dt);

                //DataRow row = dt.NewRow();
                //row[0] = "0";
                //row[1] = "Please Select";
                //dt.Rows.InsertAt(row, 0);
                DataTable dtCountry = Common.FillCountry();
                comboBoxCountry.DataSource = dtCountry;
                comboBoxCountry.ValueMember = "CountryId";
                comboBoxCountry.DisplayMember = "CountryName";

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
        
        
        private void FillStateByCountryId(int countryId)
        {
            try
            {
                //Conn.Open();
                //SqlCommand cmd = new SqlCommand("Select StateId,CountryId,StateName from State where CountryId=@CountryId", Conn);
                //cmd.Parameters.AddWithValue("@CountryId", countryId);
                //cmd.ExecuteNonQuery();
                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //adapter.Fill(dt);
                //DataRow row = dt.NewRow();
                //row[0] = "0";
                //row[1] = "0";
                //row[2] = "Please Select";
                //dt.Rows.InsertAt(row, 0);
                //Common common = new Common();
                DataTable dtState = Common.GetStateByCountryId(countryId);
                comboBoxState.DataSource = dtState;
                comboBoxState.ValueMember = "StateId";
                comboBoxState.DisplayMember = "StateName";

                //Conn.Close();
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
                //Conn.Open();
                //SqlCommand cmd = new SqlCommand("Select CityId,StateId,CityName from City where StateId=@StateId ", Conn);
                //cmd.Parameters.AddWithValue("@StateId", stateId);
                //cmd.ExecuteNonQuery();
                //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //adapter.Fill(dt);
                //DataRow row = dt.NewRow();
                //row[0] = "0";
                //row[1] = "0";
                //row[2] = "Please Select";
                //dt.Rows.InsertAt(row, 0);
                DataTable dtCity = Common.GetCityByStateId(stateId);
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


        #region Combobox Change Event
        private void comboBoxCountry_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxCountry.SelectedValue.ToString() != "0")
                {
                    int seletedCountryId = Convert.ToInt32(comboBoxCountry.SelectedValue.ToString());
                    FillStateByCountryId(seletedCountryId);
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
                    int selectedStateId = Convert.ToInt32(comboBoxState.SelectedValue.ToString());
                    FillCityByStateId(selectedStateId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
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
            if (checkBoxHindi.Checked)
            {
                language = "Hindi,";
            }
            if (checkBoxEnglish.Checked)
            {
                language += "English,";
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
        private void ClearState()
        {
            DataTable dt = new DataTable();
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
            DataTable dt = new DataTable();
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
            Message = string.Empty;

            if ((string.IsNullOrEmpty(txtName.Text)))
            {
                Message = "Please enter name. ";
            }
            if ((string.IsNullOrEmpty(txtEmail.Text)))
            {
                Message += "\nPlease enter email. ";
            }
            if ((string.IsNullOrEmpty(txtUserName.Text)))
            {
                Message += "\nPlease enter the username. ";
            }
            if ((string.IsNullOrEmpty(txtPassword.Text)))
            {
                Message += "\nPlease enter the password. ";
            }
            if (!string.IsNullOrEmpty(Message))
            {
                result = false;
            }


            return result;
        }

        private void Clear()
        {
            Id = 0;
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
            checkBoxHindi.Checked = false;
            checkBoxSanskrit.Checked = false;
            checkBoxEnglish.Checked = false;
            checkBoxFrench.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
            richTextBoxDesBox.Clear();
            Message = string.Empty;
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool pass = Validation();
                if (pass)
                {
                    Conn.Open();
                    SqlCommand cmd = new SqlCommand("Select UserName from StudentInfo where UserName=@UserName", Conn);
                    cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        try
                        {
                            string name = Convert.ToString(txtName.Text);
                            string email = Convert.ToString(txtEmail.Text);
                            string userName = Convert.ToString(txtUserName.Text);
                            string password = Convert.ToString(txtPassword.Text);
                            string gender = SelectGender();
                            string address = Convert.ToString(txtAddress.Text);
                            string countryId = Convert.ToString(comboBoxCountry.SelectedValue.ToString());
                            string stateId = Convert.ToString(comboBoxState.SelectedValue.ToString());
                            string cityId = Convert.ToString(comboBoxCity.SelectedValue.ToString());
                            string language = SelectLanguage();
                            string date = Convert.ToString(dateTimePicker1.Text);
                            string description = Convert.ToString(richTextBoxDesBox.Text);
                            string sqlQuery = "Insert into StudentInfo(Name,Email,UserName,Password,Gender," +
                                "Address,CountryId,StateId,CityId,Language,Date,Description) " +
                                "values('" + name + "','" + email + "','" + userName + "','" + password + "','" + gender + "','" + address + "'," +
                                "'" + countryId + "','" + stateId + "','" + cityId + "','" + language + "','" + date + "','" + description + "')";
                            SqlCommand command = new SqlCommand(sqlQuery, Conn);
                            command.ExecuteNonQuery();
                            Conn.Close();
                            Clear();
                            Display();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        MessageBox.Show("User Already exist");
                    }
                }
                else
                {
                    MessageBox.Show(Message);
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

        private void gridViewStudentDashBord_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {

                Id = Convert.ToInt32(gridViewStudentDashBord.Rows[e.RowIndex].Cells[0].Value.ToString());
                txtName.Text = gridViewStudentDashBord.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtEmail.Text = gridViewStudentDashBord.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtUserName.Text = gridViewStudentDashBord.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtPassword.Text = gridViewStudentDashBord.Rows[e.RowIndex].Cells[4].Value.ToString();
                if (gridViewStudentDashBord.Rows[e.RowIndex].Cells[5].Value.ToString().Equals("Male"))
                {
                    rbtnMale.Checked = true;
                }
                else
                {
                    rbtnFemale.Checked = true;
                }
                txtAddress.Text = gridViewStudentDashBord.Rows[e.RowIndex].Cells[6].Value.ToString();
                int selectedCountryId = Convert.ToInt32(gridViewStudentDashBord.Rows[e.RowIndex].Cells[7].Value.ToString());
                comboBoxCountry.SelectedValue = selectedCountryId;
                FillStateByCountryId(selectedCountryId);
                int selectedStateId = Convert.ToInt32(gridViewStudentDashBord.Rows[e.RowIndex].Cells[8].Value.ToString());
                comboBoxState.SelectedValue = selectedStateId;
                FillCityByStateId(selectedStateId);
                int selectedCityId = Convert.ToInt32(gridViewStudentDashBord.Rows[e.RowIndex].Cells[9].Value.ToString());
                comboBoxCity.SelectedValue = selectedCityId;


                string[] languages = gridViewStudentDashBord.Rows[e.RowIndex].Cells[10].Value.ToString().Split(',');
                if (languages != null && languages.Length > 0)
                {
                    foreach (string language in languages)
                    {
                        if (languages.Contains("English"))
                        {
                            checkBoxEnglish.Checked = true;
                        }
                        if (languages.Contains("Hindi"))
                        {
                            checkBoxHindi.Checked = true;
                        }
                        if (languages.Contains("French"))
                        {
                            checkBoxFrench.Checked = true;
                        }
                        if (languages.Contains("Sanskrit"))
                        {
                            checkBoxSanskrit.Checked = true;
                        }
                    }
                }
                string time = (gridViewStudentDashBord.Rows[e.RowIndex].Cells[11].Value.ToString());
                DateTime date2 = Convert.ToDateTime(time, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                dateTimePicker1.Value = date2;
                richTextBoxDesBox.Text = gridViewStudentDashBord.Rows[e.RowIndex].Cells[12].Value.ToString();
                Display();
                txtUserName.ReadOnly = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            string name = Convert.ToString(txtName.Text);
            string email = Convert.ToString(txtEmail.Text);
            string userName = Convert.ToString(txtUserName.Text);
           // txtUserName.ReadOnly = true;
            string password = Convert.ToString(txtPassword.Text);
            string gender = SelectGender();
            string address = Convert.ToString(txtAddress.Text);
            string countryId = Convert.ToString(comboBoxCountry.SelectedValue.ToString());
            string stateId = Convert.ToString(comboBoxState.SelectedValue.ToString());
            string cityId = Convert.ToString(comboBoxCity.SelectedValue.ToString());
            string language = SelectLanguage();
            string date = Convert.ToString(dateTimePicker1.Text);
            string description = Convert.ToString(richTextBoxDesBox.Text);

            try
            {
                Conn.Open();
                string sqlQuery = "Update StudentInfo set Name ='" + name + "',Email='" + email + "'," +
                    "UserName ='" + userName + "',Password='" + password + "',Gender='" + gender + "',Address='" + address + "'," +
                    "CountryId='" + countryId + "',StateId='" + stateId + "',CityId='" + cityId + "',Language='" + language + "'," +
                    "Date='" + date + "',Description='" + description + "' where Id ='" + Id + "' ";
                SqlCommand command = new SqlCommand(sqlQuery, Conn);
                command.ExecuteNonQuery();
                Conn.Close();
                Display();
                Clear();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("Delete from StudentInfo where Id='" + Id + "'", Conn);
                cmd.ExecuteNonQuery();
                Conn.Close();
                Display();
                Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           this.Close();
        }
    }
}
