using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient;

namespace Inventory_managment
{
    public partial class Barcode : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=master;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        public Barcode()
        {
            InitializeComponent();

            
            txtBarcode.GotFocus += (s, e) => { placeholderTextBlock.Visibility = Visibility.Collapsed; };
            txtBarcode.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBarcode.Text))
                {
                    placeholderTextBlock.Visibility = Visibility.Visible;
                }
            };
        }

        private void SearchBarcode_Click(object sender, RoutedEventArgs e)
        {
            string barcode = txtBarcode.Text;

            if (string.IsNullOrWhiteSpace(barcode))
            {
                MessageBox.Show("Please enter a barcode number.", "Input Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name, SKU, Category, UnitPrice, Quantity FROM IMS_Products WHERE Barcode = @Barcode";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Barcode", barcode);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Display product details in the TextBlock
                        string productDetails = $"Product Name: {reader.GetString(0)}\n" +
                                                $"SKU: {reader.GetString(1)}\n" +
                                                $"Category: {reader.GetString(2)}\n" +
                                                $"Unit Price: {reader.GetDecimal(3):C}\n" +
                                                $"Quantity: {reader.GetInt32(4)}";
                        productDetailsTextBlock.Text = productDetails;

                        // Load and display the barcode image
                        string imagePath = @"D:\image.jpeg";
                        if (System.IO.File.Exists(imagePath))
                        {
                            barcodeImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                        }
                        else
                        {
                            MessageBox.Show("The barcode image file is missing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No product found for the given barcode.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                        productDetailsTextBlock.Text = string.Empty;
                        barcodeImage.Source = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while searching for the product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}