using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CourierApp
{
    public partial class NadawaniePrzesylki : Window
    {
        private PotwierdzNadanie potwierdzNadanieWindow;
        private List<Element> elementyPrzesylki;

        public NadawaniePrzesylki()
        {
            InitializeComponent();
            List<string> koperty = PobierzKoperty();
            List<string> paczki = PobierzPaczki();
            List<string> wagi = PobierzWagi();
            cbRozmiarPaczki.ItemsSource = paczki;
            cbRozmiarKoperty.ItemsSource = koperty;
            cbWaga.ItemsSource = wagi;
            elementyPrzesylki = new List<Element>();

        }

        private List<string> PobierzPaczki()
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
        private List<string> PobierzKoperty()
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
        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz wybrane wartości z formularza
            string typPrzesylki = rbKoperta.IsChecked == true ? "Koperta" : "Paczka";
            string rozmiar = "";
            string waga = "";

            if (rbKoperta.IsChecked == true)
            {
                if (cbRozmiarKoperty.SelectedItem != null)
                {
                    rozmiar = cbRozmiarKoperty.SelectedItem.ToString();
                }
            }
            else if (rbPaczka.IsChecked == true)
            {
                if (cbRozmiarPaczki.SelectedItem != null)
                {
                    rozmiar = cbRozmiarPaczki.SelectedItem.ToString();
                }
            }

            if (cbWaga.SelectedItem != null)
            {
                waga = cbWaga.SelectedItem.ToString();
            }

            // Walidacja danych
            bool isValid = true;
            StringBuilder validationMessage = new StringBuilder();

            if (string.IsNullOrEmpty(typPrzesylki))
            {
                isValid = false;
                validationMessage.AppendLine("Wybierz typ przesyłki (Koperta/Paczka).");
            }

            if (string.IsNullOrEmpty(rozmiar))
            {
                isValid = false;
                validationMessage.AppendLine("Wybierz rozmiar przesyłki.");
            }

            if (string.IsNullOrEmpty(waga))
            {
                isValid = false;
                validationMessage.AppendLine("Wybierz wagę przesyłki.");
            }

            // Dodawanie elementu przesyłki
            if (isValid)
            {
                Element element = new Element
                {
                    Type = typPrzesylki,
                    Name = rozmiar,
                    Description = waga
                };

                elementyPrzesylki.Add(element);

                lbElementy.ItemsSource = null;
                lbElementy.ItemsSource = elementyPrzesylki;
            }
            else
            {
                // Wyświetl komunikat walidacji na ekranie
                MessageBox.Show(validationMessage.ToString(), "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy jest zaznaczony element do usunięcia
            Button button = (Button)sender;
            Element element = (Element)button.Tag;

            // Usuń element z listy
            elementyPrzesylki.Remove(element);

            // Odśwież powiązanie danych
            lbElementy.Items.Refresh();
        }
        private static int FindFreeId(string connectionString, string tableName, string idColumnName)
        {
            string query = $"SELECT {idColumnName} FROM {tableName}";

            List<int> ids = new List<int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader[idColumnName];
                    ids.Add(id);
                }

                reader.Close();
            }

            int nextFreeId = 1;

            while (ids.Contains(nextFreeId))
            {
                nextFreeId++;
            }

            return nextFreeId;
        }

        private void Nadaj_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";

            // Obsługa przycisku "Nadaj"
            string[] daneNadawcy = new string[2];
            string[] daneOdbiorcy = new string[2];

            string successMsg = "";
            string errorMsg = "";


            daneNadawcyCheck(daneNadawcy, ref successMsg, ref errorMsg);
            daneObiorcyCheck(daneOdbiorcy, ref successMsg, ref errorMsg);

            // Przykładowa logika obsługi danych przesyłki
            if (lbElementy.Items.Count == 0 )
            {
                errorMsg += "W przesyłce nie ma żadnych elementów";
            }
            if (errorMsg.Length == 0)
            {
                // Tworzenie głównego rekordu Przesyłki
                string insertPrzesylkaQuery = @"
            INSERT INTO Przesyłki (PrzesyłkaId, NazwaNadawcy, NazwaOdbiorcy, AdresNadawcy, AdresOdbiorcy, DataPrzesyłki, Status)
            VALUES (@PrzesyłkaId, @NazwaNadawcy, @NazwaOdbiorcy, @AdresNadawcy, @AdresOdbiorcy, GETDATE(), 'nadano');
            SELECT SCOPE_IDENTITY();"; // Pobieranie identyfikatora dodanego rekordu Przesyłki

                int przesyłkaId;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(insertPrzesylkaQuery, connection))
                    {
                        przesyłkaId = FindFreeId(connectionString, "dbo.Przesyłki", "PrzesyłkaId");
                        command.Parameters.AddWithValue("@PrzesyłkaId", przesyłkaId);
                        command.Parameters.AddWithValue("@NazwaNadawcy", daneNadawcy[0]);
                        command.Parameters.AddWithValue("@NazwaOdbiorcy", daneOdbiorcy[0]);
                        command.Parameters.AddWithValue("@AdresNadawcy", daneNadawcy[1]);
                        command.Parameters.AddWithValue("@AdresOdbiorcy", daneOdbiorcy[1]);

                        command.ExecuteNonQuery();

                    }

                    foreach (var item in lbElementy.Items)
                    {
                        Element element = (Element)item;

                        // Tworzenie rekordu ElementyPrzesyłki i powiązanie go z głównym rekordem Przesyłki
                        string insertElementyPrzesylkiQuery = @"
                    INSERT INTO ElementyPrzesyłki (ElementPrzesyłkiId, PrzesyłkaId, Typ, Rozmiar, Waga)
                    VALUES (@ElementPrzesyłkiId, @PrzesyłkaId, @Typ, @Rozmiar, @Waga);";

                        using (SqlCommand command = new SqlCommand(insertElementyPrzesylkiQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ElementPrzesyłkiId", FindFreeId(connectionString, "dbo.ElementyPrzesyłki", "ElementPrzesyłkiId"));

                            command.Parameters.AddWithValue("@PrzesyłkaId", przesyłkaId);
                            command.Parameters.AddWithValue("@Typ", element.Type);
                            command.Parameters.AddWithValue("@Rozmiar", element.Name);
                            command.Parameters.AddWithValue("@Waga", element.Description);

                            command.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }

                // Wyświetlanie komunikatu sukcesu
                MessageBox.Show("Przesyłka została nadana.");

                // Zamknięcie okna potwierdzenia nadania
                this.Close();
            }
            else
            {
                // Wyświetlanie komunikatu błędu
                MessageBox.Show(errorMsg);
            }
        }




        private void daneNadawcyCheck(string[] daneNadawcy, ref string successMsg, ref string errorMsg)
        {
            if (txtAdresNadawcy.Text.Length != 0 && txtNazwaNadawcy.Text.Length != 0)
            {
                daneNadawcy[0] = txtNazwaNadawcy.Text;
                daneNadawcy[1] = txtAdresNadawcy.Text;
                successMsg += $"Nazwa nadawcy: {daneNadawcy[0]}\nAdres nadawcy: {daneNadawcy[1]}\n";
            }
            else
            {
                errorMsg += $"Pola danych nadawcy nie może być puste\n";
            }
        }

        private void daneObiorcyCheck(string[] daneOdbiorcy, ref string successMsg, ref string errorMsg)
        {
            if (txtAdresOdbiorcy.Text.Length != 0 && txtNazwaOdbiorcy.Text.Length != 0)
            {
                daneOdbiorcy[0] = txtNazwaOdbiorcy.Text;
                daneOdbiorcy[1] = txtAdresOdbiorcy.Text;
                successMsg += $"Nazwa odbiorcy: {daneOdbiorcy[0]}\nAdres odbiorcy: {daneOdbiorcy[1]}\n";
            }
            else
            {
                errorMsg += $"Pola danych odbiorcy nie może być puste\n";
            }
        }

        private void rbKoperta_Checked(object sender, RoutedEventArgs e)
        {
            if (rbKoperta.IsChecked == true)
            {
                // Wybrano kopertę
                if (cbRozmiarKoperty != null && cbRozmiarPaczki != null)
                {
                    cbRozmiarKoperty.Visibility = Visibility.Visible;
                    cbRozmiarPaczki.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void rbPaczka_Checked(object sender, RoutedEventArgs e)
        {
            if (rbPaczka.IsChecked == true)
            {
                // Wybrano paczkę
                if (cbRozmiarKoperty != null && cbRozmiarPaczki != null)
                {
                    cbRozmiarKoperty.Visibility = Visibility.Collapsed;
                    cbRozmiarPaczki.Visibility = Visibility.Visible;
                }

            }
        }

    }
}
