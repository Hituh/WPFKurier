using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace CourierApp
{
    public partial class DodajElement : Window
    {
        public Element myElement = new Element();
        public DodajElement(string type)
        {
            Debug.WriteLine(type);
            InitializeComponent();
            if (type == "waga")
            {
                lTitle.Content = "Dodaj nową wagę:";
                lElementName.Content = "Waga:";
                lElementSecondary.Content = "Mnożnik ceny:";
            }
            if (type == "paczka")
            {
                lTitle.Content = "Dodaj nowy rozmiar paczki:";
                lElementName.Content = "Rozmiar paczki:";
                lElementSecondary.Content = "Cena:";
            }
            if (type == "koperta")
            {
                lTitle.Content = "Dodaj nowy rozmiar koperty:";
                lElementName.Content = "Rozmiar koperty:";
                lElementSecondary.Content = "Cena:";
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
