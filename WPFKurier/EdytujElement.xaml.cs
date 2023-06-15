using System.Windows;

namespace CourierApp
{
    public partial class EdytujElement : Window
    {
        public Element myElement = new Element();

        public EdytujElement(Element ogElement, string type)
        {
            InitializeComponent();

            switch (type)
            {
                case "waga":
                    lTitle.Content = "Edytuj wagę:";
                    lElementName.Content = "Waga:";
                    lElementSecondary.Content = "Mnożnik ceny:";
                    break;
                case "paczka":
                    lTitle.Content = "Edytuj rozmiar paczki:";
                    lElementName.Content = "Rozmiar paczki:";
                    lElementSecondary.Content = "Cena:";
                    break;
                case "koperta":
                    lTitle.Content = "Edytuj rozmiar koperty:";
                    lElementName.Content = "Rozmiar koperty:";
                    lElementSecondary.Content = "Cena:";
                    break;
            }

            tbElementName.Text = ogElement.Name;
            tbElementSecondary.Text = ogElement.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            myElement.Name = tbElementName.Text;
            myElement.Description = tbElementSecondary.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
