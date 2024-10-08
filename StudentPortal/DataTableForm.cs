using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace StudentPortal
{
    public partial class DataTableForm : Form
    {
        SqlConnection con;
        string Connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
        
        public DataTableForm()
        {
            InitializeComponent();
            con = new SqlConnection(Connection);
            //CreateDataTable();
            Display();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Country", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dt = new DataTable();
            adapter.Fill(dt);
            comboBoxCountry.ValueMember = "CountryId";
            comboBoxCountry.DisplayMember = "CountryName";
            comboBoxCountry.DataSource = dt;
            comboBoxState.Enabled = false;
            comboBoxCity.Enabled = false;
            con.Close();

        }
        private void Display()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Testing", con);
            cmd.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void CreateDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn id =new DataColumn("Id");
            id.DataType = typeof(int);
            dt.Columns.Add(id);
            DataColumn name =new DataColumn("Name");
            name.DataType =typeof(string);
            dt.Columns.Add(name);
            DataRow row1 = dt.NewRow();
            row1["Id"] = 1;
            row1["Name"] = "Anant";
            dt.Rows.Add(row1);
            DataRow row2 = dt.NewRow();
            row2["Id"] = 2;
            row2["Name"] = "Harish";
            dataGridView1.DataSource = dt;
            dt.Rows.Add(row2);
        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCountry.SelectedValue.ToString() != null)
            {
                var connectionstate = con.State;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Select * from State where CountryId=@CountryId", con);
                cmd.Parameters.AddWithValue("@CountryId", comboBoxCountry.SelectedValue.ToString());

                cmd.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboBoxState.ValueMember = "StateId";
                comboBoxState.DisplayMember = "StateName";
                comboBoxState.DataSource = dt;
                comboBoxState.Enabled = true;
                comboBoxCity.Enabled = true;
            }
            con.Close();
        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxState.SelectedValue.ToString() != null)
            {
                var connectionstate = con.State;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Select * from City where StateId=@StateId", con);
                cmd.Parameters.AddWithValue("@StateId", comboBoxState.SelectedValue.ToString());
                cmd.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                comboBoxCity.ValueMember = "CityId";
                comboBoxCity.DisplayMember = "CityName";
                comboBoxCity.DataSource = dt;
                comboBoxState.Enabled = true;
                comboBoxCity.Enabled = true;
            }
            con.Close();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            comboBoxCountry.SelectedIndex = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            comboBoxState.SelectedIndex = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            comboBoxCity.SelectedIndex = int.Parse(dataGridView1.Rows[e.RowIndex ].Cells[3].Value.ToString());
        }
    }
}
