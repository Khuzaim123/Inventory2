using System;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Collections.Generic;
using InventoryApp;

namespace Inventory_managment
{
    public partial class Product : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        private int id;
        public Product(int id)
        {
            InitializeComponent();
            LoadCategories();
            LoadProducts();  // Load all products initially
            this.id = id;
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
                        command.Parameters.AddWithValue("@TableAffected", "IMS_Products");
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

        private Dashboard GetDashboardWindow()
        {
            // Iterate through all open windows
            foreach (Window window in Application.Current.Windows)
            {
                // Check if the window is of type Dashboard
                if (window is Dashboard dashboard)
                {
                    return dashboard; // Return the found Dashboard window
                }
            }
            return null; // If no Dashboard window is open
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO IMS_Products (Name, SKU, Category, UnitPrice, Quantity) VALUES (@Name, @SKU, @Category, @UnitPrice, @Quantity)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Name", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@SKU", txtSKU.Text);
                    cmd.Parameters.AddWithValue("@Category", cmbCategory.Text);
                    cmd.Parameters.AddWithValue("@UnitPrice", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product added successfully.");
                    LogAudit(id, "added Product", $"Product added {txtProductName.Text}");
                    ClearFields();
                    LoadProducts();  // Reload products after adding

                    // Update the dashboard immediately after adding a product
                    Dashboard dashboard = GetDashboardWindow();
                    if (dashboard != null)
                    {
                        dashboard.UpdateDashboardData(); // Update the dashboard if found
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedItem == null)
                {
                    MessageBox.Show("Please select a product to update.");
                    return;
                }

                DataRowView row = (DataRowView)dgProducts.SelectedItem;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE IMS_Products SET Name = @Name, SKU = @SKU, Category = @Category, UnitPrice = @UnitPrice, Quantity = @Quantity WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Name", txtProductName.Text);
                    cmd.Parameters.AddWithValue("@SKU", txtSKU.Text);
                    cmd.Parameters.AddWithValue("@Category", cmbCategory.Text);
                    cmd.Parameters.AddWithValue("@UnitPrice", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@ProductID", row["ProductID"]);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product updated successfully.");
                    LogAudit(id, "updated Product", $"Productupdated {txtProductName.Text}");
                    ClearFields();
                    LoadProducts();  // Reload products after update

                    // Update the dashboard immediately after updating a product
                    Dashboard dashboard = GetDashboardWindow();
                    if (dashboard != null)
                    {
                        dashboard.UpdateDashboardData(); // Update the dashboard if found
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgProducts.SelectedItem == null)
                {
                    MessageBox.Show("Please select a product to delete.");
                    return;
                }

                DataRowView row = (DataRowView)dgProducts.SelectedItem;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM IMS_Products WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@ProductID", row["ProductID"]);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product deleted successfully.");
                    LogAudit(id, "deleted Product", $"Product deleted {txtProductName.Text}");
                    ClearFields();
                    LoadProducts();  // Reload products after delete

                    // Find the Dashboard window and update it
                    Dashboard dashboard = GetDashboardWindow();
                    if (dashboard != null)
                    {
                        dashboard.UpdateDashboardData(); // Update the dashboard if found
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtProductName.Clear();
            txtSKU.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            cmbCategory.SelectedIndex = -1;
            dgProducts.SelectedItem = null;
        }

        private void LoadProducts(string category = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT ProductID, Name, SKU, Category, UnitPrice, Quantity FROM IMS_Products";

                    // Apply category filter if it's not null or "All"
                    if (!string.IsNullOrEmpty(category) && category != "All")
                    {
                        query += " WHERE Category = @Category";
                    }

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Add parameter for category if necessary
                    if (!string.IsNullOrEmpty(category) && category != "All")
                    {
                        cmd.Parameters.AddWithValue("@Category", category.Trim());
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind data to the DataGrid
                    dgProducts.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}");
            }
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.Items.Clear();
                cmbFilterCategory.Items.Clear();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT DISTINCT Category FROM IMS_Products ORDER BY Category";  // Sorted query
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<string> categories = new List<string>();
                    while (reader.Read())
                    {
                        string category = reader["Category"].ToString().Trim(); // Trim spaces
                        categories.Add(category);
                    }

                    categories.Sort();  // Sorting categories manually (optional if the query already sorts them)

                    foreach (string category in categories)
                    {
                        cmbCategory.Items.Add(category);
                        cmbFilterCategory.Items.Add(category);
                    }

                    // Add "All" as the first item for the filter dropdown
                    cmbFilterCategory.Items.Insert(0, "All");
                    cmbFilterCategory.SelectedIndex = 0; // Default to "All"
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}");
            }
        }

        private void cmbFilterCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This method can be used to handle selection changes if needed
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected category from the ComboBox
            string selectedCategory = cmbFilterCategory.SelectedItem?.ToString().Trim();

            // Load products based on the selected category (including null to show all products)
            LoadProducts(selectedCategory == "All" ? null : selectedCategory);
        }
    }
}