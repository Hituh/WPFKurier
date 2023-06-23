using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace CourierApp
{
    public partial class EdytujPrzesylke : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
        private Package OriginalElementZamowienia;
        private Package EditedElementZamowienia;

        public EdytujPrzesylke(Package elementZamowienia)
        {
            InitializeComponent();
            OriginalElementZamowienia = elementZamowienia;
            EditedElementZamowienia = new Package();
            CopyPackageValues(OriginalElementZamowienia, EditedElementZamowienia);
            DataContext = EditedElementZamowienia;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Zapisz zmienione wartości do obiektu EditedElementZamowienia
            EditedElementZamowienia.NazwaNadawcy = NazwaNadawcyTextBox.Text;
            EditedElementZamowienia.AdresNadawcy = AdresNadawcyTextBox.Text;
            EditedElementZamowienia.NazwaOdbiorcy = NazwaOdbiorcyTextBox.Text;
            EditedElementZamowienia.AdresOdbiorcy = AdresOdbiorcyTextBox.Text;
            EditedElementZamowienia.Status = StatusTextBox.Text;

            // Aktualizacja tabeli w bazie danych
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE dbo.Przesyłki SET NazwaNadawcy = @NazwaNadawcy, AdresNadawcy = @AdresNadawcy, NazwaOdbiorcy = @NazwaOdbiorcy, AdresOdbiorcy = @AdresOdbiorcy, Status = @Status WHERE PrzesyłkaId = @PrzesyłkaId";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@NazwaNadawcy", EditedElementZamowienia.NazwaNadawcy);
                command.Parameters.AddWithValue("@AdresNadawcy", EditedElementZamowienia.AdresNadawcy);
                command.Parameters.AddWithValue("@NazwaOdbiorcy", EditedElementZamowienia.NazwaOdbiorcy);
                command.Parameters.AddWithValue("@AdresOdbiorcy", EditedElementZamowienia.AdresOdbiorcy);
                command.Parameters.AddWithValue("@Status", EditedElementZamowienia.Status);
                command.Parameters.AddWithValue("@PrzesyłkaId", EditedElementZamowienia.PrzesyłkaId);
                command.ExecuteNonQuery();
            }

            // Zamknij okno
            Close();
        }

        private void CopyPackageValues(Package source, Package destination)
        {
            destination.PrzesyłkaId = source.PrzesyłkaId;
            destination.NazwaNadawcy = source.NazwaNadawcy;
            destination.NazwaOdbiorcy = source.NazwaOdbiorcy;
            destination.AdresNadawcy = source.AdresNadawcy;
            destination.AdresOdbiorcy = source.AdresOdbiorcy;
            destination.DataPrzesyłki = source.DataPrzesyłki;
            destination.Status = source.Status;
            destination.ElementyPrzesyłki = new List<ElementyPrzesyłki>(source.ElementyPrzesyłki);
        }
    }
}
