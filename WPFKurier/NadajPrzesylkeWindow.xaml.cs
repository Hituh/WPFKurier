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
    public partial class NadajPrzesylkeWindow : Window
    {
        public NadajPrzesylkeWindow()
        {
            InitializeComponent();
        }

        private void Nadaj_Click(object sender, RoutedEventArgs e)
        {
            // Obsługa przycisku "Nadaj"
            
        }

        private void rbKoperta_Checked(object sender, RoutedEventArgs e)
        {
            if (rbKoperta.IsChecked == true)
            {
                // Wybrano kopertę
                if (cbRozmiarKoperty != null && cbRozmiarPaczki != null) {
                    cbRozmiarKoperty.Visibility = Visibility.Visible;
                    cbRozmiarPaczki.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void rbPaczka_Checked(object sender, RoutedEventArgs e)
        {
            if (rbPaczka.IsChecked == true)
            {
                // Wybrano paczkę
                if(cbRozmiarKoperty != null && cbRozmiarPaczki != null) {
                    cbRozmiarKoperty.Visibility = Visibility.Collapsed;
                    cbRozmiarPaczki.Visibility = Visibility.Visible;
                }
                    
            }
        }
    }
}
