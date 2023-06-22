﻿using System;
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