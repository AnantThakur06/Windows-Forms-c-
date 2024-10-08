using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace StudentPortal
{
    public class Common
    {
        SqlConnection Conn;
        string Connection = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

        public Common()
        {
            Conn = new SqlConnection(Connection);
        }
        public DataTable GetCityByStateId(int stateId)
        {
            DataTable dtCity = new DataTable();
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("Select CityId,StateId,CityName from City where StateId=@StateId ", Conn);
                cmd.Parameters.AddWithValue("@StateId", stateId);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtCity);
                DataRow row = dtCity.NewRow();
                row[0] = "0";
                row[1] = "0";
                row[2] = "Please Select";
                dtCity.Rows.InsertAt(row, 0);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Conn.Close();
            }
            return dtCity;
        }
        public DataTable GetStateByCountryId(int countryId)
        {
            DataTable dtState = new DataTable();
            try
            {
                Conn.Open();
                string sql = "Select StateId,CountryId,StateName from State where CountryId=@CountryId";
                SqlCommand cmd = new SqlCommand(sql, Conn);
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtState);
                DataRow row = dtState.NewRow();
                row[0] = "0";
                row[1] = "0";
                row[2] = "Please Select";
                dtState.Rows.InsertAt(row, 0);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Conn.Close();
            }
            return dtState;
        }
        public DataTable FillCountry()
        {
            DataTable dtCountry = new DataTable();
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("Select CountryId,CountryName from Country", Conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dtCountry);

                DataRow row = dtCountry.NewRow();
                row[0] = "0";
                row[1] = "Please Select";
                dtCountry.Rows.InsertAt(row, 0);
                

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Conn.Close();
            }
            return dtCountry;
        }

    }
}
