using System.Windows;
using System.Windows.Input;

namespace CourierApp
{
    public partial class Logowanie : Window
    {
        public Logowanie()
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
