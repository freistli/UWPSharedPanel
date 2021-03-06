﻿using Microsoft.Toolkit.Wpf.UI.XamlHost;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UWPSharedPanel;

namespace TestWPFApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool EventSet = false;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void SharedXamlHost_ChildChanged(object sender, EventArgs e)
        {
            if (EventSet == false)
            {
                 
                WindowsXamlHost windowsXamlHost =  sender as Microsoft.Toolkit.Wpf.UI.XamlHost.WindowsXamlHost; ;

                UWPSharedPanel.RealEstateComponent sharedPanel =
                     windowsXamlHost.GetUwpInternalObject() as UWPSharedPanel.RealEstateComponent;
                sharedPanel.StringChanged += OnStringChanged;

                EventSet = true;
                 
            }

        }
        private void Window_Closed(object sender, EventArgs e)
        {
             
            SharedXamlHost.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            UWPSharedPanel.RealEstateComponent sharedPanel =
                    SharedXamlHost.GetUwpInternalObject() as UWPSharedPanel.RealEstateComponent;

            Notes.Text = sharedPanel.ViewModel.CurrentRealEstate.TotalRating.ToString()+" "+ sharedPanel.ViewModel.CurrentRealEstate.Notes;

            sharedPanel.ViewModel.CurrentRealEstate = new RealEstate
            {
                ConvenieceRating = 4,
                HomeRating = 5,
                LocationRating = 3,
                TotalRating = 2,
                Notes = "Sent from WPF " + DateTime.Now.ToString()
            };
            

            //Notes.Text = sharedPanel.ViewModel.CurrentRealEstate.Notes;

        }

        private void OnStringChanged(object sender, StringChangedEventArgs e)
        {
            var selectionIndex = Notes.SelectionStart;
            Notes.Text = Notes.Text.Insert(selectionIndex, e.notes);
            Notes.SelectionStart = selectionIndex + e.notes.Length;
             
        }
    }
}
