using System.Windows;
using is_payment_system.Service;
using is_payment_system.SampleData;
using MySqlConnector;

namespace is_payment_system;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        DatabaseInitializer.Initialize();
        SampleDataPopulator.Populate();
        base.OnStartup(e);
        
        Console.WriteLine("Hello, World!");
        try 
        {
            using var con = DbConnection.CreateOpen();
            
            Console.WriteLine($"Connection State: {con.State}");
            Console.WriteLine("Connected to MySQL!");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }
}