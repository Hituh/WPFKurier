﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CourierApp
{
    public partial class EdytujDane : Window
    {
        private List<string> rozmiaryKopert = new List<string>();
        private List<double> cenyKopert = new List<double>();
        private List<string> rozmiaryPaczek = new List<string>();
        private List<double> cenyPaczek = new List<double>();
        private List<string> wagi = new List<string>();
        private List<double> mnoznikiWag = new List<double>();

        private List<Element> elementsKoperty = new List<Element>();
        private List<Element> elementsPaczki = new List<Element>();
        private List<Element> elementsWagi = new List<Element>();
        public EdytujDane()
        {
            InitializeComponent();
            UpdateLists();
        }

        private void UpdateLists()
        {
            rozmiaryKopert.Clear();
            rozmiaryPaczek.Clear();
            wagi.Clear();
            cenyKopert.Clear();
            cenyPaczek.Clear();
            mnoznikiWag.Clear();
            elementsKoperty.Clear();
            elementsPaczki.Clear();
            elementsWagi.Clear();
            rozmiaryKopert = PobierzRozmiaryKopert();
            cenyKopert = PobierzCenyKopert();
            rozmiaryPaczek = PobierzRozmiaryPaczek();
            cenyPaczek = PobierzCenyPaczek();
            wagi = PobierzWagi();
            mnoznikiWag = PobierzMnoznikiWag();

            for (int i = 0; i < rozmiaryKopert.Count; i++)
            {
                elementsKoperty.Add(new Element
                {
                    Name = rozmiaryKopert[i],
                    Description = cenyKopert[i].ToString("C2")
                });
            }
            elementsKoperty = elementsKoperty.OrderBy(element => element.Name).ToList();

            lbRozmiaryKopert.ItemsSource = elementsKoperty;

            for (int i = 0; i < rozmiaryPaczek.Count; i++)
            {
                elementsPaczki.Add(new Element
                {
                    Name = rozmiaryPaczek[i],
                    Description = cenyPaczek[i].ToString("C2")
                });
            }
            elementsPaczki = elementsPaczki.OrderBy(element => element.Name).ToList();

            lbRozmiaryPaczek.ItemsSource = elementsPaczki;

            for (int i = 0; i < wagi.Count; i++)
            {
                elementsWagi.Add(new Element
                {
                    Name = wagi[i],
                    Description = mnoznikiWag[i].ToString("F2")
                });
            }
            elementsWagi = elementsWagi.OrderBy(element => element.Name).ToList();

            lbWagi.ItemsSource = elementsWagi;
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

        private void DodajWage_Click(object sender, RoutedEventArgs e)
        {
            DodajElement dodajElement = new DodajElement("waga");
            bool? dialogResult = dodajElement.ShowDialog();
            if (dialogResult == true)
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
                string opis = dodajElement.myElement.Name;
                string mnoznik = dodajElement.myElement.Description;
                string tableName = "dbo.Waga";
                int id = FindFreeId(connectionString, tableName, "Id_wagi");

                string query = $"INSERT INTO {tableName} (Id_wagi, Opis, Mnoznik) VALUES ('{id}', '{opis}', '{mnoznik}')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Debug.WriteLine($"Rows affected: {rowsAffected}");

                    UpdateLists();
                }
            }
        }
        private void DodajRozmiarKoperty_Click(object sender, RoutedEventArgs e)
        {
            DodajElement dodajElement = new DodajElement("koperta");
            bool? dialogResult = dodajElement.ShowDialog();
            if (dialogResult == true)
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
                string rozmiar = dodajElement.myElement.Name;
                string cena = dodajElement.myElement.Description;
                string tableName = "dbo.Koperty";
                int id = FindFreeId(connectionString, tableName, "Id_koperty");

                string query = $"INSERT INTO {tableName} (Id_koperty, Rozmiar, Cena) VALUES ('{id}', '{rozmiar}', '{cena}')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Debug.WriteLine($"Rows affected: {rowsAffected}");

                    UpdateLists();
                }
            }

        }
        private void DodajRozmiarPaczki_Click(object sender, RoutedEventArgs e)
        {
            DodajElement dodajElement = new DodajElement("paczka");
            bool? dialogResult = dodajElement.ShowDialog();
            if (dialogResult == true)
            {
                string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
                string rozmiar = dodajElement.myElement.Name;
                string cena = dodajElement.myElement.Description;
                string tableName = "dbo.Paczki";
                int id = FindFreeId(connectionString, tableName, "Id_paczki");

                string query = $"INSERT INTO {tableName} (Id_paczki, Rozmiar, Cena) VALUES ('{id}', '{rozmiar}', '{cena}')";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Debug.WriteLine($"Rows affected: {rowsAffected}");

                    UpdateLists();
                }
            }
        }
        private void Edit_Element(Element newElement, string ogElementName)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "";
            if (rozmiaryKopert.Contains(ogElementName))
            {
                query = $"UPDATE dbo.Koperty SET Rozmiar = '{newElement.Name}', Cena = {newElement.Description} WHERE Rozmiar = '{ogElementName}'";
            }

            if (rozmiaryPaczek.Contains(ogElementName))
            {
                query = $"UPDATE dbo.Paczki SET Rozmiar = '{newElement.Name}', Cena = {newElement.Description} WHERE Rozmiar = '{ogElementName}'";
            }

            if (wagi.Contains(ogElementName))
            {
                query = $"UPDATE dbo.Waga SET Opis = '{newElement.Name}', Mnoznik = {newElement.Description} WHERE Opis = '{ogElementName}'";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Debug.WriteLine(query);
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Debug.WriteLine("Element deleted successfully.");
                }
                else
                {
                    Debug.WriteLine("Element not found or deletion failed.");
                }
            }

        }
        private void Edytuj_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Element ogElement = (Element)button.Tag;
            string type = "";
            if (rozmiaryKopert.Contains(ogElement.Name))
            {
                type = "koperta";
            }
            if (rozmiaryPaczek.Contains(ogElement.Name))
            {
                type = "paczka";
            }
            if (wagi.Contains(ogElement.Name))
            {
                type = "waga";
            }
            EdytujElement edytujElement = new EdytujElement(ogElement, type);
            bool? dialogResult = edytujElement.ShowDialog();

            if (dialogResult == true)
            {
                Element newElement = edytujElement.myElement;
                Edit_Element(newElement, ogElement.Name);
                UpdateLists();
            }
        }
        private void Delete_Element(string element)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "";
            if (rozmiaryKopert.Contains(element))
            {
                query = $"DELETE FROM dbo.Koperty WHERE Rozmiar = '{element}'";
            }

            if (rozmiaryPaczek.Contains(element))
            {
                query = $"DELETE FROM dbo.Paczki WHERE Rozmiar = '{element}'";
            }

            if (wagi.Contains(element))
            {
                query = $"DELETE FROM dbo.Waga WHERE Opis = '{element}'";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Debug.WriteLine(query);
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Debug.WriteLine("Element deleted successfully.");
                }
                else
                {
                    Debug.WriteLine("Element not found or deletion failed.");
                }
            }
        }
        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Element ogElement = (Element)button.Tag;

            if (ogElement != null)
            {
                PotwierdzUsuniecieDanych oknoPotwierdzenia = new PotwierdzUsuniecieDanych(ogElement);

                // Wyświetlanie okna dialogowego i oczekiwanie na wynik
                bool? dialogResult = oknoPotwierdzenia.ShowDialog();


                // Sprawdzenie wyniku dialogu
                if (dialogResult == true)
                {
                    Delete_Element(ogElement.Name);
                    UpdateLists();
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
        private List<double> PobierzCenyKopert()
        {
            List<double> ceny = new List<double>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "SELECT Cena FROM dbo.Koperty";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    double cena = 0.0;
                    double.TryParse(reader["Cena"].ToString(), out cena);
                    ceny.Add(cena);
                }

                reader.Close();
            }

            return ceny;
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
        private List<double> PobierzCenyPaczek()
        {
            List<double> ceny = new List<double>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "SELECT Cena FROM dbo.Paczki";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    double cena = 0.0;
                    double.TryParse(reader["Cena"].ToString(), out cena);
                    ceny.Add(cena);
                }

                reader.Close();
            }

            return ceny;
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
        private List<double> PobierzMnoznikiWag()
        {
            List<double> mnozniki = new List<double>();

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
            string query = "SELECT Mnoznik FROM dbo.Waga";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    double mnoznik = 0.0;
                    double.TryParse(reader["Mnoznik"].ToString(), out mnoznik);
                    mnozniki.Add(mnoznik);

                }

                reader.Close();
            }

            return mnozniki;
        }

        private void rbKopertyCena_Checked(object sender, RoutedEventArgs e)
        {
            if (elementsKoperty != null && lbRozmiaryKopert != null)
            {
                elementsKoperty = elementsKoperty.OrderBy(element => element.Description).ToList();
                lbRozmiaryKopert.ItemsSource = elementsKoperty;
            }

        }

        private void rbKopertyNazwa_Checked(object sender, RoutedEventArgs e)
        {
            if (elementsKoperty != null && lbRozmiaryKopert != null)
            {
                elementsKoperty = elementsKoperty.OrderBy(element => element.Name).ToList();
                lbRozmiaryKopert.ItemsSource = elementsKoperty;
            }
        }

        private void rbKPaczkiNazwa_Checked(object sender, RoutedEventArgs e)
        {
            if (elementsPaczki != null && lbRozmiaryPaczek != null)
            {
                elementsPaczki = elementsPaczki.OrderBy(element => element.Description).ToList();
                lbRozmiaryPaczek.ItemsSource = elementsPaczki;
            }
        }

        private void rbPaczkiCena_Checked(object sender, RoutedEventArgs e)
        {
            if (elementsPaczki != null && lbRozmiaryPaczek != null)
            {
                elementsPaczki = elementsPaczki.OrderBy(element => element.Name).ToList();
                lbRozmiaryPaczek.ItemsSource = elementsPaczki;
            }
        }

        private void rbWagiNazwa_Checked(object sender, RoutedEventArgs e)
        {
            if (elementsWagi != null && lbWagi != null)
            {
                elementsWagi = elementsWagi.OrderBy(element => element.Description).ToList();
                lbWagi.ItemsSource = elementsWagi;
            }
        }

        private void rbWagiMnoznik_Checked(object sender, RoutedEventArgs e)
        {
            if (elementsWagi != null && lbWagi != null)
            {
                elementsWagi = elementsWagi.OrderBy(element => element.Name).ToList();
                lbWagi.ItemsSource = elementsWagi;
            }
        }
        private void tbKopertySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbKopertySearch.Text.ToLower();

            List<Element> filteredElements = elementsKoperty.Where(element => element.Name.ToLower().Contains(searchText) ||
                                                                       element.Description.ToLower().Contains(searchText))
                                                      .ToList();

            lbRozmiaryKopert.ItemsSource = filteredElements;
        }
        private void tbPaczkiSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbPaczkiSearch.Text.ToLower();

            List<Element> filteredElements = elementsPaczki.Where(element => element.Name.ToLower().Contains(searchText) ||
                                                                       element.Description.ToLower().Contains(searchText))
                                                      .ToList();

            lbRozmiaryPaczek.ItemsSource = filteredElements;
        }

        private void tbWagiSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbWagiSearch.Text.ToLower();

            List<Element> filteredElements = elementsWagi.Where(element => element.Name.ToLower().Contains(searchText) ||
                                                                       element.Description.ToLower().Contains(searchText))
                                                      .ToList();

            lbWagi.ItemsSource = filteredElements;
        }


    }
}
