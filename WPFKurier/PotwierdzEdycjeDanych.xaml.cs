using System.Windows;

namespace CourierApp
{
    public partial class PotwierdzEdycjeDanych : Window
    {
        public PotwierdzEdycjeDanych(Element zedytowanyElement, Element ogElement)
        {
            InitializeComponent();
            tbElementOryginalny.Text = ogElement.Name;
            tbElementZmieniony.Text = zedytowanyElement.Name;
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
