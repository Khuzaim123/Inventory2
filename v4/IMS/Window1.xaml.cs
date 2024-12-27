using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace Inventory_managment
{
    public partial class Audit : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        public Audit()
        {
            InitializeComponent();
            LoadAuditLogs();
        }

        private void LoadAuditLogs()
        {
            List<AuditLog> auditLogs = new List<AuditLog>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT LogID, UserID, Action, TableAffected, ActionTime, Description FROM IMS_AuditLogs ORDER BY ActionTime DESC";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        AuditLog log = new AuditLog
                        {
                            LogID = reader.GetInt32(0),
                            UserID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            Action = reader.IsDBNull(2) ? "N/A" : reader.GetString(2),
                            TableAffected = reader.IsDBNull(3) ? "N/A" : reader.GetString(3),
                            ActionTime = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                            Description = reader.IsDBNull(5) ? "N/A" : reader.GetString(5)
                        };
                        auditLogs.Add(log);
                    }

                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("SQL Error: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            if (auditLogs.Count > 0)
            {
                AuditDataGrid.ItemsSource = auditLogs; // Updated to match XAML name
            }
            else
            {
                MessageBox.Show("No logs available.");
            }
        }
    }

    public class AuditLog
    {
        public int LogID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; }
        public string TableAffected { get; set; }
        public DateTime ActionTime { get; set; }
        public string Description { get; set; }
    }
}
