using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace Inventory_managment
{
    public partial class Order : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=master;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        public Order()
        {
            InitializeComponent();
            LoadReorderAlerts();
        }

        private void LoadReorderAlerts()
        {
            // Load reorder point alerts from the database
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name FROM IMS_Products WHERE Quantity < 10"; // Adjust threshold as needed
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    ObservableCollection<string> alerts = new ObservableCollection<string>();
                    while (reader.Read())
                    {
                        alerts.Add(reader.GetString(0)); // Assuming the column is 'Name'
                    }
                    //ReorderAlertsList.ItemsSource = alerts;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reorder alerts: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddSalesOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO IMS_SalesOrders (CustomerName, OrderDate, Status, TotalAmount) VALUES (@CustomerName, @OrderDate, @Status, @TotalAmount)";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                    command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", (cmbSalesStatus.SelectedItem as ComboBoxItem)?.Content.ToString());
                    command.Parameters.AddWithValue("@TotalAmount", decimal.Parse(txtTotalAmount.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Sales order added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding sales order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddPurchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert query using SupplierID directly
                    string query = "INSERT INTO IMS_PurchaseOrders (SupplierID, OrderDate, Status, TotalAmount) " +
                                   "VALUES (@SupplierID, @OrderDate, @Status, @TotalAmount)";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Use txtSupplierID for Purchase Orders (if it's Supplier ID)
                    command.Parameters.AddWithValue("@SupplierID", int.Parse(txtSupplierID.Text));

                    command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", (cmbPurchaseStatus.SelectedItem as ComboBoxItem)?.Content.ToString());
                    command.Parameters.AddWithValue("@TotalAmount", decimal.Parse(txtPurchaseTotalAmount.Text));

                    command.ExecuteNonQuery();

                    MessageBox.Show("Purchase order added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding purchase order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddSupplier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insert query using SupplierID directly
                    string query = "INSERT INTO IMS_PurchaseOrders (SupplierID, OrderDate, Status, TotalAmount) " +
                                   "VALUES (@SupplierID, @OrderDate, @Status, @TotalAmount)";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Use txtSupplierID for Purchase Orders (if it's Supplier ID)
                    command.Parameters.AddWithValue("@SupplierID", int.Parse(txtSupplierID.Text));

                    command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", (cmbPurchaseStatus.SelectedItem as ComboBoxItem)?.Content.ToString());
                    command.Parameters.AddWithValue("@TotalAmount", decimal.Parse(txtPurchaseTotalAmount.Text));

                    command.ExecuteNonQuery();

                    MessageBox.Show("Purchase order added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding purchase order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnSaveAll_Click(object sender, RoutedEventArgs e)
        {
            // Logic to save all orders and suppliers
            MessageBox.Show("All data has been saved successfully!");
        }
    }
}