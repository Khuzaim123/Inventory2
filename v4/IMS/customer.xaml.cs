using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace InventoryApp
{
    public partial class customer : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=master;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        public customer()
        {
            InitializeComponent();
            LoadCustomerData();
        }

        // Load existing customer data from the database
        private void LoadCustomerData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT CustomerName, Phone, Email, Address FROM IMS_Customers";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    List<Customer> customers = new List<Customer>();
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerName = reader.GetString(0),
                            Phone = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Address = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }

                    CustomerListView.ItemsSource = customers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading customer data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the 'Add Customer' button
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Get customer details from TextBoxes
            string customerName = CustomerNameTextBox.Text;
            string phone = PhoneTextBox.Text;
            string email = EmailTextBox.Text;
            string address = AddressTextBox.Text;

            // Validate inputs
            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO IMS_Customers (CustomerName, Phone, Email, Address) VALUES (@CustomerName, @Phone, @Email, @Address)";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@CustomerName", customerName);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Address", address);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCustomerData(); // Reload the customer data
                    }
                    else
                    {
                        MessageBox.Show("Failed to add customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the 'Delete Customer' button
        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a customer is selected from the ListView
            if (CustomerListView.SelectedItem is Customer selectedCustomer)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM IMS_Customers WHERE CustomerName = @CustomerName";
                        SqlCommand command = new SqlCommand(query, connection);

                        // Use parameter to prevent SQL injection
                        command.Parameters.AddWithValue("@CustomerName", selectedCustomer.CustomerName);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadCustomerData(); // Reload customer data
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the 'Update Customer' button
        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a customer is selected
            if (CustomerListView.SelectedItem is Customer selectedCustomer)
            {
                // Fill the textboxes with the selected customer's details
                CustomerNameTextBox.Text = selectedCustomer.CustomerName;
                PhoneTextBox.Text = selectedCustomer.Phone;
                EmailTextBox.Text = selectedCustomer.Email;
                AddressTextBox.Text = selectedCustomer.Address;

                // Change the Add button to an Update button (optional for UI feedback)
                AddCustomerButton.Content = "Update Customer";
                AddCustomerButton.Click -= AddCustomerButton_Click; // Remove existing Add event handler
                AddCustomerButton.Click += (sender, args) => UpdateCustomer(selectedCustomer); // Add new Update event handler
            }
            else
            {
                MessageBox.Show("Please select a customer to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Function to update the customer data
        private void UpdateCustomer(Customer selectedCustomer)
        {
            string customerName = CustomerNameTextBox.Text;
            string phone = PhoneTextBox.Text;
            string email = EmailTextBox.Text;
            string address = AddressTextBox.Text;

            // Validate inputs
            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE IMS_Customers SET Phone = @Phone, Email = @Email, Address = @Address WHERE CustomerName = @CustomerName";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Add parameters
                    command.Parameters.AddWithValue("@CustomerName", customerName);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Address", address);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCustomerData(); // Reload the customer data
                    }
                    else
                    {
                        MessageBox.Show("Failed to update customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Customer class for data binding
    public class Customer
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}