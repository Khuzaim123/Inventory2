using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace InventoryApp
{
    public partial class HelpSupport : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=master;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        public HelpSupport()
        {
            InitializeComponent();
        }

        private void SubmitSupport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                IssueTypeComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO IMS_SupportTickets (Name, Email, IssueType, Description, SubmissionDate, Status) 
                                   VALUES (@Name, @Email, @IssueType, @Description, @SubmissionDate, 'New')";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", NameTextBox.Text);
                    command.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                    command.Parameters.AddWithValue("@IssueType", ((ComboBoxItem)IssueTypeComboBox.SelectedItem).Content.ToString());
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@SubmissionDate", DateTime.Now);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Support ticket submitted successfully. We'll contact you soon.",
                                  "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear the form
                    NameTextBox.Clear();
                    EmailTextBox.Clear();
                    IssueTypeComboBox.SelectedIndex = -1;
                    DescriptionTextBox.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting support ticket: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}