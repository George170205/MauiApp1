using System;
using Microsoft.Data.SqlClient;

namespace MauiApp1.Data
{
    public static class TestConnection
    {
        private static string connectionString =
            "Server=DESKTOP-ER28UEG\\SQLEXPRESS;" +
            "Database=SchoolMUAI;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";

        public static bool CanConnect()
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
        }
    }
}
