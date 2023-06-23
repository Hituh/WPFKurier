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

        private List<Package> packages = new List<Package>();

        public MainWindow()
        {
            InitializeComponent();

            // Generate random data for testing
            UpdateDataContext();

            // Set the data context for binding
        }
        private void UpdateDataContext()
        {
            // Get updated data from the database
            GetPackagesFromDatabase();

            // Create a new instance of MainViewModel with the updated data
            MainViewModel viewModel = new MainViewModel(packages);

            // Update the data context
            DataContext = null; // Clear the existing DataContext
            DataContext = viewModel; // Set the new DataContext
        }

        private void GetPackagesFromDatabase()
        {
            packages.Clear();

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

        }

        private List<ElementyPrzesyłki> GetElementsFromDatabase(int przesyłkaId)
        {
            List<ElementyPrzesyłki> elements = new List<ElementyPrzesyłki>();

            string query = "SELECT ElementPrzesyłkiId, Typ, Rozmiar, Waga FROM ElementyPrzesyłki WHERE PrzesyłkaId = @PrzesyłkaId";

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
                            ElementPrzesyłkiId = Convert.ToInt32(reader["ElementPrzesyłkiId"]?.ToString()),
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
            Logowanie logowanieWindow = new Logowanie();

            logowanieWindow.Left = Mouse.GetPosition(this).X;
            logowanieWindow.Top = Mouse.GetPosition(this).Y;
            logowanieWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            logowanieWindow.Owner = this;
            if (logowanieWindow.ShowDialog() == true)
            {
                // Po poprawnym zalogowaniu, otwieranie okna ModyfikujDaneWindow
                EdytujDane modyfikujDaneWindow = new EdytujDane();
                modyfikujDaneWindow.Owner = this;
                modyfikujDaneWindow.ShowDialog();
            }
        }

        private void NadajPrzesylke_Click(object sender, RoutedEventArgs e)
        {
            NadawaniePrzesylki nadajPrzesylkeWindow = new NadawaniePrzesylki();
            if (nadajPrzesylkeWindow != null)
            {
                nadajPrzesylkeWindow.Left = Mouse.GetPosition(this).X;
                nadajPrzesylkeWindow.Top = Mouse.GetPosition(this).Y;
                nadajPrzesylkeWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                nadajPrzesylkeWindow.Owner = this;
                nadajPrzesylkeWindow.ShowDialog();

                // Wywołanie funkcji GetPackagesFromDatabase po zamknięciu okna NadajPrzesylkeWindow
                UpdateDataContext();
            }
        }


        private void EditPackage_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            Package selectedPackage = (Package)editButton.DataContext;

            // Utwórz nowe okno EdytujZamówienie
            EdytujPrzesylke editWindow = new EdytujPrzesylke(selectedPackage);
            editWindow.Closed += EditWindow_Closed; // Dodaj obsługę zdarzenia Closed
            editWindow.Show();

        }

        private void EditWindow_Closed(object sender, EventArgs e)
        {
            // Wywołaj funkcję odświeżania listy po zamknięciu okna EdytujZamówienie
            UpdateDataContext();
        }


        private void DeletePackage_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            Package selectedPackage = (Package)deleteButton.DataContext;

            // Usuń przesyłkę wraz z powiązanymi elementami przesyłki
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Usuń powiązane elementy przesyłki
                string deleteElementsQuery = "DELETE FROM dbo.ElementyPrzesyłki WHERE PrzesyłkaId = @ElementPrzesyłkiId";
                SqlCommand deleteElementsCommand = new SqlCommand(deleteElementsQuery, connection);
                deleteElementsCommand.Parameters.AddWithValue("@ElementPrzesyłkiId", selectedPackage.PrzesyłkaId);
                deleteElementsCommand.ExecuteNonQuery();

                // Usuń przesyłkę
                string deletePackageQuery = "DELETE FROM dbo.Przesyłki WHERE PrzesyłkaId = @PrzesyłkaId";
                SqlCommand deletePackageCommand = new SqlCommand(deletePackageQuery, connection);
                deletePackageCommand.Parameters.AddWithValue("@PrzesyłkaId", selectedPackage.PrzesyłkaId);
                deletePackageCommand.ExecuteNonQuery();
            }

            // Wywołaj zdarzenie lub metodę odświeżania listy na głównym oknie
            UpdateDataContext();
        }


        private void EditSubPackage_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            ElementyPrzesyłki selectedPackage = (ElementyPrzesyłki)editButton.DataContext;

            // Utwórz nowe okno EdytujZamówienie
            EdytujElementPrzesylki editWindow = new EdytujElementPrzesylki(selectedPackage);
            editWindow.Closed += EditWindow_Closed; // Dodaj obsługę zdarzenia Closed
            editWindow.Show();
        }


        private void DeleteSubPackage_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            ElementyPrzesyłki selectedSubPackage = (ElementyPrzesyłki)deleteButton.DataContext;
            // Perform delete action for the selected sub-package
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Usuń powiązane elementy przesyłki
                string deleteElementsQuery = "DELETE FROM dbo.ElementyPrzesyłki WHERE ElementPrzesyłkiId = @ElementPrzesyłkiId";
                SqlCommand deleteElementsCommand = new SqlCommand(deleteElementsQuery, connection);
                deleteElementsCommand.Parameters.AddWithValue("@ElementPrzesyłkiId", selectedSubPackage.ElementPrzesyłkiId);
                deleteElementsCommand.ExecuteNonQuery();
            }
            UpdateDataContext();
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
        public int ElementPrzesyłkiId { get; set; }
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
