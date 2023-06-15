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
using System.Xml.Linq;

namespace CourierApp
{
    /// <summary>
    /// Interaction logic for EdytujElement.xaml
    /// </summary>
    public partial class EdytujElement : Window
    {
        public Element myElement = new Element();
        public EdytujElement(Element ogElement, string type)
        {
            InitializeComponent();
            if (type == "waga")
            {
                lTitle.Content = "Edytuj wagę:";
                lElementName.Content = "Waga:";
                lElementSecondary.Content = "Mnożnik ceny:";
            }
            if (type == "paczka")
            {
                lTitle.Content = "Edytuj rozmiar paczki:";
                lElementName.Content = "Rozmiar paczki:";
                lElementSecondary.Content = "Cena:";
            }
            if (type == "koperta")
            {
                lTitle.Content = "Edytuj rozmiar koperty:";
                lElementName.Content = "Rozmiar koperty:";
                lElementSecondary.Content = "Cena:";
            }
            tbElementName.Text = ogElement.Name;
            tbElementSecondary.Text = ogElement.Description;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            myElement.Name = tbElementName.Text;
            myElement.Description = tbElementSecondary.Text;


        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
