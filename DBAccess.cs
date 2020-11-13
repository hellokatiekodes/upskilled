namespace Simple_Stock_IT_App
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    class DBAccess
    {
        private static SqlConnection objConnection;
        private static SqlDataAdapter objDataAdapter;

        public static string ConnectionString { get; private set; }

        private static void OpenConnection()
        {
            try
            {
                if (objConnection == null)
                {
                    objConnection = new SqlConnection(@"Data Source=GAUTAM\SQLEXPRESS; Initial Catalog=DemoPOS; User Id=sa; Password=123456");
                    objConnection.Open();
                }
                else
                {
                    if (objConnection.State != ConnectionState.Open)
                    {
                        objConnection = new SqlConnection(@"Data Source=GAUTAM\SQLEXPRESS; Initial Catalog=DemoPOS; User Id=sa; Password=123456");
                        objConnection.Open();
                    }
                }
            }
            catch (Exception ex) { }
        }

        private static void CloseConnection()
        {
            try
            {
                if (!(objConnection == null))
                {
                    if (objConnection.State == ConnectionState.Open)
                    {
                        objConnection.Close();
                        objConnection.Dispose();
                    }
                }
            }
            catch (Exception ex) { }
        }

        public static DataTable FillDataTable(string Query, DataTable Table)
        {

            OpenConnection();
            try
            {
                objDataAdapter = new SqlDataAdapter(Query, objConnection);
                objDataAdapter.Fill(Table);
                objDataAdapter.Dispose();
                CloseConnection();

                return Table;
            }
            catch
            {
                return null;
            }
        }

        private static SqlDataReader ExecuteReader(string cmd)
        {
            try
            {
                SqlDataReader objReader;
                objConnection = new SqlConnection(ConnectionString);
                OpenConnection();
                SqlCommand cmdRedr = new SqlCommand(cmd, objConnection);
                objReader = cmdRedr.ExecuteReader(CommandBehavior.CloseConnection);
                cmdRedr.Dispose();
                return objReader;
            }
            catch
            {
                return null;
            }
        }

        private static bool ExecuteQuery(string query)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}


 