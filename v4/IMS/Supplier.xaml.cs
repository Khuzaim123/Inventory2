using System;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows;

namespace Inventory_managment
{
    public partial class Supplier : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"; 
        private int? currentSupplierId; // To hold the current supplier ID if editing

        public ObservableCollection<PurchaseOrder> PurchaseHistory { get; set; } = new ObservableCollection<PurchaseOrder>();

        public Supplier()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            // Load supplier data if needed (e.g., for editing)
            // For now, let's assume we are adding a new supplier
            ClearFields();
        }

        private void ClearFields()
        {
            txtSupplierName.Clear();
            txtContactName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            currentSupplierId = null;
            PurchaseHistory.Clear();
        }

        private void btnSaveSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text) || string.IsNullOrWhiteSpace(txtContactName.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command;

                    if (currentSupplierId.HasValue)
                    {
                        // Update existing supplier
                        command = new SqlCommand("UPDATE IMS_Suppliers SET SupplierName = @SupplierName, ContactName = @ContactName, Phone = @Phone, Email = @Email, Address = @Address WHERE SupplierID = @SupplierID", connection);
                        command.Parameters.AddWithValue("@SupplierID", currentSupplierId.Value);
                    }
                    else
                    {
                        // Insert new supplier
                        command = new SqlCommand("INSERT INTO IMS_Suppliers (SupplierName, ContactName, Phone, Email, Address) VALUES (@SupplierName, @ContactName, @Phone, @Email, @Address)", connection);
                    }

                    command.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text);
                    command.Parameters.AddWithValue("@ContactName", txtContactName.Text);
                    command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@Address", txtAddress.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Supplier saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPurchaseHistory(int supplierId)
        {
            PurchaseHistory.Clear();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT PurchaseOrderID, OrderDate, TotalAmount, Status FROM IMS_PurchaseOrders WHERE SupplierID = @SupplierID", connection);
                    command.Parameters.AddWithValue("@SupplierID", supplierId);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        PurchaseOrder order = new PurchaseOrder
                        {
                            PurchaseOrderID = reader.GetInt32(0),
                            OrderDate = reader.GetDateTime(1),
                            TotalAmount = reader.GetDecimal(2),
                            Status = reader.GetString(3)
                        };
                        PurchaseHistory.Add(order);
                    }
                }
                dataGridPurchaseHistory.ItemsSource = PurchaseHistory;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading purchase history: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class PurchaseOrder
    {
        public int PurchaseOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}