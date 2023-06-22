using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourierApp
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";

        public MainWindow()
        {
            InitializeComponent();

            // Generate random data for testing
            List<Package> packages = GetPackagesFromDatabase();

            // Set the data context for binding
            DataContext = new MainViewModel(packages);
        }

        private List<Package> GetPackagesFromDatabase()
        {
            List<Package> packages = new List<Package>();

            string query = "SELECT PrzesyłkaId, NazwaNadawcy, NazwaOdbiorcy, AdresNadawcy, AdresOdbiorcy, DataPrzesyłki, Status FROM Przesyłki";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Package package = new Package
                        {
                            PrzesyłkaId = Convert.ToInt32(reader["PrzesyłkaId"]),
                            NazwaNadawcy = reader["NazwaNadawcy"]?.ToString(),
                            NazwaOdbiorcy = reader["NazwaOdbiorcy"]?.ToString(),
                            AdresNadawcy = reader["AdresNadawcy"]?.ToString(),
                            AdresOdbiorcy = reader["AdresOdbiorcy"]?.ToString(),
                            DataPrzesyłki = reader["DataPrzesyłki"] != DBNull.Value ? Convert.ToDateTime(reader["DataPrzesyłki"]) : (DateTime?)null,
                            Status = reader["Status"]?.ToString()
                        };

                        packages.Add(package);
                    }
                }
            }

            // Fetch elements of each package
            foreach (Package package in packages)
            {
                package.ElementyPrzesyłki = GetElementsFromDatabase(package.PrzesyłkaId);
            }

            return packages;
        }

        private List<ElementyPrzesyłki> GetElementsFromDatabase(int przesyłkaId)
        {
            List<ElementyPrzesyłki> elements = new List<ElementyPrzesyłki>();

            string query = "SELECT Typ, Rozmiar, Waga FROM ElementyPrzesyłki WHERE PrzesyłkaId = @PrzesyłkaId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PrzesyłkaId", przesyłkaId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ElementyPrzesyłki element = new ElementyPrzesyłki
                        {
                            Typ = reader["Typ"]?.ToString(),
                            Rozmiar = reader["Rozmiar"]?.ToString(),
                            Waga = reader["Waga"]?.ToString()
                        };

                        elements.Add(element);
                    }
                }
            }

            return elements;
        }

        private void ModyfikujDane_Click(object sender, RoutedEventArgs e)
        {
            // Otwieranie okna logowania
            LogowanieWindow logowanieWindow = new LogowanieWindow();

            logowanieWindow.Left = Mouse.GetPosition(this).X;
            logowanieWindow.Top = Mouse.GetPosition(this).Y;
            logowanieWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            logowanieWindow.Owner = this;
            if (logowanieWindow.ShowDialog() == true)
            {
                // Po poprawnym zalogowaniu, otwieranie okna ModyfikujDaneWindow
                ModyfikujDaneWindow modyfikujDaneWindow = new ModyfikujDaneWindow();
                modyfikujDaneWindow.Owner = this;
                modyfikujDaneWindow.ShowDialog();
            }
        }

        private void NadajPrzesylke_Click(object sender, RoutedEventArgs e)
        {
            NadajPrzesylkeWindow nadajPrzesylkeWindow = new NadajPrzesylkeWindow();
            if (nadajPrzesylkeWindow != null)
            {
                nadajPrzesylkeWindow.Left = Mouse.GetPosition(this).X;
                nadajPrzesylkeWindow.Top = Mouse.GetPosition(this).Y;
                nadajPrzesylkeWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                nadajPrzesylkeWindow.Owner = this;
                nadajPrzesylkeWindow.ShowDialog();

                // Wywołanie funkcji GetPackagesFromDatabase po zamknięciu okna NadajPrzesylkeWindow
                GetPackagesFromDatabase();
            }
        }


        private void EditPackage_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            Package selectedPackage = (Package)editButton.DataContext;
            // Perform edit action for the selected package
            MessageBox.Show($"Edit package: {selectedPackage.PrzesyłkaId}, {selectedPackage.NazwaNadawcy}, {selectedPackage.NazwaOdbiorcy}, {selectedPackage.DataPrzesyłki}, {selectedPackage.Status}");
        }

        private void DeletePackage_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            Package selectedPackage = (Package)deleteButton.DataContext;
            // Perform delete action for the selected package
            MessageBox.Show($"Delete package: {selectedPackage.PrzesyłkaId}, {selectedPackage.NazwaNadawcy}, {selectedPackage.NazwaOdbiorcy}, {selectedPackage.DataPrzesyłki}, {selectedPackage.Status}");
        }

        private void EditSubPackage_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            ElementyPrzesyłki selectedSubPackage = (ElementyPrzesyłki)editButton.DataContext;
            // Perform edit action for the selected sub-package
            MessageBox.Show($"Edit sub-package: {selectedSubPackage.PrzesyłkaId}, {selectedSubPackage.Typ}, {selectedSubPackage.Rozmiar}, {selectedSubPackage.Waga}");
        }

        private void DeleteSubPackage_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            ElementyPrzesyłki selectedSubPackage = (ElementyPrzesyłki)deleteButton.DataContext;
            // Perform delete action for the selected sub-package
            MessageBox.Show($"Delete sub-package: {selectedSubPackage.PrzesyłkaId}, {selectedSubPackage.Typ}, {selectedSubPackage.Rozmiar}, {selectedSubPackage.Waga}");
        }

    }

    public class Package
    {
        public int PrzesyłkaId { get; set; }
        public string NazwaNadawcy { get; set; }
        public string NazwaOdbiorcy { get; set; }
        public string AdresNadawcy { get; set; }
        public string AdresOdbiorcy { get; set; }
        public DateTime? DataPrzesyłki { get; set; }
        public string Status { get; set; }
        public List<ElementyPrzesyłki> ElementyPrzesyłki { get; set; }

        public Package()
        {
            ElementyPrzesyłki = new List<ElementyPrzesyłki>();
        }
    }

    public class ElementyPrzesyłki
    {
        public string PrzesyłkaId { get; set; }
        public string Typ { get; set; }
        public string Rozmiar { get; set; }
        public string Waga { get; set; }
    }

    public class MainViewModel
    {
        public List<Package> Packages { get; set; }

        public MainViewModel(List<Package> packages)
        {
            Packages = packages;
        }
    }
}
