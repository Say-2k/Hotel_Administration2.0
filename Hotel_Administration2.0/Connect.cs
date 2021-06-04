using System.Data;
using System.Data.SqlClient;

namespace Hotel_Administration2._0
{
    class Connect
    {
        public static SqlConnection _conn = new SqlConnection(@"Data Source = DESKTOP-NNTMJTC; Initial Catalog = Hotel_Administration; Integrated Security = True");

        public static DataSet Ds = new DataSet();

        public static void Table_Fill(string name, string sql)
        {
            if (Ds.Tables[name] != null)
            {
                Ds.Tables[name].Clear();
            }
            _conn.Open();
            SqlDataAdapter dat = new SqlDataAdapter(sql, _conn);
            dat.Fill(Ds, name);
            _conn.Close();
        }

        public static bool Modification_Execute(string sql)
        {
            SqlCommand comm = new SqlCommand(sql, _conn);
            _conn.Open();
            comm.ExecuteNonQuery();
            _conn.Close();
            return true;
        }
    }
}
