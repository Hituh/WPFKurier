using System;
using System.Windows;

namespace CourierApp
{
    public partial class DodajElement : Window
    {
        public Element myElement = new Element();

        public DodajElement(string type)
        {
            InitializeComponent();

            switch (type)
            {
                case "waga":
                    lTitle.Content = "Dodaj nową wagę:";
                    lElementName.Content = "Waga:";
                    lElementSecondary.Content = "Mnożnik ceny:";
                    break;
                case "paczka":
                    lTitle.Content = "Dodaj nowy rozmiar paczki:";
                    lElementName.Content = "Rozmiar paczki:";
                    lElementSecondary.Content = "Cena:";
                    break;
                case "koperta":
                    lTitle.Content = "Dodaj nowy rozmiar koperty:";
                    lElementName.Content = "Rozmiar koperty:";
                    lElementSecondary.Content = "Cena:";
                    break;
            }
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
