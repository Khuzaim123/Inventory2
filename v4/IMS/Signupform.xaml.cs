using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace InventoryApp
{
    public partial class Signupform : Window
    {
        // Use the correct connection string for your database setup
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        public Signupform()
        {
            InitializeComponent();
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This method can be used to perform any specific actions when the role is selected
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // Database constraints (update these values based on your schema)
            int maxUsernameLength = 50; // Update with the actual max length of 'Username' column
            int maxRoleLength = 20;    // Update with the actual max length of 'Role' column

            // Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (username.Length > maxUsernameLength)
            {
                MessageBox.Show($"Username must not exceed {maxUsernameLength} characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (role.Length > maxRoleLength)
            {
                MessageBox.Show($"Role must not exceed {maxRoleLength} characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Hash the password before storing it
            string hashedPassword = HashPassword(password);

            try
            {
                // Use Microsoft.Data.SqlClient.SqlConnection to connect to the database
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the username already exists
                    string checkQuery = "SELECT COUNT(1) FROM IMS_Users WHERE Username = @Username";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add(new SqlParameter("@Username", username));

                        int userExists = (int)checkCommand.ExecuteScalar();

                        if (userExists > 0)
                        {
                            MessageBox.Show("Username is already taken.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // Insert the new user into the database
                    string insertQuery = "INSERT INTO IMS_Users (Username, PasswordHash, Role, CreatedAt) OUTPUT INSERTED.UserID VALUES (@Username, @PasswordHash, @Role, GETDATE())";
                    using (var insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.Add(new SqlParameter("@Username", username));
                        insertCommand.Parameters.Add(new SqlParameter("@PasswordHash", hashedPassword));
                        insertCommand.Parameters.Add(new SqlParameter("@Role", role));

                        object result = insertCommand.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int userId))
                        {
                            MessageBox.Show("Signup successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                            LogAudit(userId, "Signup", "User signed up successfully");

                            // Pass the user role to the Dashboard window
                            Dashboard dashboard = new Dashboard(userId, role);
                            dashboard.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Signup failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            LogAudit(0, "Attempt to signup", "Signup failed");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Message.Contains("String or binary data would be truncated"))
                {
                    MessageBox.Show("One of the input fields exceeds the allowed length. Please review your input and try again.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"A database error occurred: {sqlEx.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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


        // Method to hash the password using SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
