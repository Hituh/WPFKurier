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
    /// <summary>
    /// Interaction logic for LogowanieWindow.xaml
    /// </summary>
    public partial class LogowanieWindow : Window
    {
        public LogowanieWindow()
        {
            InitializeComponent();
        }
        // LogowanieWindow.xaml.cs
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Zaloguj_Click(sender, e);
            }
        }

        private void Zaloguj_Click(object sender, RoutedEventArgs e)
        {
            string wpisanaNazwaUzytkownika = txtNazwaAdministratora.Text;
            string wpisaneHaslo = txtHaslo.Password;

            // Sprawdzenie poprawności wprowadzonych danych logowania
            if (SprawdzPoprawnoscLogowania(wpisanaNazwaUzytkownika, wpisaneHaslo))
            {
                // Poprawne dane logowania
                DialogResult = true;
                Close();
            }
            else
            {
                // Niepoprawne dane logowania
                MessageBox.Show("Niepoprawna nazwa użytkownika lub hasło.", "Błąd logowania", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNazwaAdministratora.Text = "";
                txtHaslo.Password = "";
                txtNazwaAdministratora.Focus();
            }
        }

        private bool SprawdzPoprawnoscLogowania(string nazwaUzytkownika, string haslo)
        {
            if (nazwaUzytkownika == "1" && haslo == "1")
            {
                return true;
            }
            else { return false; }
        }
    }
}
