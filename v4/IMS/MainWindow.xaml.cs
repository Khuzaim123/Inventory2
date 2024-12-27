using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace InventoryApp
{
    public partial class MainWindow:Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Signupform signupform = new Signupform();
             signupform.ShowDialog();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            LoginForm loginform = new LoginForm();  
            loginform.ShowDialog();
            this.Close();

        }
    }
      
}
