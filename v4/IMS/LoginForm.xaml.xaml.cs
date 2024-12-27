using Microsoft.Data.SqlClient;  
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace InventoryApp
{
    public partial class LoginForm : Window
    {  //"Data Source=DESKTOP-GSJ1HJB\\SQLEXPRESS;Initial Catalog=Inventory;Integrated Security=True;Trust Server Certificate=True";
       // Connection string using the appropriate provider
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LogAudit(int userId, string action, string description)
        {
            try
            {
                using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO IMS_AuditLogs (UserID, Action, TableAffected, ActionTime, Description) VALUES (@UserID, @Action, @TableAffected, @ActionTime, @Description)";
                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@Action", action);
                        command.Parameters.AddWithValue("@TableAffected", "Users");
                        command.Parameters.AddWithValue("@ActionTime", DateTime.Now);
                        command.Parameters.AddWithValue("@Description", description);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log audit: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Both username and password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int userId = 0;
            string role = string.Empty;

            // Hash the password for secure comparison with the stored hash
            string hashedPassword = HashPassword(password);

            try
            {
                // Use Microsoft.Data.SqlClient.SqlConnection to connect to the database
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT UserID, Role FROM IMS_Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
                    using (var command = new SqlCommand(query, connection)) // SqlCommand also comes from Microsoft.Data.SqlClient
                    {
                        command.Parameters.Add(new SqlParameter("@Username", username));
                        command.Parameters.Add(new SqlParameter("@PasswordHash", hashedPassword));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Move to the first record
                            {
                                userId = reader.GetInt32(0); // Get UserID
                                role = reader.GetString(1); // Get Role

                                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();

                                LogAudit(userId, "Login", "User logged in successfully");

                                Dashboard dashboard = new Dashboard(userId, role);
                                dashboard.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                                LogAudit(0, "Failed Login attempt", "User login failed");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash); // Return the base64 encoded hash
            }
        }
    }
}