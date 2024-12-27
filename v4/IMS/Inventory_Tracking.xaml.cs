using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace Inventory_managment
{
    public partial class Inventory_Tracking : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=master;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"; 
        public event Action StocksUpdated;

        public Inventory_Tracking()
        {
            InitializeComponent();
            LoadLocations();
            LoadBatches();
            LoadMovementHistory();
        }

        private void LoadLocations()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT LocationName FROM IMS_Locations";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstLocations.Items.Add(reader.GetString(0));
                }
            }
        }

        private void LoadBatches()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT BatchNumber FROM dbo.IMS_ProductBatches";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstBatches.Items.Add(reader.GetString(0));
                }
            }
        }

        private void LoadMovementHistory()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MovementType, Quantity, MovementDate FROM dbo.IMS_StockMovementHistory";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lstMovementHistory.Items.Add($"{reader.GetString(0)}: {reader.GetInt32(1)} on {reader.GetDateTime(2)}");
                }
            }
        }

        private void btnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            string locationName = txtLocationName.Text;
            string address = txtLocationAddress.Text;
            string phone = txtLocationPhone.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO dbo.IMS_Locations (LocationName, Address, Phone) VALUES (@LocationName, @Address, @Phone)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocationName", locationName);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Phone", phone);
                command.ExecuteNonQuery();
            }

            lstLocations.Items.Add(locationName);
            txtLocationName.Clear();
            txtLocationAddress.Clear();
            txtLocationPhone.Clear();
        }

        private void btnAddBatch_Click(object sender, RoutedEventArgs e)
        {
            int productId = int.Parse(txtBatchProductID.Text);
            string batchNumber = txtBatchNumber.Text;
            int quantity = int.Parse(txtBatchQuantity.Text);
            DateTime expiryDate = DateTime.Parse(txtBatchExpiry.Text);
            int locationId = cmbBatchLocation.SelectedIndex + 1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO dbo.IMS_ProductBatches (ProductID, BatchNumber, ExpiryDate, Quantity, LocationID) VALUES (@ProductID, @BatchNumber, @ExpiryDate, @Quantity, @LocationID)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@BatchNumber", batchNumber);
                command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@LocationID", locationId);
                command.ExecuteNonQuery();
            }

            lstBatches.Items.Add(batchNumber);
            txtBatchProductID.Clear();
            txtBatchNumber.Clear();
            txtBatchQuantity.Clear();
            txtBatchExpiry.Clear();
            StocksUpdated?.Invoke(); // Raise the event after adding the batch
        }

        private void btnRecordMovement_Click(object sender, RoutedEventArgs e)
        {
            int productId = int.Parse(txtMovementProductID.Text);
            int quantity = int.Parse(txtMovementQuantity.Text);
            string movementType = ((ComboBoxItem)cmbMovementType.SelectedItem).Content.ToString();
            int sourceLocationId = cmbSourceLocation.SelectedIndex + 1;
            int destinationLocationId = cmbDestinationLocation.SelectedIndex + 1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO dbo.IMS_StockMovementHistory (ProductID, MovementType, Quantity, SourceLocationID, DestinationLocationID) VALUES (@ProductID, @MovementType, @Quantity, @SourceLocationID, @DestinationLocationID)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", productId);
                command.Parameters.AddWithValue("@MovementType", movementType);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@SourceLocationID", sourceLocationId);
                command.Parameters.AddWithValue("@DestinationLocationID", destinationLocationId);
                command.ExecuteNonQuery();
            }

            LoadMovementHistory();
            txtMovementProductID.Clear();
            txtMovementQuantity.Clear();
            cmbMovementType.SelectedIndex = -1;
            cmbSourceLocation.SelectedIndex = -1;
            cmbDestinationLocation.SelectedIndex = -1;
            StocksUpdated?.Invoke(); // Raise the event after recording the movement
        }
    }
}