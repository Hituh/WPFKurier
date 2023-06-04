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
    /// <summary>
    /// Interaction logic for ZarzadzajPrzesylkamiWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NadajPrzesylkeWindow nadajPrzesylkeWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NadajPrzesylke_Click(object sender, RoutedEventArgs e)
        {
            if (nadajPrzesylkeWindow == null)
            {
                nadajPrzesylkeWindow = new NadajPrzesylkeWindow();
                nadajPrzesylkeWindow.Owner = this;
                nadajPrzesylkeWindow.Closed += NadajPrzesylkeWindow_Closed;
                nadajPrzesylkeWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                nadajPrzesylkeWindow.ShowDialog();
            }
        }

        private void NadajPrzesylkeWindow_Closed(object sender, EventArgs e)
        {
            nadajPrzesylkeWindow = null;
        }
    }
}
