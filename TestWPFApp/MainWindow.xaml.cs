using Microsoft.Toolkit.Wpf.UI.XamlHost;
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
                WindowsXamlHost windowsXamlHost = (WindowsXamlHost)sender;

                UWPSharedPanel.RealEstateComponent sharedPanel =
                    (UWPSharedPanel.RealEstateComponent)windowsXamlHost.Child;
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
            if (EventSet == false)
            {
                UWPSharedPanel.RealEstateComponent sharedPanel =
                    (UWPSharedPanel.RealEstateComponent)SharedXamlHost.Child;
                sharedPanel.Ratings = new double[] { 5, 4, 3, 4 };
                sharedPanel.SetRating();
                sharedPanel.Notes = "This house is spacious and bright.";
                sharedPanel.SetNotes();
                EventSet = true;
            }
        }

        private void OnStringChanged(object sender, StringChangedEventArgs e)
        {
            var selectionIndex = Notes.SelectionStart;
            Notes.Text = Notes.Text.Insert(selectionIndex, e.notes);
            Notes.SelectionStart = selectionIndex + e.notes.Length;
             
        }
    }
}
