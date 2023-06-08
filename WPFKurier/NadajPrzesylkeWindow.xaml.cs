using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace CourierApp
{
    public partial class NadajPrzesylkeWindow : Window
    {
        private PotwierdzNadanie potwierdzNadanieWindow;
        public NadajPrzesylkeWindow()
        {
            InitializeComponent();
            List<string> koperty = PobierzKoperty();
            List<string> paczki = PobierzPaczki();
            List<string> wagi = PobierzWagi();
            cbRozmiarPaczki.ItemsSource = paczki;
            cbRozmiarKoperty.ItemsSource = koperty;
            cbWaga.ItemsSource = wagi;
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

        private void Nadaj_Click(object sender, RoutedEventArgs e)
        {
            // Obsługa przycisku "Nadaj"
            string typPrzesylki = rbKoperta.IsChecked == true ? "Koperta" : "Paczka";
            string rozmiar = "";
            string waga = "";
            string[] daneNadawcy = new string[2];
            string[] daneOdbiorcy = new string[2];

            string successMsg = "";
            string errorMsg = "";

            rozmiarKopertaCheck(ref rozmiar, ref successMsg, ref errorMsg);
            rozmiarPaczkaCheck(ref rozmiar, ref successMsg, ref errorMsg);
            wagaCheck(ref waga, ref successMsg, ref errorMsg);
            daneNadawcyCheck(daneNadawcy, ref successMsg, ref errorMsg);
            daneObiorcyCheck(daneOdbiorcy, ref successMsg, ref errorMsg);

            // Przykładowa logika obsługi danych przesyłki
            if (errorMsg.Length == 0)
            {
                if (potwierdzNadanieWindow == null)
                {
                    potwierdzNadanieWindow = new PotwierdzNadanie(successMsg);
                    potwierdzNadanieWindow.Owner = this;
                    potwierdzNadanieWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    potwierdzNadanieWindow.ShowDialog();
                }
            }
            else
            {
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

        private void wagaCheck(ref string waga, ref string successMsg, ref string errorMsg)
        {
            if (cbWaga.SelectedItem != null)
            {

                waga = cbWaga.SelectedItem.ToString();
                successMsg += $"Waga: {waga}\n";
            }
            else
            {
                errorMsg += "Pole waga nie może być puste\n";
            }
        }

        private void rozmiarPaczkaCheck(ref string rozmiar, ref string successMsg, ref string errorMsg)
        {
            if (rbPaczka.IsChecked == true)
            {
                // Wybrano paczkę
                if (cbRozmiarPaczki.SelectedItem != null)
                {
                    rozmiar = cbRozmiarPaczki.SelectedItem.ToString();
                    successMsg = $"Rozmiar: {rozmiar}\n";
                }
                else
                {
                    errorMsg += "Pole rozmiar przesyłki nie może być puste\n";
                }
            }
        }

        private void rozmiarKopertaCheck(ref string rozmiar, ref string successMsg, ref string errorMsg)
        {
            if (rbKoperta.IsChecked == true)
            {
                // Wybrano kopertę
                if (cbRozmiarKoperty.SelectedItem != null)
                {
                    rozmiar = cbRozmiarKoperty.SelectedItem.ToString();
                    successMsg = $"Rozmiar: {rozmiar}\n";
                }
                else
                {
                    errorMsg += "Pole rozmiar przesyłki nie może być puste\n";
                }
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
