using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Configuration;
using Microsoft.Identity.Client;

namespace Inventory_managment
{
    public partial class Setting : Window
    {
        private readonly string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=master;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"; // Your connection string
        private DataTable usersTable = new DataTable();
        private DataTable customersTable = new DataTable();
        private DataTable productsTable = new DataTable();
        private DataTable suppliersTable = new DataTable();

        public Setting()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            LoadUsers();
            LoadCustomers();
            LoadProducts();
            LoadSuppliers();
            UpdateCounters();
            LoadLocations();
            LoadSupportTickets();
        }

        #region User Management
        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT UserID, Username, Role, CreatedAt FROM IMS_Users";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                usersTable.Clear();
                adapter.Fill(usersTable);
                dgUsers.ItemsSource = usersTable.DefaultView;
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Prompt for user input using MessageBox
                string username = Microsoft.VisualBasic.Interaction.InputBox("Enter Username:", "Add User");
                string password = Microsoft.VisualBasic.Interaction.InputBox("Enter Password:", "Add User");
                string role = Microsoft.VisualBasic.Interaction.InputBox("Enter Role:", "Add User");

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
                {
                    MessageBox.Show("All fields are required to add a user.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO IMS_Users (Username, PasswordHash, Role, CreatedAt) VALUES (@Username, @Password, @Role, @CreatedAt)", conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password); // Consider hashing the password
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("User  added successfully.");
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}");
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as DataRowView;
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM IMS_Users WHERE UserID = @UserID", conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", selectedUser["UserID"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}");
                }
            }
        }

        private void btnUpdateRole_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as DataRowView;
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user to update.");
                return;
            }

            try
            {
                string newRole = Microsoft.VisualBasic.Interaction.InputBox("Enter new role:", "Update User Role", selectedUser["Role"].ToString());
                if (string.IsNullOrWhiteSpace(newRole))
                {
                    MessageBox.Show("Role cannot be empty.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE IMS_Users SET Role = @Role WHERE UserID = @UserID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Role", newRole);
                        cmd.Parameters.AddWithValue("@UserID", selectedUser["UserID"]);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("User  role updated successfully.");
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user role: {ex.Message}");
            }
        }
        #endregion

        #region Customer Management
        private void LoadCustomers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CustomerID, CustomerName, Email, Phone, Address FROM IMS_Customers";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                customersTable.Clear();
                adapter.Fill(customersTable);
                dgCustomers.ItemsSource = customersTable.DefaultView;
            }
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = dgCustomers.SelectedItem as DataRowView;
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM IMS_Customers WHERE CustomerID = @CustomerID", conn))
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", selectedCustomer["CustomerID"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadCustomers();
                    UpdateCounters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting customer: {ex.Message}");
                }
            }
        }

        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = dgCustomers.SelectedItem as DataRowView;
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to update.");
                return;
            }

            try
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new name:", "Update Customer", selectedCustomer["CustomerName"].ToString());
                string newEmail = Microsoft.VisualBasic.Interaction.InputBox("Enter new email:", "Update Customer", selectedCustomer["Email"].ToString());
                string newPhone = Microsoft.VisualBasic.Interaction.InputBox("Enter new phone:", "Update Customer", selectedCustomer["Phone"].ToString());
                string newAddress = Microsoft.VisualBasic.Interaction.InputBox("Enter new address:", "Update Customer", selectedCustomer["Address"].ToString());

                if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newEmail) || string.IsNullOrWhiteSpace(newPhone) || string.IsNullOrWhiteSpace(newAddress))
                {
                    MessageBox.Show("All fields are required to update the customer.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE IMS_Customers SET CustomerName = @Name, Email = @Email, Phone = @Phone, Address = @Address WHERE CustomerID = @CustomerID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", newName);
                        cmd.Parameters.AddWithValue("@Email", newEmail);
                        cmd.Parameters.AddWithValue("@Phone", newPhone);
                        cmd.Parameters.AddWithValue("@Address", newAddress);
                        cmd.Parameters.AddWithValue("@CustomerID", selectedCustomer["CustomerID"]);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Customer updated successfully.");
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}");
            }
        }
        #endregion

        #region Product Management
        private void LoadProducts()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProductID, Name, SKU, Category, Quantity, UnitPrice FROM IMS_Products";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                productsTable.Clear();
                adapter.Fill(productsTable);
                dgProducts.ItemsSource = productsTable.DefaultView;
            }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = dgProducts.SelectedItem as DataRowView;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM IMS_Products WHERE ProductID = @ProductID", conn))
                        {
                            cmd.Parameters.AddWithValue("@ProductID", selectedProduct["ProductID"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadProducts();
                    UpdateCounters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}");
                }
            }
        }

        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = dgProducts.SelectedItem as DataRowView;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            try
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new product name:", "Update Product", selectedProduct["Name"].ToString());
                string newSKU = Microsoft.VisualBasic.Interaction.InputBox("Enter new SKU:", "Update Product", selectedProduct["SKU"].ToString());
                string newCategory = Microsoft.VisualBasic.Interaction.InputBox("Enter new category:", "Update Product", selectedProduct["Category"].ToString());
                int newQuantity = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Enter new quantity:", "Update Product", selectedProduct["Quantity"].ToString()));
                decimal newUnitPrice = decimal.Parse(Microsoft.VisualBasic.Interaction.InputBox("Enter new unit price:", "Update Product", selectedProduct["UnitPrice"].ToString()));

                if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newSKU) || string.IsNullOrWhiteSpace(newCategory))
                {
                    MessageBox.Show("All fields are required to update the product.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE IMS_Products SET Name = @Name, SKU = @SKU, Category = @Category, Quantity = @Quantity, UnitPrice = @UnitPrice WHERE ProductID = @ProductID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", newName);
                        cmd.Parameters.AddWithValue("@SKU", newSKU);
                        cmd.Parameters.AddWithValue("@Category", newCategory);
                        cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                        cmd.Parameters.AddWithValue("@UnitPrice", newUnitPrice);
                        cmd.Parameters.AddWithValue("@ProductID", selectedProduct["ProductID"]);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Product updated successfully.");
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}");
            }
        }
        #endregion

        #region Supplier Management
        private void LoadSuppliers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SupplierID, SupplierName, ContactName, Phone, Email FROM IMS_Suppliers";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                suppliersTable.Clear();
                adapter.Fill(suppliersTable);
                dgSuppliers.ItemsSource = suppliersTable.DefaultView;
            }
        }

        private void btnDeleteSupplier_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = dgSuppliers.SelectedItem as DataRowView;
            if (selectedSupplier == null)
            {
                MessageBox.Show("Please select a supplier to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM IMS_Suppliers WHERE SupplierID = @SupplierID", conn))
                        {
                            cmd.Parameters.AddWithValue("@SupplierID", selectedSupplier["SupplierID"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadSuppliers();
                    UpdateCounters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting supplier: {ex.Message}");
                }
            }
        }

        private void btnUpdateSupplier_Click(object sender, RoutedEventArgs e)
        {
            var selectedSupplier = dgSuppliers.SelectedItem as DataRowView;
            if (selectedSupplier == null)
            {
                MessageBox.Show("Please select a supplier to update.");
                return;
            }

            try
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new supplier name:", "Update Supplier", selectedSupplier["SupplierName"].ToString());
                string newContact = Microsoft.VisualBasic.Interaction.InputBox("Enter new contact name:", "Update Supplier", selectedSupplier["ContactName"].ToString());
                string newEmail = Microsoft.VisualBasic.Interaction.InputBox("Enter new email:", "Update Supplier", selectedSupplier["Email"].ToString());
                string newAddress = Microsoft.VisualBasic.Interaction.InputBox("Enter new address:", "Update Supplier", selectedSupplier["Address"].ToString());

                if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newContact) || string.IsNullOrWhiteSpace(newEmail) || string.IsNullOrWhiteSpace(newAddress))
                {
                    MessageBox.Show("All fields are required to update the supplier.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE IMS_Suppliers SET SupplierName = @Name, ContactName = @Contact, Email = @Email, Address = @Address WHERE SupplierID = @SupplierID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", newName);
                        cmd.Parameters.AddWithValue("@Contact", newContact);
                        cmd.Parameters.AddWithValue("@Email", newEmail);
                        cmd.Parameters.AddWithValue("@Address", newAddress);
                        cmd.Parameters.AddWithValue("@SupplierID", selectedSupplier["SupplierID"]);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Supplier updated successfully.");
                LoadSuppliers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating supplier: {ex.Message}");
            }
        }
        #endregion

        private void UpdateCounters()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Update total customers
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM IMS_Customers", conn))
                {
                    txtTotalCustomers.Text = cmd.ExecuteScalar().ToString();
                }

                // Update total products
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM IMS_Products", conn))
                {
                    txtTotalProducts.Text = cmd.ExecuteScalar().ToString();
                }

                // Update total suppliers
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM IMS_Suppliers", conn))
                {
                    txtTotalSuppliers.Text = cmd.ExecuteScalar().ToString();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Implement save functionality for any pending changes
            MessageBox.Show("Changes saved successfully!");
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to discard changes?", "Confirm",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void LoadSupportTickets()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT TicketID, Name, Email, IssueType, Description, SubmissionDate, Status FROM IMS_SupportTickets";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable supportTicketsTable = new DataTable();
                    adapter.Fill(supportTicketsTable);
                    dgSupportTickets.ItemsSource = supportTicketsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading support tickets: " + ex.Message);
            }
        }


        private void LoadLocations()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT LocationID, LocationName, Address, Phone FROM IMS_Locations";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable locationsTable = new DataTable();
                    adapter.Fill(locationsTable);
                    dgLocations.ItemsSource = locationsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading locations: " + ex.Message);
            }
        }
    }
}