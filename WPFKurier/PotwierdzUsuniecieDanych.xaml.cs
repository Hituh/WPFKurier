using System.Windows;

namespace CourierApp
{
    public partial class PotwierdzUsuniecieDanych : Window
    {
        public PotwierdzUsuniecieDanych(Element element)
        {
            InitializeComponent();
            tbElementName.Text = element.Name;
            tbElementSecondary.Text = element.Description;
        }
        private void Tak_Click(object sender, RoutedEventArgs e)
        {
            // Ustawienie wyniku dialogu na true (potwierdzenie)
            DialogResult = true;
            Close();
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            // Ustawienie wyniku dialogu na false (anulowanie)
            DialogResult = false;
            Close();
        }

    }
}
