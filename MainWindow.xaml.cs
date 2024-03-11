using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SalesManager_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        private SqlDataAdapter da;
        private DataSet set;
        private SqlCommandBuilder cmb;
        private SqlCommand command;
        private string tableName;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connShop"].ConnectionString);
            PopulateListBox();
            tableName = "Products";
            
        }
        private void PopulateListBox()
        {
            List<string> tableNames = GetTableNames();
            List_Box.ItemsSource = tableNames;
            List_Box.SelectionChanged += List_Box_SelectionChanged;
        }



        private List<string> GetTableNames()
        {
            var list = new List<string>();
            connection.Open();
            DataTable schema = connection.GetSchema("Tables");
            connection.Close();
            foreach (DataRow row in schema.Rows)
            {
                list.Add(row[2].ToString());
            }
           
            return list;
        }

        private void Fill_event(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tableName)) return;
            string sql = $"select * from {tableName}";
            da = new SqlDataAdapter(sql, connection);
            cmb = new SqlCommandBuilder(da); // insert update delete
            set = new DataSet();
            da.Fill(set, tableName);
            dataGrid.ItemsSource = set.Tables[tableName].DefaultView;
        }




        private void Update_event(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tableName)) return;
            da.Update(set, tableName);
        }

        private void List_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            if (List_Box.SelectedItem is string selectedItem)
            {
                tableName = selectedItem;
            }
            else
            {
                MessageBox.Show("Selected item is not a valid table name.");
            }

        }


    }
}
