using System.Configuration;
using MySqlConnector;

namespace is_payment_system.Service;

public class DbConnection
{
    public static MySqlConnection CreateOpen()
    {
        var cs = ConfigurationManager.ConnectionStrings["PaymentSystemDB"]?.ConnectionString
                 ?? throw new InvalidOperationException("Missing connection string: PaymentSystemDB (App.config)");

        cs = Environment.ExpandEnvironmentVariables(cs);
        
        var con = new MySqlConnection(cs);
        con.Open();
        return con;
    }
}