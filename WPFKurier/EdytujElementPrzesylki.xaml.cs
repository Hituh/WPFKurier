using System.Data.SqlClient;
using System.Windows;

namespace CourierApp
{
    public partial class EdytujElementPrzesylki : Window
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=WPF;";
        private ElementyPrzesyłki OriginalElementPrzesylki;
        private ElementyPrzesyłki EditedElementPrzesylki;

        public EdytujElementPrzesylki(ElementyPrzesyłki elementPrzesylki)
        {
            InitializeComponent();
            OriginalElementPrzesylki = elementPrzesylki;
            EditedElementPrzesylki = new ElementyPrzesyłki();
            CopyElementPrzesylkiValues(OriginalElementPrzesylki, EditedElementPrzesylki);
            DataContext = EditedElementPrzesylki;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz wartości wprowadzone przez użytkownika
            EditedElementPrzesylki.Typ = TypTextBox.Text;
            EditedElementPrzesylki.Rozmiar = RozmiarTextBox.Text;
            EditedElementPrzesylki.Waga = WagaTextBox.Text;

            // Wykonaj operacje zapisu lub aktualizacji w bazie danych
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE dbo.ElementyPrzesyłki SET Typ = @Typ, Rozmiar = @Rozmiar, Waga = @Waga WHERE ElementPrzesyłkiId = @ElementPrzesyłkiId";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.Parameters.AddWithValue("@Typ", EditedElementPrzesylki.Typ);
                command.Parameters.AddWithValue("@Rozmiar", EditedElementPrzesylki.Rozmiar);
                command.Parameters.AddWithValue("@Waga", EditedElementPrzesylki.Waga);
                command.Parameters.AddWithValue("@ElementPrzesyłkiId", EditedElementPrzesylki.ElementPrzesyłkiId);

                command.ExecuteNonQuery();
            }

            // Zamknij okno
            Close();
        }

        private void CopyElementPrzesylkiValues(ElementyPrzesyłki source, ElementyPrzesyłki destination)
        {
            destination.ElementPrzesyłkiId = source.ElementPrzesyłkiId;
            destination.Typ = source.Typ;
            destination.Rozmiar = source.Rozmiar;
            destination.Waga = source.Waga;
        }
    }
}
