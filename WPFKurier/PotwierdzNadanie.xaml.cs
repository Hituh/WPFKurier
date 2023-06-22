using System.Collections.Generic;
using System.Windows;

namespace CourierApp
{
    public partial class PotwierdzNadanie : Window
    {
        public List<Element> ElementyPrzesylki { get; set; }

        public PotwierdzNadanie()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Nadaj_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }

}
