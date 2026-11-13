using System.Configuration;
using System.Data.SqlClient;

namespace TRS_1._1
{
    class baglanti
    {
        public SqlConnection baglantim()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[
                "TRS_1._1.Properties.Settings.admingirisConnectionString"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
