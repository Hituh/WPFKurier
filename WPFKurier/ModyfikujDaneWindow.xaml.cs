using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace CourierApp
{
    public partial class ModyfikujDaneWindow : Window
    {
        public ModyfikujDaneWindow()
        {
            InitializeComponent();
            UpdateLists();
        }

        private void UpdateLists()
        {
            // Pobierz rozmiary kopert z bazy danych i przypisz do listy lbRozmiaryKopert
            List<string> rozmiaryKopert = PobierzRozmiaryKopert();
            lbRozmiaryKopert.ItemsSource = rozmiaryKopert;

            // Pobierz rozmiary paczek z bazy danych i przypisz do listy lbRozmiaryPaczek
            List<string> rozmiaryPaczek = PobierzRozmiaryPaczek();
            lbRozmiaryPaczek.ItemsSource = rozmiaryPaczek;

            // Pobierz wagi z bazy danych i przypisz do listy lbWagi
            List<string> wagi = PobierzWagi();
            lbWagi.ItemsSource = wagi;
        }

        private void DodajWage_Click(object sender, RoutedEventArgs e)
        {
            // Kod obsługi przycisku Edytuj
        }
        private void DodajRozmiarKoperty_Click(object sender, RoutedEventArgs e)
        {
            // Kod obsługi przycisku Edytuj
        }
        private void DodajRozmiarPaczki_Click(object sender, RoutedEventArgs e)
        {
            // Kod obsługi przycisku Edytuj
        }
        private void Edytuj_Click(object sender, RoutedEventArgs e)
        {
            // Kod obsługi przycisku Edytuj
        }

        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz wybrany element z ListBox
            string element = (sender as Button)?.DataContext as string;

            if (element != null)
            {
                // Tworzenie nowego okna dialogowego
                OknoPotwierdzeniaWindow oknoPotwierdzenia = new OknoPotwierdzeniaWindow(element);

                // Wyświetlanie okna dialogowego i oczekiwanie na wynik
                bool? dialogResult = oknoPotwierdzenia.ShowDialog();

                // Sprawdzenie wyniku dialogu
                if (dialogResult == true)
                {
                    UpdateLists();
                    // Usunięcie elementu z listy
                    // ...
                }
            }
        }


        private List<string> PobierzRozmiaryKopert()
        {
            List<string> koperty = new List<string>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "SELECT Rozmiar FROM dbo.Koperty";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string rozmiar = reader["Rozmiar"].ToString();
                    koperty.Add(rozmiar);
                }

                reader.Close();
            }

            return koperty;
        }

        private List<string> PobierzRozmiaryPaczek()
        {
            List<string> paczki = new List<string>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "SELECT Rozmiar FROM dbo.Paczki";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string rozmiar = reader["Rozmiar"].ToString();
                    paczki.Add(rozmiar);
                }

                reader.Close();
            }

            return paczki;
        }

        private List<string> PobierzWagi()
        {
            List<string> wagi = new List<string>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "SELECT Opis FROM dbo.Waga";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string rozmiar = reader["Opis"].ToString();
                    wagi.Add(rozmiar);
                }

                reader.Close();
            }

            return wagi;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
