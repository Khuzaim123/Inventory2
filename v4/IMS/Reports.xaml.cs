using System;
using System.Data;
using Microsoft.Data.SqlClient; // Updated to use Microsoft.Data.SqlClient
using System.Windows;
using System.Windows.Controls;

namespace Inventory_managment
{
    public partial class Report : Window
    {
        private string connectionString = "Data Source=KHUZAIM-PC;Initial Catalog=project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        public Report()
        {
            InitializeComponent();
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedReport = (ReportTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedReport == null)
            {
                MessageBox.Show("Please select a report type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataTable reportData = null;

            switch (selectedReport)
            {
                case "Inventory Valuation":
                    reportData = GetInventoryValuationReport();
                    break;
                case "Stock Movement":
                    reportData = GetStockMovementReport();
                    break;
                case "Sales and Purchase":
                    reportData = GetSalesAndPurchaseReport();
                    break;
                default:
                    MessageBox.Show("Invalid report type selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            if (reportData != null)
            {
                ReportDataGrid.ItemsSource = reportData.DefaultView;
            }
        }

        private DataTable GetInventoryValuationReport()
        {
            string query = @"
                SELECT 
                    p.Name AS ProductName, 
                    p.Quantity, 
                    p.UnitPrice, 
                    (p.Quantity * p.UnitPrice) AS TotalValue
                FROM IMS_Products p
                ORDER BY p.Name;";

            return ExecuteQuery(query);
        }

        private DataTable GetStockMovementReport()
        {
            string query = @"
                SELECT 
                    sm.MovementDate, 
                    p.Name AS ProductName, 
                    sm.MovementType, 
                    sm.Quantity, 
                    sm.Description
                FROM IMS_StockMovements sm
                INNER JOIN IMS_Products p ON sm.ProductID = p.ProductID
                ORDER BY sm.MovementDate DESC;";

            return ExecuteQuery(query);
        }

        private DataTable GetSalesAndPurchaseReport()
        {
            string query = @"
                SELECT 
                    'Sales' AS TransactionType, 
                    so.OrderDate, 
                    sod.Quantity, 
                    sod.UnitPrice, 
                    (sod.Quantity * sod.UnitPrice) AS TotalAmount, 
                    so.CustomerName
                FROM IMS_SalesOrders so
                INNER JOIN IMS_SalesOrderDetails sod ON so.SalesOrderID = sod.SalesOrderID
                
                UNION ALL
                
                SELECT 
                    'Purchase' AS TransactionType, 
                    po.OrderDate, 
                    pod.Quantity, 
                    pod.UnitPrice, 
                    (pod.Quantity * pod.UnitPrice) AS TotalAmount, 
                    s.SupplierName
                FROM IMS_PurchaseOrders po
                INNER JOIN IMS_PurchaseOrderDetails pod ON po.PurchaseOrderID = pod.PurchaseOrderID
                INNER JOIN IMS_Suppliers s ON po.SupplierID = s.SupplierID
                
                ORDER BY TransactionType, OrderDate;";

            return ExecuteQuery(query);
        }

        private DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return dataTable;
        }

        private void ReportTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle selection change if needed
        }
    }
}