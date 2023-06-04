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

namespace CourierApp
{
    public partial class NadajPrzesylkeWindow : Window
    {
        private PotwierdzNadanie potwierdzNadanieWindow;
        public NadajPrzesylkeWindow()
        {
            InitializeComponent();
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

                waga = ((ComboBoxItem)cbWaga.SelectedItem).Content.ToString();
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
                    rozmiar = ((ComboBoxItem)cbRozmiarPaczki.SelectedItem).Content.ToString();
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
                    rozmiar = ((ComboBoxItem)cbRozmiarKoperty.SelectedItem).Content.ToString();
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
