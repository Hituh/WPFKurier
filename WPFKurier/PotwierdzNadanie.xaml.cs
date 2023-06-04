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
    /// <summary>
    /// Interaction logic for PotwierdzNadanie.xaml
    /// </summary>
    public partial class PotwierdzNadanie : Window
    {
        public PotwierdzNadanie(string successMsg)
        {
            InitializeComponent();
            tbDetails.Text = successMsg;
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Nadaj_Click(object sender, RoutedEventArgs e)
        {


        }
    }
}
