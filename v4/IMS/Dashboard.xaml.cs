using System;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.ComponentModel;
using Inventory_managment;
using System.Windows.Controls;

namespace InventoryApp
{
    public partial class Dashboard : Window, INotifyPropertyChanged
    {
        private int totalStock;
        private string userRole;
        private string welcomeMessage;

        public string WelcomeMessage
        {
            get => welcomeMessage;
            set
            {
                welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        public int TotalStock
        {
            get => totalStock;
            set
            {
                totalStock = value;
                OnPropertyChanged(nameof(TotalStock));
            }
        }

        private int totalSales;
        public int TotalSales
        {
            get => totalSales;
            set
            {
                totalSales = value;
                OnPropertyChanged(nameof(TotalSales));
            }
        }

        private int totalPurchases;
        public int TotalPurchases
        {
            get => totalPurchases;
            set
            {
                totalPurchases = value;
                OnPropertyChanged(nameof(TotalPurchases));
            }
        }

        public ObservableCollection<string> LowStockAlerts { get; set; } = new ObservableCollection<string>();

        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=master;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private int Userid;

        public Dashboard(int userid, string role)
        {
            InitializeComponent();
            LoadDashboardData();
            this.DataContext = this;
            Userid = userid;
            userRole = role;

            // Set welcome message based on role
            switch (role)
            {
                case "Admin":
                    WelcomeMessage = "Welcome to Admin Dashboard ";
                    break;
                case "Manager":
                    WelcomeMessage = "Welcome to Manager Dashboard";
                    break;
                case "Staff":
                    WelcomeMessage = "Welcome to Staff Dashboard";
                    break;
                default:
                    WelcomeMessage = "Dashboard Overview";
                    break;
            }

            SetAccessControl();
            
        }

        private void SetAccessControl()
        {
            // Role-based visibility
            if (userRole == "Admin")
            {
                // Admin can see all buttons
                Products.Visibility = Visibility.Visible;
                Suppliers.Visibility = Visibility.Visible;
                Orders.Visibility = Visibility.Visible;
                Reports.Visibility = Visibility.Visible;
                ScanBarcode.Visibility = Visibility.Visible;
                btnInventoryTracking.Visibility = Visibility.Visible;
                CustomerInfo.Visibility = Visibility.Visible;
            }
            else if (userRole == "Manager")
            {
                Products.Visibility = Visibility.Visible;
                Suppliers.Visibility = Visibility.Visible;
                Orders.Visibility = Visibility.Visible;
                Reports.Visibility = Visibility.Visible;
                btnInventoryTracking.Visibility = Visibility.Visible;
                CustomerInfo.Visibility = Visibility.Visible;
            }
            else if (userRole == "Staff")
            {
                // Staff can only access specific buttons
                Products.Visibility = Visibility.Visible;
                Reports.Visibility = Visibility.Visible;
                ScanBarcode.Visibility = Visibility.Visible;
                CustomerInfo.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Invalid role detected. Access is restricted.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Fetch Total Stock
                    string stockQuery = "SELECT SUM(Quantity) FROM IMS_Products";
                    SqlCommand stockCommand = new SqlCommand(stockQuery, connection);
                    TotalStock = Convert.ToInt32(stockCommand.ExecuteScalar());

                    // Show notification if stock exceeds 1650 or drops below 1600
                    if (TotalStock > 1650)
                    {
                        MessageBox.Show("Warning: Total stock exceeds 1650 units.",
                                        "Stock Alert",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }
                    else if (TotalStock < 1600)
                    {
                        MessageBox.Show("Warning: Total stock is below 1600 units.",
                                        "Stock Alert",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }

                    // Fetch Total Sales
                    string salesQuery = "SELECT SUM(TotalAmount) FROM IMS_SalesOrders";
                    SqlCommand salesCommand = new SqlCommand(salesQuery, connection);
                    TotalSales = Convert.ToInt32(salesCommand.ExecuteScalar());

                    // Show notification if Total Sales exceed 54000
                    if (TotalSales > 54000)
                    {
                        MessageBox.Show("Alert: Total Sales have exceeded 54,000 units.",
                                        "Sales Alert",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }

                    // Fetch Total Purchases
                    string purchasesQuery = "SELECT SUM(TotalAmount) FROM IMS_PurchaseOrders";
                    SqlCommand purchasesCommand = new SqlCommand(purchasesQuery, connection);
                    TotalPurchases = Convert.ToInt32(purchasesCommand.ExecuteScalar());

                    // Show notification if Total Purchases exceed 690000
                    if (TotalPurchases > 690000)
                    {
                        MessageBox.Show("Alert: Total Purchases have exceeded 690,000 units.",
                                        "Purchases Alert",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }

                    // Low Stock Query
                    string lowStockQuery = "SELECT Name, Quantity FROM IMS_Products WHERE Quantity < 10";
                    SqlCommand lowStockCommand = new SqlCommand(lowStockQuery, connection);
                    SqlDataReader reader = lowStockCommand.ExecuteReader();
                    LowStockAlerts.Clear(); // Clear previous alerts

                    bool hasLowStock = false; // Flag to track if there are any low stock items

                    if (reader.HasRows) // Ensure there is data before reading
                    {
                        while (reader.Read())
                        {
                            string productName = reader.GetString(0);
                            int quantity = reader.GetInt32(1);

                            LowStockAlerts.Add($"{productName}: Only {quantity} left in stock!");

                            // If a product is running low, show a system notification
                            hasLowStock = true;
                        }
                    }

                    // Close the reader before executing the next query
                    reader.Close();

                    // Show a notification message if any product has low stock
                    if (hasLowStock)
                    {
                        MessageBox.Show("Warning: Some products have low stock (less than 10). Please check the low stock alerts.",
                                        "Low Stock Alert",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                    }

                    // Check for order status changes (e.g., Pending orders)
                    string orderStatusQuery = "SELECT SalesOrderID, Status FROM IMS_SalesOrders WHERE Status = 'Pending'";
                    SqlCommand orderStatusCommand = new SqlCommand(orderStatusQuery, connection);
                    SqlDataReader orderStatusReader = orderStatusCommand.ExecuteReader();
                    bool hasPendingOrders = false;

                    if (orderStatusReader.HasRows) // If there are pending orders
                    {
                        while (orderStatusReader.Read())
                        {
                            string orderId = orderStatusReader.GetInt32(0).ToString(); // SalesOrderID
                            string status = orderStatusReader.GetString(1);

                            // Adding the pending order to the system message
                            LowStockAlerts.Add($"Order ID: {orderId} is currently {status}. Please review.");
                            hasPendingOrders = true;
                        }
                    }

                    // Close the order status reader
                    orderStatusReader.Close();

                    // Show a notification if there are pending orders
                    if (hasPendingOrders)
                    {
                        MessageBox.Show("Alert: There are pending orders in the system that require attention.",
                                        "Order Status Alert",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }

                    // You can also implement system messages here
                    string systemMessagesQuery = "SELECT Message FROM IMS_SystemMessages WHERE IsActive = 1"; // Example: system messages
                    SqlCommand systemMessagesCommand = new SqlCommand(systemMessagesQuery, connection);
                    SqlDataReader systemMessagesReader = systemMessagesCommand.ExecuteReader();
                    bool hasSystemMessages = false;

                    if (systemMessagesReader.HasRows) // If there are active system messages
                    {
                        while (systemMessagesReader.Read())
                        {
                            string systemMessage = systemMessagesReader.GetString(0);
                            LowStockAlerts.Add($"System Message: {systemMessage}");
                            hasSystemMessages = true;
                        }
                    }

                    // Close the system messages reader
                    systemMessagesReader.Close();

                    // Show system messages if available
                    if (hasSystemMessages)
                    {
                        MessageBox.Show("System Messages: Please check for important updates or actions required.",
                                        "System Messages",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Add this method to update dashboard data explicitly
        public void UpdateDashboardData()
        {
            LoadDashboardData(); // Reload the dashboard data
        }

        private void btnInventoryTracking_Click(object sender, RoutedEventArgs e)
        {
            Inventory_Tracking i = new Inventory_Tracking();
            i.StocksUpdated += LoadDashboardData; // Subscribe to the event
            i.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Product p = new Product(Userid);
            p.Show();
        }

        private void ScanBarcode_Click(object sender, RoutedEventArgs e)
        {
            Barcode b = new Barcode();
            b.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Order o = new Order();
            o.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Supplier supplier = new Supplier();
            supplier.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Report r = new Report();
            r.Show();
        }

        private void CustomerInfo_Click(object sender, RoutedEventArgs e)
        {
            customer customerManagementWindow = new customer();
            customerManagementWindow.Show();
        }

        private void HelpSupport_Click(object sender, RoutedEventArgs e)
        {
            string message = "Welcome to Inventory Management System!\n\n" +
                            "For further assistance, contact:\n" +
                            "233503@students.au.edu.pk\n" +
                            "233798@students.au.edu.pk\n" +
                            "233587@students.au.edu.pk\n\n" +
                            "Did you know?\n" +
                            "• Effective inventory management can reduce storage costs by up to 40%\n" +
                            "• Real-time inventory tracking can prevent 90% of stockouts";

            MessageBox.Show(message, "Welcome to Help & Support", MessageBoxButton.OK, MessageBoxImage.Information);

            HelpSupport helpSupport = new HelpSupport();
            helpSupport.Show();
        }

        private void AuditLog_Click(object sender, RoutedEventArgs e)
        {
            Audit audit = new Audit();
            audit.Show();
        }

        private void sett_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            setting.Show();
        }
    }
}